using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class Rope : MonoBehaviour
{
    [Header("Hook")]
    public Transform hook;

    [Header("Rope Settings")]
    public int segmentCount = 35;
    public float segmentLength = 0.08f;
    public float ropeRadius = 0.01f;
    public Material ropeMaterial;

    private Rigidbody[] bodies;
    private LineRenderer line;

    void Start()
    {
        // Strong solver for lifting stability
        Physics.defaultSolverIterations = 200;
        Physics.defaultSolverVelocityIterations = 200;

        BuildRope();
        SetupLineRenderer();
    }

    void LateUpdate()
    {
        UpdateSmoothLine();
    }

    void FixedUpdate()
    {
        // 🔥 Tension-based stiffening (real sling behavior)
        for (int i = 1; i < bodies.Length; i++)
        {
            ConfigurableJoint joint = bodies[i].GetComponent<ConfigurableJoint>();
            if (joint == null) continue;

            float dist = Vector3.Distance(
                bodies[i].position,
                bodies[i - 1].position);

            SoftJointLimitSpring spring = joint.linearLimitSpring;

            if (dist > segmentLength * 0.97f)
            {
                // Under load → very stiff
                spring.spring = 90000f;
                spring.damper = 5000f;
            }
            else
            {
                // Slack → flexible
                spring.spring = 12000f;
                spring.damper = 400f;
            }

            joint.linearLimitSpring = spring;
        }
    }

    void BuildRope()
    {
        bodies = new Rigidbody[segmentCount];

        Rigidbody previousBody = hook.GetComponent<Rigidbody>();

        if (previousBody == null)
            previousBody = hook.gameObject.AddComponent<Rigidbody>();

        previousBody.mass = 5f;
        previousBody.useGravity = true;
        previousBody.isKinematic = false;
        previousBody.interpolation = RigidbodyInterpolation.Interpolate;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            segment.name = "RopeSegment_" + i;

            // ✅ MAKE SEGMENT CHILD OF THIS OBJECT
            segment.transform.SetParent(this.transform, true);

            segment.transform.position =
                hook.position + hook.forward * segmentLength * (i + 1);

            segment.transform.localScale =
                new Vector3(ropeRadius, segmentLength / 2f, ropeRadius);

            Rigidbody rb = segment.AddComponent<Rigidbody>();
            rb.mass = 0.25f;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.drag = 6f;
            rb.angularDrag = 6f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.solverIterations = 150;
            rb.solverVelocityIterations = 150;

            // Make segment grabbable
            XRGrabInteractable grab = segment.AddComponent<XRGrabInteractable>();
            grab.movementType = XRGrabInteractable.MovementType.VelocityTracking;
            grab.throwOnDetach = false;

            ConfigurableJoint joint = segment.AddComponent<ConfigurableJoint>();
            joint.connectedBody = previousBody;
            joint.autoConfigureConnectedAnchor = false;

            joint.anchor = new Vector3(0, segmentLength / 2f, 0);
            joint.connectedAnchor = new Vector3(0, -segmentLength / 2f, 0);

            // Length constraint
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Locked;

            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = segmentLength;
            joint.linearLimit = limit;

            joint.linearLimitSpring = new SoftJointLimitSpring()
            {
                spring = 12000f,
                damper = 400f
            };

            // Allow natural curve
            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Free;

            joint.projectionMode = JointProjectionMode.PositionAndRotation;
            joint.projectionDistance = 0.0005f;
            joint.projectionAngle = 0.05f;

            segment.GetComponent<MeshRenderer>().enabled = false;

            bodies[i] = rb;
            previousBody = rb;
        }

        StartCoroutine(DelayedEyeConnection());
    }
    IEnumerator DelayedEyeConnection()
    {
        yield return new WaitForFixedUpdate();
        CreateEyeConnections();
    }

    void CreateEyeConnections()
    {
        int eyeIndex = 9;

        if (bodies.Length <= eyeIndex + 1)
            return;

        ConfigurableJoint firstEyeJoint = bodies[0].gameObject.AddComponent<ConfigurableJoint>();
        firstEyeJoint.connectedBody = bodies[eyeIndex];
        ConfigureEyeJoint(firstEyeJoint);

        int lastIndex = bodies.Length - 1;
        int connectIndex = bodies.Length - 1 - eyeIndex;

        ConfigurableJoint lastEyeJoint = bodies[lastIndex].gameObject.AddComponent<ConfigurableJoint>();
        lastEyeJoint.connectedBody = bodies[connectIndex];
        ConfigureEyeJoint(lastEyeJoint);
    }

    void ConfigureEyeJoint(ConfigurableJoint joint)
    {
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;

        joint.projectionMode = JointProjectionMode.PositionAndRotation;
        joint.projectionDistance = 0.0005f;
        joint.projectionAngle = 0.05f;
    }

    void SetupLineRenderer()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = ropeRadius * 2f;
        line.endWidth = ropeRadius * 2f;
        line.numCapVertices = 20;
        line.useWorldSpace = true;
        line.material = ropeMaterial;
    }

    void UpdateSmoothLine()
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(hook.position);

        for (int i = 0; i < bodies.Length; i++)
            points.Add(bodies[i].position);

        List<Vector3> smooth = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? points[i] : points[i - 1];
            Vector3 p1 = points[i];
            Vector3 p2 = points[i + 1];
            Vector3 p3 = i + 2 < points.Count ? points[i + 2] : p2;

            for (int j = 0; j < 5; j++)
            {
                float t = j / 5f;
                smooth.Add(GetCatmullRomPosition(t, p0, p1, p2, p3));
            }
        }

        line.positionCount = smooth.Count;
        line.SetPositions(smooth.ToArray());
    }

    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }
}