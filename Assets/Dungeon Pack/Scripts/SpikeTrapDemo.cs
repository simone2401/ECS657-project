using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapDemo : MonoBehaviour {
    public Animator spikeTrapAnim; // Animator for the SpikeTrap;

    // State tracking variable
    private bool spikesAreUp = true;

    void Awake()
    {
        // Get the Animator component from the trap;
        spikeTrapAnim = GetComponent<Animator>();

        // Play the open animation once to ensure spikes start UP
        //spikeTrapAnim.SetTrigger("open");

        // Immediately stop the automatic cycling Coroutine
        // Stop all coroutines on this script just in case
        StopAllCoroutines();
    }

    // Simplified function called by the QTE Button to lower the spikes
    public void ToggleSpikes()
    {
        // Only run the animation if the spikes are currently up
        if (spikesAreUp)
        {
            // Play the close animation
            spikeTrapAnim.SetTrigger("close");

            // Update the state so the spikes won't trigger again
            spikesAreUp = false;
            Debug.Log("Spikes are now DOWN");
        }
    }

    // Function used by the HazardTrigger to check if the spikes are currently a threat
    public bool IsUp()
    {
        return spikesAreUp;
    }

    /*
    //This script goes on the SpikeTrap prefab;

    public Animator spikeTrapAnim; //Animator for the SpikeTrap;

    // Use this for initialization
    void Awake()
    {
        //get the Animator component from the trap;
        spikeTrapAnim = GetComponent<Animator>();
        //start opening and closing the trap for demo purposes;
        StartCoroutine(OpenCloseTrap());
    }


    IEnumerator OpenCloseTrap()
    {
        //play open animation;
        spikeTrapAnim.SetTrigger("open");
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //play close animation;
        spikeTrapAnim.SetTrigger("close");
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //Do it again;
        StartCoroutine(OpenCloseTrap());

    }
    */
}