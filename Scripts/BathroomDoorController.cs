using UnityEngine;

public class BathroomDoorController : MonoBehaviour
{
    private bool canOpen = true;
    private bool isOpen = false;

    [SerializeField] private Animator doorAnimator; 

    public void TryOpenDoor()
    {
        if (canOpen)
        {
            if (!isOpen)
            {
                doorAnimator.SetTrigger("OpenDoor");
                isOpen = true;
                Debug.Log("Door is opening.");
            }
            else
            {
                doorAnimator.SetTrigger("CloseDoor");
                isOpen = false;
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
