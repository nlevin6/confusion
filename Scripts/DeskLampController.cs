using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeskLampController : MonoBehaviour
{
    [SerializeField] private bool isLightOn = false; // Set this to false to start with the light off

    [SerializeField] private UnityEvent lightOnEvent;
    [SerializeField] private UnityEvent lightOffEvent;

    // Reference to the controlled light
    [SerializeField] private Light controlledLight; 

    private void Start()
    {
        // Set the light's enabled state based on isLightOn, without invoking events
        if (controlledLight != null)
        {
            controlledLight.enabled = isLightOn;
        }
    }

    public void InteractSwitch()
    {
        if (!isLightOn)
        {
            isLightOn = true;
            lightOnEvent.Invoke();

            // Ensure the light component is turned on
            if (controlledLight != null)
            {
                controlledLight.enabled = true;
            }
            Debug.Log("Desk lamp turned on.");
        }
        else
        {
            isLightOn = false;
            lightOffEvent.Invoke();

            // Ensure the light component is turned off
            if (controlledLight != null)
            {
                controlledLight.enabled = false;
            }
            Debug.Log("Desk lamp turned off.");
        }
    }
}
