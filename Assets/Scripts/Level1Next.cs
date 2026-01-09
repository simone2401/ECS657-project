using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Next : MonoBehaviour
{
    public string nextSceneName = "LevelTwo";
    public void OnNextLevelClicked()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("NextLevelButton: nextSceneName is null");
            return;
        }
        SceneManager.LoadScene(nextSceneName);
    }
}