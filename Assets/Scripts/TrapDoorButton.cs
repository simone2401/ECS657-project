using UnityEngine;

public class TrapDoorButton : MonoBehaviour
{
    public TrapDoor trapDoor;
    private bool isOpen = false;
    private void OnMouseDown(){
        if (trapDoor == null)
        {
            Debug.Log("This button doesn't have a trap door");
            return;
        }
        isOpen = !isOpen;
        trapDoor.ToggleDoor();
    }

}


