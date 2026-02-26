using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TubeRenderer))]
public class ChokerRopeSystem : MonoBehaviour
{
    [Header("Hook")]
    public Transform hook;

    [Header("Wire Settings")]
    public int segmentCount = 35;
    public float segmentLength = 0.07f;
    public float wireRadius = 0.015f;
    public Material wireMaterial;

    private Rigidbody[] bodies;
    private TubeRenderer tubeRenderer;

    void Start()
    {
        Physics.defaultSolverIterations = 30;
        Physics.defaultSolverVelocityIterations = 30;

        tubeRenderer = GetComponent<TubeRenderer>();
        SetupTube();
        BuildWire();
    }

    void LateUpdate()
    {
        UpdateTube();
    }

    void SetupTube()
    {
        tubeRenderer.material = wireMaterial;

        var type = typeof(TubeRenderer);

        type.GetField("_radiusOne",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(tubeRenderer, wireRadius);

        type.GetField("_sides",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(tubeRenderer, 18);

        type.GetField("_useWorldSpace",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(tubeRenderer, true);
    }

    void BuildWire()
    {
        bodies = new Rigidbody[segmentCount];

        Rigidbody previousBody = hook.GetComponent<Rigidbody>();
        if (previousBody == null)
            previousBody = hook.gameObject.AddComponent<Rigidbody>();

        previousBody.mass = 15f;
        previousBody.interpolation = RigidbodyInterpolation.Interpolate;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = new GameObject("WireSegment_" + i);
            segment.transform.parent = transform;
            segment.transform.position =
                hook.position + hook.forward * segmentLength * (i + 1);

            Rigidbody rb = segment.AddComponent<Rigidbody>();
            rb.mass = 1.5f;
            rb.drag = 6f;
            rb.angularDrag = 6f;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            SphereCollider col = segment.AddComponent<SphereCollider>();
            col.radius = wireRadius;
            PhysicMaterial mat = new PhysicMaterial();
            mat.dynamicFriction = 1f;
            mat.staticFriction = 1f;
            mat.bounciness = 0f;
            mat.frictionCombine = PhysicMaterialCombine.Maximum;
            col.material = mat;

            ConfigurableJoint joint = segment.AddComponent<ConfigurableJoint>();
            joint.connectedBody = previousBody;

            joint.autoConfigureConnectedAnchor = false;

            joint.anchor = new Vector3(0, 0, 0);
            joint.connectedAnchor = new Vector3(0, 0, 0);

            // 🔥 Correct rope behavior
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = segmentLength;
            joint.linearLimit = limit;

            // NO SPRING (important)
            joint.linearLimitSpring = new SoftJointLimitSpring()
            {
                spring = 0,
                damper = 0
            };

            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Free;

            joint.projectionMode = JointProjectionMode.PositionAndRotation;
            joint.projectionDistance = 0.001f;
            joint.projectionAngle = 1f;

            bodies[i] = rb;
            previousBody = rb;
        }
    }

    void UpdateTube()
    {
        List<Vector3> positions = new List<Vector3>();
        positions.Add(hook.position);

        foreach (var body in bodies)
            positions.Add(body.position);

        tubeRenderer.SetPositions(positions.ToArray());
    }
}