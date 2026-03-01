using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEmitter : MonoBehaviour
{
    public UnityEvent<Collider, GameObject> onTriggerStart;
    public UnityEvent<Collider, GameObject> onTriggerEnd;
    [Space()]
    [Header("Checking with Tag/Layer (Optional)")]
    public String targetTag = "";
    public String targetLayer = "";
    public UnityEvent onTriggerStartTag;
    public UnityEvent onTriggerStayTag;
    public UnityEvent onTriggerEndTag;

    private void Start()
    {
        if (GetComponents<Collider>().Length < 1)
        {
            Debug.LogWarning(name + " is missing colliders. Trigger emitter need at least one collider");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject.tag);
        if (targetTag != "" && other.CompareTag(targetTag) ||
            targetLayer != "" && other.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            onTriggerStartTag.Invoke();
        }
        onTriggerStart.Invoke(other, gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (targetTag != "" && other.CompareTag(targetTag) ||
            targetLayer != "" && other.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            onTriggerEndTag.Invoke();
        }
        onTriggerEnd.Invoke(other, gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (targetTag != "" && other.CompareTag(targetTag) ||
            targetLayer != "" && other.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            onTriggerStayTag.Invoke();
        }
    }
}
