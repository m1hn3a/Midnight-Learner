using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneHandler : MonoBehaviour
{
    [Header("Scene Settings")]
    public string mainMenuSceneName; // Set this in the Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DeleteSave(); // Clear PlayerPrefs

            if (!string.IsNullOrEmpty(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
            else
            {
                Debug.LogError("Main menu scene name not set in the inspector!");
            }
        }
    }

    void DeleteSave()
    {
        // Remove only specific save keys (safer than DeleteAll)
        PlayerPrefs.DeleteKey("CurrentProblemIndex");
        PlayerPrefs.DeleteKey("ParentCheckChance");
        PlayerPrefs.DeleteKey("PlayerCode");

        PlayerPrefs.Save();
        Debug.Log("Save data deleted. Starting fresh next time.");
    }
}
