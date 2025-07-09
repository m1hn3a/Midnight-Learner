using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    public Button continueButton;
    public string fallbackScene = "Night1"; // Default scene if no saved data found

    void Start()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueGame);
    }

    void ContinueGame()
    {
        // Check if we have runtime memory access
        System.Type saveSystemType = typeof(SaveSystem);
        var field = saveSystemType.GetField("runtimeMemory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var runtimeMemory = field.GetValue(null) as System.Collections.Generic.Dictionary<string, object>;

        if (runtimeMemory != null && runtimeMemory.ContainsKey("CurrentScene"))
        {
            string lastScene = (string)runtimeMemory["CurrentScene"];
            Debug.Log($"[ContinueButton] Loading saved scene: {lastScene}");
            SceneManager.LoadScene(lastScene);
        }
        else
        {
            Debug.LogWarning($"[ContinueButton] No saved scene found, loading fallback: {fallbackScene}");
            SceneManager.LoadScene(fallbackScene);
        }
    }
}
