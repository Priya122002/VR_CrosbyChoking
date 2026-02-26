using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public Transform head;
    public float distance = 1.8f;
    public float heightOffset = 0f;
    public float longPressDuration = 1.5f;

    private float holdTimer = 0f;
    private bool isHolding = false;

    private InputDevice leftController;

    void Start()
    {
        SpawnInFront();
        InitializeLeftController();
    }

    void InitializeLeftController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);

        if (devices.Count > 0)
            leftController = devices[0];
    }

    void Update()
    {
        if (!leftController.isValid)
            InitializeLeftController();

        if (leftController.TryGetFeatureValue(CommonUsages.menuButton, out bool pressed) && pressed)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= longPressDuration)
            {
                SpawnInFront();
                holdTimer = 0f;
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    void SpawnInFront()
    {
        if (head == null) return;

        transform.position =
            head.position +
            head.forward * distance +
            Vector3.up * heightOffset;

        transform.rotation =
            Quaternion.LookRotation(transform.position - head.position);
    }
}