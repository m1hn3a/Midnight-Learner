using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UpgradeSelector : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public string sceneToLoad = "Night2"; // Adjust scene name for older upgrades

    private UpgradeType selectedUpgrade = UpgradeType.None;
    private bool confirmed = false;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(leftButton.gameObject);
    }

    void Update()
    {
        if (confirmed) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;

            if (selected == leftButton.gameObject)
                selectedUpgrade = UpgradeType.LongerWarningTime;
            else if (selected == rightButton.gameObject)
                selectedUpgrade = UpgradeType.SlowerParentCheckRate;

            if (selectedUpgrade != UpgradeType.None)
            {
                UpgradeManager.SelectedUpgrade = selectedUpgrade;
                confirmed = true;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
