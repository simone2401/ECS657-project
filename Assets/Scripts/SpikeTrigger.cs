using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // player fails if  they get hit by spikes
        if (other.CompareTag("Character"))
        {
            // stop the character moving
            CharacterPos cart = other.GetComponent<CharacterPos>();
            if (cart != null){
                cart.StopMovement();
                Debug.Log("Character got hit by a spike!");
            }
            LevelEvents.Fail();
        }
    }
}
