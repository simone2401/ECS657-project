using UnityEngine;
using UnityEngine.SceneManagement;

public class Level0Next : MonoBehaviour
{
    public string nextSceneName = "LevelOne";
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