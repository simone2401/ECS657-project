using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Next : MonoBehaviour
{

    public string nextSceneName = "End";
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
