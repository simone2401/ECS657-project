using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        TrapDoor trapDoor = GetComponentInParent<TrapDoor>();
        // player fails if they fall - when the door is open
        if (other.CompareTag("Character")&& trapDoor.isOpen){
            // stop the character moving
            CharacterPos cart = other.GetComponent<CharacterPos>();
            if (cart != null)
            {
            cart.StopMovement();
            Debug.Log("Character fell through the trap door!");
            }
            other.transform.position += Vector3.down * 3f;
            LevelEvents.Fail();
        }
    }
 }
