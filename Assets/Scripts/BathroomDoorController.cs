using UnityEngine;

public class BathroomDoorController : MonoBehaviour
{
    private bool canOpen = true;
    private bool isOpen = false; // Track whether the door is currently open or closed

    [SerializeField] private Animator doorAnimator; 

    public void TryOpenDoor()
    {
        if (canOpen)
        {
            if (!isOpen)
            {
                doorAnimator.SetTrigger("OpenDoor");
                isOpen = true; // Update the state to indicate the door is now open
                Debug.Log("Door is opening.");
            }
            else
            {
                doorAnimator.SetTrigger("CloseDoor");
                isOpen = false; // Update the state to indicate the door is now closed
                Debug.Log("Door is closing.");
            }
        }
        else
        {
            Debug.Log("Door is locked. Cannot interact.");
        }
    }

    public void UnlockDoor()
    {
        canOpen = true;
        Debug.Log("Door is unlocked.");
    }
}
