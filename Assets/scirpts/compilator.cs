using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ColiruCompiler : MonoBehaviour
{
    public TMP_InputField codeInputField;     // C++ Code Input Field
    public TMP_InputField compileCommandField; // Compile Command Input Field (Optional)
    public TMP_InputField outputField;        // Output Display Field
    public TMP_InputField inputField;         // Player Input Field
    public Button compileButton;              // Compile Button
    public Button sendInputButton;            // Send Input Button

    private const string coliruUrl = "http://coliru.stacked-crooked.com/compile";
    private bool waitingForInput = false;     // Flag to track input state
    private string userInput = "";            // Stores user input

    void Start()
    {
        compileButton.onClick.AddListener(OnCompileButtonClicked);
        sendInputButton.onClick.AddListener(OnUserInputSubmitted);

        sendInputButton.gameObject.SetActive(true);
        sendInputButton.interactable = true;

        Debug.Log("Game Started - sendInputButton is enabled!");
    }

    public void OnCompileButtonClicked()
    {
        if (waitingForInput)
        {
            Debug.LogWarning("Already waiting for input. Please enter input first.");
            return;
        }

        waitingForInput = true;
        userInput = ""; // Reset input buffer

        outputField.text = "Waiting for input...\n";

        // Optionally disable compile button until input received
        compileButton.interactable = false;
    }

    public void OnUserInputSubmitted()
    {
        if (!waitingForInput)
        {
            Debug.LogWarning("Not waiting for input currently.");
            return;
        }

        userInput = inputField.text.Trim();
        if (string.IsNullOrEmpty(userInput))
        {
            Debug.LogWarning("No input received, please enter input before submitting.");
            return;
        }

        waitingForInput = false;
        compileButton.interactable = true;  // Re-enable compile button

        string code = codeInputField.text;
        string command;

        if (compileCommandField != null && !string.IsNullOrEmpty(compileCommandField.text))
        {
            command = compileCommandField.text;
        }
        else
        {
            // Escape special characters in input properly
            string escapedInput = userInput.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
            command = $"g++ -std=c++17 -o main main.cpp 2>&1 && echo \"{escapedInput}\" | ./main 2>&1";
        }

        StartCoroutine(CompileCode(code, command));
    }

    private IEnumerator CompileCode(string code, string command)
    {
        outputField.text = "Compiling...\n";

        CodePayload payload = new CodePayload { src = code, cmd = command };
        string json = JsonUtility.ToJson(payload);

        using (UnityWebRequest request = new UnityWebRequest(coliruUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                outputField.text = "Error: " + request.error;
            }
            else
            {
                string output = request.downloadHandler.text;
                outputField.text = output;

                sendInputButton.gameObject.SetActive(true);
                sendInputButton.interactable = true;

                Debug.Log("Compilation complete. Output:\n" + output);
            }
        }
    }
}

[System.Serializable]
public class CodePayload
{
    public string src;
    public string cmd;
}
