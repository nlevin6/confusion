using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeskLampController : MonoBehaviour
{
    [SerializeField] private bool isLightOn = false; 

    [SerializeField] private UnityEvent lightOnEvent;
    [SerializeField] private UnityEvent lightOffEvent;
    [SerializeField] private Light controlledLight; 

    private void Start()
    {
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

            if (controlledLight != null)
            {
                controlledLight.enabled = false;
            }
            Debug.Log("Desk lamp turned off.");
        }
    }
}
