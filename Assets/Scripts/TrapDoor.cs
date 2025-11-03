using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public bool isOpen = false; // the door is closed to start
    public GameObject openTrapDoor;
    public GameObject closedTrapDoor;
    public int gridX;
    public int gridZ;

    // Start is called before the first frame update
    void Start()
    {
        UpdateState();
    }
    public void ToggleDoor() { 
        isOpen = !isOpen;
        UpdateState();
    }
    public void SetDoorState(bool open) {
        isOpen = open;
        UpdateState();
    }

    private void UpdateState() { 
        if(openTrapDoor != null) { 
            openTrapDoor.SetActive(!isOpen);
        }
        if (closedTrapDoor != null){
            closedTrapDoor.SetActive(!isOpen);
        }
    }
}
