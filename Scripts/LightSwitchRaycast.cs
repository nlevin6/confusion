using UnityEngine;
using UnityEngine.UI;

public class LightSwitchRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    private LightSwitchController lightSwitchObj;
    private DeskLampController deskLampObj;
    private BedroomDoorController doorBedObj;
    private BathroomDoorController doorBathObj;

    [SerializeField] private Image crosshair;

    private void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out RaycastHit hit, rayLength))
        {
            // Check for all possible interactable objects
            var lightSwitch = hit.collider.GetComponent<LightSwitchController>();
            var deskLamp = hit.collider.GetComponent<DeskLampController>();
            var doorBed = hit.collider.GetComponent<BedroomDoorController>();
            var doorBath = hit.collider.GetComponent<BathroomDoorController>();

            if (lightSwitch != null)
            {
                lightSwitchObj = lightSwitch;
                deskLampObj = null;
                doorBedObj = null;
                doorBathObj = null;
                CrosshairChange(true);
            }
            else if (deskLamp != null)
            {
                deskLampObj = deskLamp;
                lightSwitchObj = null;
                doorBedObj = null;
                doorBathObj = null;
                CrosshairChange(true);
            }
            else if (doorBed != null)
            {
                doorBedObj = doorBed;
                doorBathObj = null;
                lightSwitchObj = null;
                deskLampObj = null;
                CrosshairChange(true);
            }
            else if (doorBath != null)
            {
                doorBathObj = doorBath;
                doorBedObj = null;
                lightSwitchObj = null;
                deskLampObj = null;
                CrosshairChange(true);
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }

        if (lightSwitchObj != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            lightSwitchObj.InteractSwitch();
        }
        else if (deskLampObj != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            deskLampObj.InteractSwitch();
        }
        else if (doorBedObj != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            doorBedObj.TryOpenDoor();
        }
        else if (doorBathObj != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            doorBathObj.TryOpenDoor();
        }
    }

    private void ClearInteraction()
    {
        if (lightSwitchObj != null || deskLampObj != null || doorBedObj != null || doorBathObj != null)
        {
            CrosshairChange(false);
            lightSwitchObj = null;
            deskLampObj = null;
            doorBedObj = null;
            doorBathObj = null;
        }
    }

    void CrosshairChange(bool on)
    {
        if (crosshair != null)
        {
            crosshair.color = on ? Color.red : Color.white;
        }
    }
}
