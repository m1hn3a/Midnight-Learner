using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoTransitionController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName; // Set this in Inspector

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned!");
            return;
        }

        videoPlayer.isLooping = true;
        videoPlayer.Play();

        Camera.main.backgroundColor = Color.black;
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        videoPlayer.Stop(); // Optional: Stop video before transition
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }
}
