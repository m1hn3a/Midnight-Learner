using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    [Header("Parent System")]
    public ParentCheckSystem parentCheckSystem;

    [Header("Scene to Load")]
    public string sceneToLoad = "MainMenu";

    [Header("Player Notes (also used for saving code)")]
    public TMP_InputField noteInputField;

    [Header("Result Text Reference")]
    public TextMeshProUGUI resultText;

    [Header("Skip Problem State")]
    public bool skipUsed = false; // ✅ New field added

    public int currentProblemIndex;

    public List<string> playerUpgrades = new List<string>();

    private static Dictionary<string, object> runtimeMemory = new Dictionary<string, object>();
    private string lastResult = "";

    private void Start()
    {
        Debug.Log("[SaveSystem] Started");
        StartCoroutine(DelayedLoad());
        StartCoroutine(WatchForCorrectResult());
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(1f);
        LoadGame();
    }

    private IEnumerator WatchForCorrectResult()
    {
        while (true)
        {
            if (resultText != null && resultText.text != lastResult && resultText.text.Contains("Correct"))
            {
                Debug.Log("[SaveSystem] Correct result detected, saving game.");
                SaveGame();
                lastResult = resultText.text;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public static void SaveBeforeSceneChange()
    {
        if (FindFirstObjectByType<SaveSystem>() is SaveSystem saveSystem) // ✅ Use modern API
        {
            saveSystem.SaveGame();
            Debug.Log("[SaveSystem] Saved before scene change.");
        }
        else
        {
            Debug.LogWarning("[SaveSystem] No SaveSystem found to save before scene change.");
        }
    }

    public void SaveGame()
    {
        if (!Application.isPlaying) return;

        string scene = SceneManager.GetActiveScene().name;

        runtimeMemory["CurrentScene"] = scene;
        runtimeMemory["PlayerCode_" + scene] = noteInputField != null ? noteInputField.text : "";
        runtimeMemory["ParentCheckChance"] = parentCheckSystem != null ? parentCheckSystem.parentCheckChance : 0;

        runtimeMemory["CurrentProblemIndex"] = currentProblemIndex;
        runtimeMemory["PlayerUpgrades"] = playerUpgrades != null ? new List<string>(playerUpgrades) : new List<string>();

        runtimeMemory["SkipUsed"] = skipUsed; // ✅ Save skipUsed value

        Debug.Log($"[SaveSystem] Game saved to memory. ProblemIndex={currentProblemIndex}, UpgradesCount={playerUpgrades.Count}, SkipUsed={skipUsed}");
    }

    public void LoadGame()
    {
        if (!Application.isPlaying) return;

        string scene = SceneManager.GetActiveScene().name;

        string savedCode = runtimeMemory.ContainsKey("PlayerCode_" + scene) ? (string)runtimeMemory["PlayerCode_" + scene] : "";
        int savedChance = runtimeMemory.ContainsKey("ParentCheckChance") ? (int)runtimeMemory["ParentCheckChance"] : 0;

        if (noteInputField != null)
        {
            noteInputField.text = savedCode;
            Debug.Log($"[SaveSystem] Loaded code into noteInputField: {savedCode}");
        }

        if (parentCheckSystem != null)
            parentCheckSystem.parentCheckChance = savedChance;

        currentProblemIndex = runtimeMemory.ContainsKey("CurrentProblemIndex") ? (int)runtimeMemory["CurrentProblemIndex"] : 0;

        if (runtimeMemory.ContainsKey("PlayerUpgrades"))
        {
            playerUpgrades = (List<string>)runtimeMemory["PlayerUpgrades"];
            Debug.Log($"[SaveSystem] Loaded player upgrades. Count={playerUpgrades.Count}");
        }
        else
        {
            playerUpgrades = new List<string>();
            Debug.Log("[SaveSystem] No saved upgrades found.");
        }

        skipUsed = runtimeMemory.ContainsKey("SkipUsed") ? (bool)runtimeMemory["SkipUsed"] : false; // ✅ Load skipUsed value
        Debug.Log($"[SaveSystem] Game loaded from memory. ProblemIndex={currentProblemIndex}, SkipUsed={skipUsed}");
    }

#if UNITY_EDITOR
    [ContextMenu("Reset Runtime Save (Play Mode Only)")]
    private void ResetRuntimeSave()
    {
        runtimeMemory.Clear();
        PlayerPrefs.DeleteAll();
        Debug.Log("[SaveSystem] All saved data cleared.");
    }
#endif
}
