using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Next : MonoBehaviour
{

    public string nextSceneName = "LevelThree";
    public void OnNextLevelClicked()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("NextLevelButton: nextSceneName is Null");
            return;
        }
        SceneManager.LoadScene(nextSceneName);
    }

}
