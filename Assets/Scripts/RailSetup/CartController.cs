using UnityEngine;
using UnityEngine.Splines;

public class CartController : MonoBehaviour
{
    [SerializeField] readonly SplineContainer splineContainer;
    [SerializeField] readonly float speed = 5f;
}