using UnityEngine;

public class LeverClick : MonoBehaviour
{
    private JunctionController junction;

    void Start()
    {
        junction = GetComponentInParent<JunctionController>();
    }

    void OnMouseDown()
    {
        junction?.ToggleJunction();
    }
}