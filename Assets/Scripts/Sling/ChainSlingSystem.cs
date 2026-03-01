using UnityEngine;

public class ChainSlingSystem : MonoBehaviour
{
    public float penetrationFixForce = 5f;
    public float groundCheckDistance = 0.05f;

    private Rigidbody[] rigidbodies;

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            if (rb.mass < 0.2f)
                rb.mass = 0.5f;
        }
    }
    void FixedUpdate()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            RaycastHit hit;

            if (Physics.Raycast(rb.position + Vector3.up * 0.05f,
                                Vector3.down,
                                out hit,
                                0.2f))
            {
                if (hit.collider.GetComponent<TerrainCollider>() != null)
                {
                    float bottomOffset = 0.03f; // approx half capsule radius
                    float targetY = hit.point.y + bottomOffset;

                    if (rb.position.y < targetY)
                    {
                        Vector3 correctedPos = rb.position;
                        correctedPos.y = targetY;

                        rb.MovePosition(correctedPos);

                        rb.velocity = new Vector3(
                            rb.velocity.x,
                            0f,
                            rb.velocity.z
                        );
                    }
                }
            }
        }
    }
}