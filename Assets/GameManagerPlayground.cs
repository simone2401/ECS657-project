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
        GameStarted = true;
        Debug.Log("Game has started! Levers are now active.");
        // You would typically hide your start button/menu here
    }
}
