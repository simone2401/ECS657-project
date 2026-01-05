using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the object entering is the Character/Wizard
        if (other.CompareTag("Character") || other.CompareTag("Wizard"))
        {
            Debug.Log("Goal Reached! Character has finished the track.");

            // 2. Stop the cart so it doesn't keep looping
            CartController cart = other.GetComponent<CartController>();
            if (cart != null)
            {
                cart.StopMovement();
            }

            // 3. Trigger the level completion event
            LevelEvents.Win();
        }
    }
}