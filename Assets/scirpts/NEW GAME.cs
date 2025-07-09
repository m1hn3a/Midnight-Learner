using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class NewGameButton : MonoBehaviour
{
    public Button newGameButton;    // Assign the button in Inspector
    public string introSceneName;   // Assign the intro/trailer scene in Inspector

    void Start()
    {
        if (newGameButton != null)
            newGameButton.onClick.AddListener(StartNewGame);
    }

    void StartNewGame()
    {
        // 1. Clear PlayerPrefs
        PlayerPrefs.DeleteAll();

        // 2. Reset AutoSaveInput save file
        string inputSavePath = Application.persistentDataPath + "/savedInput.txt";
        if (File.Exists(inputSavePath))
        {
            File.Delete(inputSavePath);
            Debug.Log("[NewGameButton] Deleted AutoSaveInput save file.");
        }

        // 3. Clear SaveSystem runtime memory
        if (typeof(SaveSystem).GetField("runtimeMemory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.GetValue(null) is Dictionary<string, object> memory)
        {
            memory.Clear();
            Debug.Log("[NewGameButton] Cleared SaveSystem runtime memory.");
        }

        // Optionally reinitialize default values
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.Save();

        // 4. Load the intro scene
        SceneManager.LoadScene(introSceneName);
    }
}
