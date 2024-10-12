using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightSwitchController : MonoBehaviour
{
    [SerializeField] private bool isLightOn = false;

    [SerializeField] private UnityEvent lightOnEvent;
    [SerializeField] private Light controlledLight;
    [SerializeField] private BedroomDoorController doorController;

    private void Start()
    {
        if (controlledLight != null)
        {
            controlledLight.enabled = isLightOn;
        }

        if (isLightOn)
        {
            lightOnEvent.Invoke();
        }
    }

    public void InteractSwitch()
    {
        if (!isLightOn)
        {
            isLightOn = true;
            controlledLight.enabled = true;
            lightOnEvent.Invoke();

            if (doorController != null)
            {
                doorController.UnlockDoor();
                Debug.Log("Light turned on, door unlocked.");
            }
        }
        else
        {
            Debug.Log("Light is already on. It cannot be turned off.");
        }
    }
}
