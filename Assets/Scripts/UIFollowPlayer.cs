using UnityEngine;

public class UIFollowHead : MonoBehaviour
{
    public Transform head;
    public float distance = 1.5f;     // closer
    public float heightOffset = 0.05f; // small eye offset
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (!head) return;

        // Flatten forward direction
        Vector3 forward = head.forward;
        forward.y = 0;
        forward.Normalize();

        // Target position
        Vector3 targetPosition = head.position + forward * distance;

        // Eye level adjustment
        targetPosition.y = head.position.y + heightOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );

        // Face the player correctly
        transform.rotation = Quaternion.LookRotation(
            transform.position - head.position
        );
    }
}