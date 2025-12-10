using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    // THIS LIST holds the reference to the SpikeTrapDemo scripts you dragged in
    [SerializeField] private List<SpikeTrapDemo> targetSpikeTraps;

    // The function is called when another Collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the colliding object is the Wizard
        if (other.CompareTag("Wizard"))
        {
            Debug.Log("Wizard Cart detected on pressure plate! Deactivating " + targetSpikeTraps.Count + " traps.");

            // 2. Iterate through every trap in the list
            foreach (var trap in targetSpikeTraps)
            {
                // 3. Use the stored reference (trap) to call the function
                if (trap != null && trap.IsUp())
                {
                    trap.ToggleSpikes();
                }
            }
        }
    }
}