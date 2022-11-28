using Openverse.Input;
using UnityEngine;
using UnityEngine.XR;

public class VRPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public OpenverseDevice leftController;
    public OpenverseDevice rightController;
    public OpenverseDevice headset;

    private void Awake()
    {
        OpenverseInput.AddDeviceConnectionHandler(OnDeviceConnect);
        OpenverseInput.UpdateDevices();
        headset?.GetInputDevice().subsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
    }
    
    void OnDeviceConnect(OpenverseDevice device)
    {
        switch (device.type)
        {
            case OpenverseDeviceType.Headset:
                headset = device;
                break;
            case OpenverseDeviceType.LeftController:
                leftController = device;
                break;    
            case OpenverseDeviceType.RightController:
                rightController = device;
                break;
        }
    }

    private void Update()
    {
        if (headset != null)
        {
            head.localPosition = Vector3.Lerp(head.localPosition,headset.Get<Vector3>("devicePosition"),30f*Time.deltaTime);
            head.localRotation = headset.Get<Quaternion>("centerEyeRotation");
        }

        if (leftController != null)
        {
            leftHand.localPosition = leftController.Get<Vector3>("devicePosition");
            leftHand.localRotation = leftController.Get<Quaternion>("deviceRotation");
        }
        
        if (rightController != null)
        {
            rightHand.localPosition = rightController.Get<Vector3>("devicePosition");
            rightHand.localRotation = rightController.Get<Quaternion>("deviceRotation");
        }
    }
}
