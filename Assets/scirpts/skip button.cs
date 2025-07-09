using UnityEngine;
using UnityEngine.UI;

public class SkipProblemButton : MonoBehaviour
{
    private Button button;

    // Key for saving whether skip was used this night (unique per night/scene)
    private string skipUsedKey;

    void Awake()
    {
        button = GetComponent<Button>();
        skipUsedKey = "SkipUsed_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        button.onClick.AddListener(OnSkipClicked);
        UpdateButtonInteractable();
    }

    void UpdateButtonInteractable()
    {
        // Check if the player has the Skip upgrade AND hasn't used it yet
        bool hasSkipUpgrade = UpgradeManager.SelectedUpgrade == UpgradeType.SkipProblem;
        bool skipUsed = PlayerPrefs.GetInt(skipUsedKey, 0) == 1;

        button.interactable = hasSkipUpgrade && !skipUsed;
    }

    public void OnSkipClicked()
    {
        if (UpgradeManager.SelectedUpgrade != UpgradeType.SkipProblem)
        {
            Debug.LogWarning("Skip Problem upgrade not selected!");
            return;
        }

        // Check if skip was already used (safety)
        if (PlayerPrefs.GetInt(skipUsedKey, 0) == 1)
        {
            Debug.LogWarning("Skip Problem already used this night.");
            button.interactable = false;
            return;
        }

        // Mark skip as used for this night
        PlayerPrefs.SetInt(skipUsedKey, 1);
        PlayerPrefs.Save();

        // Disable button to prevent multiple skips
        button.interactable = false;

        // Call the SkipProblem method on your Noaptea3 script
        var noaptea3 = FindObjectOfType<Noaptea3>();
        if (noaptea3 != null)
        {
            noaptea3.SkipProblem();
            Debug.Log("Skip problem used.");
        }
        else
        {
            Debug.LogError("Noaptea3 script not found!");
        }
    }
}
