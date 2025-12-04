using UnityEngine;
using UnityEngine.EventSystems;

public class LeverClick : MonoBehaviour
{
    private JunctionController junction;

    void Start()
    {
        junction = GetComponentInParent<JunctionController>();
    }

    void OnMouseDown()
    {
        //if (GameManagerPlayground.Instance != null && !(GameManagerPlayground.Instance.GameStarted))
        //{
            junction?.ToggleJunction();
        //}
    }
}