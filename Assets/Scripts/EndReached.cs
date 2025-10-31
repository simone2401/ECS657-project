using UnityEngine;

public class EndReached : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Character"))
        {
            Debug.Log("Character reached the endpoint!");
            LevelEvents.Win();
        }
    }
}
