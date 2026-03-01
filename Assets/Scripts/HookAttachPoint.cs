using UnityEngine;

public class HookAttachPoint : MonoBehaviour
{
    [Header("Attachment Settings")]
    public Transform attachPoint;   // assign empty child at exact hook snap position
    public float snapForce = 1000f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RopeEnd"))
        {
            AttachRope(other.transform);
        }
    }

    private void AttachRope(Transform ropeEnd)
    {
        Rigidbody rb = ropeEnd.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;   // freeze physics
        }

        // Snap position & rotation
        ropeEnd.position = attachPoint.position;
        ropeEnd.rotation = attachPoint.rotation;

        // Parent it to hook
        ropeEnd.SetParent(attachPoint);

        Debug.Log("Rope Attached to Hook");
    }
}