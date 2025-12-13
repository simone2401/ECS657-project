using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerPlayground : MonoBehaviour
{
    public static GameManagerPlayground Instance { get; private set; }
    public bool GameStarted { get; private set; } = false;

    private void Awake()
    {
        // Enforce Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Public function called by the Start Button
    public void StartGame()
    {
        if (GameStarted) return; // Prevent multiple calls

        GameStarted = true;
        Debug.Log("Game has started! Levers and Carts are now active.");

        // *** NEW CODE TO START ALL CARTS ***

        // Use FindObjectsByType for better performance and to avoid the deprecated warning
        CartController[] carts = Object.FindObjectsByType<CartController>(FindObjectsSortMode.None);

        foreach (CartController cart in carts)
        {
            // Call the StartMoving function you just added to CartController
            cart.StartMoving();
        }
    }
}
