using UnityEngine;

public class SwitchT : MonoBehaviour
{
    public Transform leftTile;   //left
    public Transform rightTile;  //right

    private Vector3 leftPos;
    private Vector3 rightPos;
    private bool isToggled = false;

    void Start()
    {
        // Record initial relative position
        leftPos = leftTile.localPosition;
        rightPos = rightTile.localPosition;
    }

    private void OnMouseDown()
    {
        // click
        if (!isToggled)
        {
            leftTile.localPosition = rightPos;
            rightTile.localPosition = leftPos;
        }
        else
        {
            leftTile.localPosition = leftPos;
            rightTile.localPosition = rightPos;
        }

        isToggled = !isToggled;
    }
}
