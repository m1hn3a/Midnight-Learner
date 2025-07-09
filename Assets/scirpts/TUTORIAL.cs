using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SlideshowController : MonoBehaviour
{
    [Header("Slides (Assign in Order)")]
    public Sprite slide1;
    public Sprite slide2;
    public Sprite slide3;
    public Sprite slide4;
    public Sprite slide5;
    public Sprite slide6;

    [Header("UI Elements")]
    public Image imageDisplay;
    public Button leftButton;
    public Button rightButton;
    public Button continueButton;
    public Button startButton;

    [Header("Scene Names")]
    public string slideshowSceneName; // Scene to load when Start is pressed
    public string nextSceneName;      // Scene to load when Continue is pressed

    private List<Sprite> slides;
    private int currentIndex = 0;

    void Start()
    {
        // Setup listeners
        if (startButton != null)
            startButton.onClick.AddListener(StartSlideshow);

        if (leftButton != null)
            leftButton.onClick.AddListener(GoLeft);

        if (rightButton != null)
            rightButton.onClick.AddListener(GoRight);

        if (continueButton != null)
            continueButton.onClick.AddListener(GoToNextScene);

        // Fill slide list
        slides = new List<Sprite> { slide1, slide2, slide3, slide4, slide5, slide6 };

        // Show first slide
        if (imageDisplay != null)
            UpdateSlide();

        // Hide continue button initially
        if (continueButton != null)
            continueButton.gameObject.SetActive(false);
    }

    void StartSlideshow()
    {
        if (!string.IsNullOrEmpty(slideshowSceneName))
            SceneManager.LoadScene(slideshowSceneName);
        else
            Debug.LogWarning("Slideshow scene name not set.");
    }

    void GoLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateSlide();
        }
    }

    void GoRight()
    {
        if (currentIndex < slides.Count - 1)
        {
            currentIndex++;
            UpdateSlide();
        }
    }

    void UpdateSlide()
    {
        if (imageDisplay != null && currentIndex >= 0 && currentIndex < slides.Count)
            imageDisplay.sprite = slides[currentIndex];

        if (continueButton != null)
            continueButton.gameObject.SetActive(currentIndex == 5); // Only show on slide 6
    }

    void GoToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("Next scene name not set.");
    }
}
