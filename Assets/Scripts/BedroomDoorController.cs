using UnityEngine;

public class BedroomDoorController : MonoBehaviour
{
    private bool canOpen = false;

    [SerializeField] private Animator doorAnimator; 

    public void TryOpenDoor()
    {
        if (canOpen)
        {
            doorAnimator.SetTrigger("OpenDoor"); 
        }
    }

    public void UnlockDoor()
    {
        canOpen = true;
    }
}
