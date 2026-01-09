using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{
    public Button startButton;
    public Button restartButton;
    public Button retryButton;
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public GameObject failPanel;

    public GameObject settingsPanel; // Drag the 'SettingsPanel' here in the Inspector

    private bool gameActive = false; // true when the game is running
    private bool gameComplete = false; // true when the player finishes the puzzle
    public float levelTimeLimit = 60f; // want a time limit -> if it gets to 0 the player loses
    private float remainingTime;

    // Start is called before the first frame update
    void Start()
    {
        restartButton.gameObject.SetActive(false); // want the restart button hidden at the beginning, as well as the win/fail panels
        winPanel.SetActive(false);
        failPanel.SetActive(false);

        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
        retryButton.onClick.AddListener(RestartGame);

        UpdateTimerDisplay(levelTimeLimit); // want to show the full time before the game starts
    }

    // Update is called once per frame
    // Increases the timer while the game is active
    void Update()
    {
        if (gameActive && !gameComplete)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            { // TODO: the player also fails when a character crashes - not for this prototype
                remainingTime = 0f;
                FailGame();
            }
            UpdateTimerDisplay(remainingTime);
        }
    }

    // Begins the gameplay
    void StartGame()
    {
        remainingTime = levelTimeLimit;
        gameActive = true;
        gameComplete = false;

        startButton.gameObject.SetActive(false); // button is now hidden - we only want to start the lvl once
        restartButton.gameObject.SetActive(true);
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        settingsPanel.SetActive(false);
        // load the lvl scene
    }

    // Converts time to the form minutes:seconds
    void UpdateTimerDisplay(float time)
    {
        int mins = Mathf.FloorToInt(time / 60);
        int secs = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }

    // Player wins - all characters (1 for now) have reached their goal
    public void PuzzleComplete()
    {
        gameComplete = true;
        gameActive = false;
        winPanel.SetActive(true);
        restartButton.gameObject.SetActive(false);
        Debug.Log("Puzzle completed in: " + timerText.text); // also display in the game later
    }

    // Player lost - time ran out or a character got hit by a spike
    void FailGame()
    {
        gameComplete = false;
        gameActive = false;
        failPanel.SetActive(true);
        restartButton.gameObject.SetActive(false); //don't want the restart button when the game's over - that's what the retry button is there for
        retryButton.gameObject.SetActive(true);
        Debug.Log("Puzzle failed");
    }
    // Resets the lvl by reloading the scene
    public void RestartGame()
    {
        // 1. Stop the game logic
        gameActive = false;
        gameComplete = false;

        // 2. Reset the timer
        remainingTime = levelTimeLimit;
        UpdateTimerDisplay(remainingTime);

        if (GameManagerPlayground.Instance != null)
        {
            GameManagerPlayground.Instance.ResetManager();
        }

        // 3. Reset all carts to their starting position
        // We find all CartControllers and tell them to go back to the start and stop moving
        CartController[] carts = Object.FindObjectsByType<CartController>(FindObjectsSortMode.None);
        foreach (CartController cart in carts)
        {
            cart.StopMovement();
            // This resets the distanceTraveled variable we added to your CartController
            cart.ResetCart();
        }

        SpikeTrapDemo[] traps = Object.FindObjectsByType<SpikeTrapDemo>(FindObjectsSortMode.None);
        foreach (SpikeTrapDemo trap in traps)
        {
            trap.ResetTrap();
        }

        // Reset all trees
        TreeObstacle[] trees = Object.FindObjectsByType<TreeObstacle>(FindObjectsSortMode.None);
        foreach (TreeObstacle tree in trees)
        {
            Debug.Log($"Resetting tree: {tree.gameObject.name}");
            tree.Reset();
        }

        // 4. Update UI visibility
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        retryButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        settingsPanel.SetActive(true);

        // Show the start button so the player can begin the new attempt
        startButton.gameObject.SetActive(true);

        Debug.Log("Level reset manually. Lever positions preserved.");
    }

    // sends you back to the menu
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void OnEnable()
    {
        LevelEvents.OnLevelWin += PuzzleComplete;
        LevelEvents.OnLevelFail += FailGame;
    }
    void OnDisable()
    {
        LevelEvents.OnLevelWin -= PuzzleComplete;
        LevelEvents.OnLevelFail -= FailGame;
    }
}