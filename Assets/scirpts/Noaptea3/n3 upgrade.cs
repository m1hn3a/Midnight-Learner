using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Night3UpgradeSelector : MonoBehaviour
{


    [Header("Assign Buttons in Inspector")]
    public Button usePCWithoutLightButton;  // Button for the light upgrade
    public Button skipProblemButton;        // Button for the skip problem upgrade

    private UpgradeType selectedUpgrade = UpgradeType.None;
    private bool confirmed = false;

    private void Start()
    {
        usePCWithoutLightButton.onClick.AddListener(() => SelectUpgrade(UpgradeType.UsePCWithoutLight));
        skipProblemButton.onClick.AddListener(() => SelectUpgrade(UpgradeType.SkipProblem));

        EventSystem.current.SetSelectedGameObject(usePCWithoutLightButton.gameObject);
    }

    private void Update()
    {
        if (confirmed) return;

        if (Input.GetKeyDown(KeyCode.Space) && selectedUpgrade != UpgradeType.None)
        {
            UpgradeManager.SelectedUpgrade = selectedUpgrade;
            confirmed = true;
            SceneManager.LoadScene("joc3");
        }
    }

    private void SelectUpgrade(UpgradeType upgrade)
    {
        selectedUpgrade = upgrade;

        // Optional: Highlight selection or give visual feedback
        Debug.Log($"[Upgrade] Selected: {upgrade}");
    }
}
