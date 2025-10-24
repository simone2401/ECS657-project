using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button controlsButton;
    public Button exitGameButton;
    public GameObject controlsPanel;

    // Start is called before the first frame update
    void Start()
    {
        controlsPanel.SetActive(false);
        playButton.onClick.AddListener(OnPlayClicked);
        controlsButton.onClick.AddListener(OnControlsClicked);
        exitGameButton.onClick.AddListener(OnExitGameClicked);
    }
    // Loads the lvl and hides the main menu after the Play button is clicked
    void OnPlayClicked(){
        SceneManager.LoadScene("PuzzleLevel");
        SceneManager.LoadScene("UI", LoadSceneMode.Additive); // Loading the first level with the UI on top

        gameObject.SetActive(false); // this hides the main menu
    }

    void OnControlsClicked() {
        controlsPanel.SetActive(true);
        playButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        exitGameButton.gameObject.SetActive(false);
    }

    public void CloseControlsPanel(){
        controlsPanel.SetActive(false);
        playButton.gameObject.SetActive(true);
        controlsButton.gameObject.SetActive(true);
        exitGameButton.gameObject.SetActive(true);
    }

    void OnExitGameClicked(){
        Application.Quit();
        Debug.Log("The game was exited");
    }
}
