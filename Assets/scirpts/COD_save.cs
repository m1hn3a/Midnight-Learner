using UnityEngine;
using TMPro;
using System.IO;

public class AutoSaveInput : MonoBehaviour
{
    public TMP_InputField inputField;
    private string saveFilePath;

    private float autosaveInterval = 1f; // seconds
    private float timer;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/savedInput.txt";
        LoadInput();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= autosaveInterval)
        {
            SaveInput();
            timer = 0f;
        }
    }

    void OnApplicationQuit()
    {
        SaveInput();
    }

    void OnDestroy()
    {
        SaveInput();
    }

    void SaveInput()
    {
        if (inputField != null)
        {
            File.WriteAllText(saveFilePath, inputField.text);
        }
    }

    void LoadInput()
    {
        if (File.Exists(saveFilePath) && inputField != null)
        {
            inputField.text = File.ReadAllText(saveFilePath);
        }
    }
}
