using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VideoPlayer))]
public class IntroSequence : MonoBehaviour
{
    [Tooltip("VideoPlayer is used for playing animations. ")]
    public VideoPlayer videoPlayer;

    [Tooltip("The name of the scene to be loaded after the video finishes")]
    public string nextSceneName = "LevelZero";

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += OnVideoFinished;

        // play
        if (!videoPlayer.isPlaying)
            videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // remove binding
        vp.loopPointReached -= OnVideoFinished;
        Debug.Log("IntroSequence: The video has finished playing and the scene is loading. " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }

}