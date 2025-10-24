using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpikesButtonHandler : MonoBehaviour
{
    // ⭐ REMOVED: public CartController characterMovement; 
    // This reference is no longer needed since QTEActivatorTrigger handles activation.

    // Reference to the SpikeController component. This must be set by the Activator.
    // Assuming your final spike script is SpikeController (or you can use SpikeTrapDemo)
    public SpikeTrapDemo linkedSpikes;

    // The amount of time the button is visible before the character hits the spike
    public float activeTime = 1.0f;

    // Timer and state variables
    private float timer = 0f;
    private bool isQTEActive = false;

    void Start()
    {
        // ⭐ Optional: Ensure button is hidden at the start of the scene
        gameObject.SetActive(false);

        // Set up the button listener if not done in the editor
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnButtonClick);
        }
    }

    // Called by the QTEActivatorTrigger.cs script
    public void ActivateQTE()
    {
        gameObject.SetActive(true); // Make the button visible
        timer = 0f;
        isQTEActive = true;
        Debug.Log("QTE Activated!");
    }

    void Update()
    {
        if (isQTEActive)
        {
            timer += Time.deltaTime;

            // If the timer runs out, the player failed to press the button in time
            if (timer >= activeTime)
            {
                MissQTE();
            }
        }
    }

    // Called when the UI button is physically clicked
    public void OnButtonClick()
    {
        if (isQTEActive)
        {
            // Call the toggle function on the linked spike trap
            if (linkedSpikes != null)
            {
                linkedSpikes.ToggleSpikes();
            }
            QTESuccess();
        }
    }

    private void QTESuccess()
    {
        isQTEActive = false;
        gameObject.SetActive(false); // Hide the button
        Debug.Log("QTE Success! Spikes disabled.");
    }

    private void MissQTE()
    {
        isQTEActive = false;
        gameObject.SetActive(false); // Hide the button
        Debug.Log("QTE Missed!");
        // NOTE: The game will fail when the character hits the spikes (HazardTrigger.cs)
        // since the spikes were not lowered.
    }
}