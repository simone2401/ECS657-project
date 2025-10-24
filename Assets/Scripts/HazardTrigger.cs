using UnityEngine;

public class HazardTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // player fails if  they get hit by spikes
        if (other.CompareTag("Character"))
        {
            // stop the character moving
            CartController cart = other.GetComponent<CartController>();
            if (cart != null){
                cart.StopMovement();
                Debug.Log("Character got hit by a spike!");
            }
            LevelEvents.Fail();
        }
    }
}
