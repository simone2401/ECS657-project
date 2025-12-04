using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{
    public Button startButton;
    public Button restartButton;
    public Button retryButton;
    public Button returnToMenuButton;
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public GameObject failPanel;
    public CharacterPos characterPos;

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
        returnToMenuButton.onClick.AddListener(ReturnToMenu);

        UpdateTimerDisplay(levelTimeLimit); // want to show the full time before the game starts
    }

    // Update is called once per frame
    // Increases the timer while the game is active
    void Update()
    {
        if (gameActive && !gameComplete)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f) { // TODO: the player also fails when a character crashes - not for this prototype
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
        // load the lvl scene
        //characterPos.StartMoving();
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
        returnToMenuButton.gameObject.SetActive(true);
    }

    // Player lost - time ran out or a character got hit by a spike
    void FailGame() {
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // sends you back to the menu
    public void ReturnToMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    private void OnEnable(){ 
        LevelEvents.OnLevelWin += PuzzleComplete; 
        LevelEvents.OnLevelFail += FailGame; 
    } 
    void OnDisable(){ 
        LevelEvents.OnLevelWin -= PuzzleComplete; 
        LevelEvents.OnLevelFail -= FailGame; 
    } 
}

