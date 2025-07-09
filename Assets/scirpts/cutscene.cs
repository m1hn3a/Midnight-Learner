using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneToLoad = "MainGame"; // Set your gameplay scene name

    private bool hasSkipped = false;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        if (!hasSkipped && Input.GetKeyDown(KeyCode.Space))
        {
            hasSkipped = true;
            SkipVideo();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (!hasSkipped)
        {
            hasSkipped = true;
            SkipVideo();
        }
    }

    void SkipVideo()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
