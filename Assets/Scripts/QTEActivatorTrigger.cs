using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEActivatorTrigger : MonoBehaviour
{
    [Tooltip("Drag the QTE UI Button Handler script here.")]
    public SpikesButtonHandler qteButton;

    [Tooltip("Drag the SpikeController component of the spikes this button controls.")]
    public SpikeTrapDemo linkedSpikes;

    // Flag to prevent the button from re-appearing after it's been triggered once
    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the character (assuming tag is "Character")
        if (other.CompareTag("Character") && !hasBeenTriggered)
        {
            if (qteButton != null && linkedSpikes != null)
            {
                // 1. Pass the correct spike reference to the button handler
                qteButton.linkedSpikes = linkedSpikes;

                // 2. Activate the QTE button
                qteButton.ActivateQTE();

                hasBeenTriggered = true;

                // OPTIONAL: Disable this script's collider immediately after activation
                // GetComponent<Collider>().enabled = false;
            }
            else
            {
                Debug.LogError("QTE Activator is missing links to Button or Spikes!");
            }
        }
    }
}
