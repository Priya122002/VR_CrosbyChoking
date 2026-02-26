using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SyntheticChainSling : MonoBehaviour
{
    [Header("Eye Anchors")]
    public Transform leftEyeAnchor;
    public Transform rightEyeAnchor;

    [Header("Rope Settings")]
    public int segmentCount = 40;
    public int eyeSegmentCount = 10;
    public float segmentLength = 0.07f;
    public float ropeRadius = 0.01f;
    public float eyeRadius = 0.08f;
    public Material ropeMaterial;

    private Rigidbody[] bodies;
    private LineRenderer line;

    void Start()
    {
        Physics.defaultSolverIterations = 200;
        Physics.defaultSolverVelocityIterations = 200;

        BuildRope();
        SetupLineRenderer();
    }

    void LateUpdate()
    {
        UpdateLine();
    }

    void FixedUpdate()
    {
        // Tighten only middle rope
        for (int i = eyeSegmentCount; i < bodies.Length - eyeSegmentCount; i++)
        {
            ConfigurableJoint joint = bodies[i].GetComponent<ConfigurableJoint>();
            if (joint == null) continue;

            float dist = Vector3.Distance(
                bodies[i].position,
                bodies[i - 1].position);

            SoftJointLimitSpring spring = joint.linearLimitSpring;

            if (dist > segmentLength * 0.95f)
            {
                spring.spring = 120000f;
                spring.damper = 8000f;
            }
            else
            {
                spring.spring = 15000f;
                spring.damper = 800f;
            }

            joint.linearLimitSpring = spring;
        }
    }

    void BuildRope()
    {
        bodies = new Rigidbody[segmentCount];
        Vector3 startPos = leftEyeAnchor.position;
        Rigidbody previousBody = null;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            segment.name = "RopeSegment_" + i;

            segment.transform.position =
                startPos + Vector3.right * segmentLength * i;

            segment.transform.localScale =
                new Vector3(ropeRadius, segmentLength / 2f, ropeRadius);

            Rigidbody rb = segment.AddComponent<Rigidbody>();
            rb.mass = 0.25f;
            rb.useGravity = true;
            rb.drag = 6f;
            rb.angularDrag = 6f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            // LEFT EYE LOGIC
            if (i < eyeSegmentCount)
            {
                // Only first and last segment of eye are physics
                if (i != 0 && i != eyeSegmentCount - 1)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
            }

            // RIGHT EYE LOGIC
            if (i >= segmentCount - eyeSegmentCount)
            {
                if (i != segmentCount - eyeSegmentCount &&
                    i != segmentCount - 1)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
            }

            // XR Grab only on non-kinematic bodies
            if (!rb.isKinematic)
            {
                XRGrabInteractable grab = segment.AddComponent<XRGrabInteractable>();
                grab.movementType = XRGrabInteractable.MovementType.VelocityTracking;
                grab.throwOnDetach = false;
            }

            if (previousBody != null)
            {
                ConfigurableJoint joint = segment.AddComponent<ConfigurableJoint>();
                joint.connectedBody = previousBody;
                joint.autoConfigureConnectedAnchor = false;

                joint.anchor = new Vector3(0, segmentLength / 2f, 0);
                joint.connectedAnchor = new Vector3(0, -segmentLength / 2f, 0);

                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Locked;

                SoftJointLimit limit = new SoftJointLimit();
                limit.limit = segmentLength;
                joint.linearLimit = limit;

                joint.linearLimitSpring = new SoftJointLimitSpring()
                {
                    spring = 15000f,
                    damper = 800f
                };

                joint.angularXMotion = ConfigurableJointMotion.Free;
                joint.angularYMotion = ConfigurableJointMotion.Free;
                joint.angularZMotion = ConfigurableJointMotion.Free;

                joint.projectionMode = JointProjectionMode.PositionAndRotation;
                joint.projectionDistance = 0.002f;
                joint.projectionAngle = 1f;
            }

            segment.GetComponent<MeshRenderer>().enabled = false;

            bodies[i] = rb;
            previousBody = rb;
        }

        SetupEyes();
    }

    void SetupEyes()
    {
        CreateFullCircleEye(0, leftEyeAnchor.position);
        CreateFullCircleEye(segmentCount - eyeSegmentCount, rightEyeAnchor.position);
    }

    void CreateFullCircleEye(int startIndex, Vector3 center)
    {
        float angleStep = 360f / (eyeSegmentCount - 1);

        for (int i = 0; i < eyeSegmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);

            Vector3 offset = new Vector3(
                0f,
                -Mathf.Cos(angle) * eyeRadius,
                Mathf.Sin(angle) * eyeRadius
            );

            bodies[startIndex + i].position = center + offset;
        }
    }

    void SetupLineRenderer()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = ropeRadius * 2f;
        line.endWidth = ropeRadius * 2f;
        line.useWorldSpace = true;
        line.material = ropeMaterial;
    }

    void UpdateLine()
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < bodies.Length; i++)
            points.Add(bodies[i].position);

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }
}
