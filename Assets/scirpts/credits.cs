using UnityEngine;
using UnityEngine.UI;

public class SideMenuController : MonoBehaviour
{
    [Header("Main Menu UI")]
    public Image mainImage;
    public Button creditsButton;

    [Header("Credits UI")]
    public Text creditsText;
    public Button backButton;

    
    void Start()
    {
        // Show main menu elements
        mainImage.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);

        // Hide credits elements at the start
        creditsText.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

        // Set up button listeners
        creditsButton.onClick.AddListener(ShowCreditsMenu);
        backButton.onClick.AddListener(ShowMainMenu);
    }

    public void ShowCreditsMenu()
    {
        mainImage.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);

        creditsText.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void ShowMainMenu()
    {
        creditsText.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

        mainImage.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
    }
}
