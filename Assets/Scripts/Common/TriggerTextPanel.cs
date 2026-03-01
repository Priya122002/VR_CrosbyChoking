using UnityEngine;
using TMPro;

public class TriggerTextPanel : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;

    [TextArea]
    public string[] messages;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // Makes panel always face camera (stable for XR also)
        Vector3 direction = transform.position - mainCamera.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void SetTriggerCount(int count)
    {
        if (messages.Length == 0) return;

        int index = Mathf.Clamp(count - 1, 0, messages.Length - 1);
        textDisplay.text = messages[index];
    }
}