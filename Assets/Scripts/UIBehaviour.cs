using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    public Button startButton;
    public Button restartButton;
    public TextMeshProUGUI timerText;

    private float elapsedTime;
    private bool gameActive = false; // true when the game is running
    private bool gameComplete = false; // true when the player finishes the puzzle

    // Start is called before the first frame update
    void Start()
    {
        restartButton.gameObject.SetActive(false); // want the restart button hidden at the beginning
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
    }

    // Update is called once per frame
    // Increases the timer while the game is active
    void Update()
    {
        if (gameActive && !gameComplete)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay(elapsedTime);
        }
    }
    // Starts counting + hides the start button
    void StartGame()
    {
        elapsedTime = 0f;
        gameActive = true;
        startButton.gameObject.SetActive(false); // button is now hidden - we only want to start the lvl once
        restartButton.gameObject.SetActive(true);

        //TODO: trigger the character's movement
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
        Debug.Log("Puzzle completed in" + timerText.text); // also display in the game later
    }

    // Resets the lvl by reloading the scene
    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

