using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    [SerializeField] private List<SpikeTrapDemo> targetSpikeTraps;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the colliding object is the Character
        if (other.CompareTag("Character") || other.CompareTag("Wizard"))
        {
            CartController cart = other.GetComponent<CartController>();

            // Check if we found the CartController and it's a valid object
            if (cart != null)
            {
                bool lethalSpikesFound = false;

                // 2. Iterate through ALL associated traps
                foreach (var trap in targetSpikeTraps)
                {
                    // Check if the individual trap is up
                    if (trap != null && trap.IsUp())
                    {
                        lethalSpikesFound = true; // Mark as dangerous
                        break; // Stop checking other traps, one up is enough to fail
                    }
                }

                // 3. Conditional Failure Block: ONLY FAIL if a lethal spike was found
                if (lethalSpikesFound)
                {
                    cart.StopMovement();
                    Debug.Log("Character got hit by a lethal spike! Failure triggered.");
                    LevelEvents.Fail();
                }
                else
                {
                    // The cart is passing through the trigger zone safely
                    Debug.Log("Spike Trigger detected, but all associated spikes are DOWN. Safe passage.");
                }
            }
        }
    }
}
