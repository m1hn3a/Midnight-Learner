using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Noaptea1 : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField codeInputField;
    public TMP_InputField inputField;
    public TMP_InputField outputField;
    public TextMeshProUGUI resultText;
    public Button checkButton;
    public Button nextProblemButton;
    public TextMeshProUGUI levelText;

    [Header("Scene Management")]
    public string winSceneName;

    [Header("Dependencies")]
    public ParentCheckSystem parentCheckSystem;

    private int currentProblemIndex = 0;
    private int level = 0;
    private const string coliruUrl = "http://coliru.stacked-crooked.com/compile";
    private string saveKey;

    [System.Serializable]
    public class Problem
    {
        public string title;
        public List<string> testInputs;
        public List<string> expectedOutputs;
    }

    [System.Serializable]
    public class CodePayload
    {
        public string src;
        public string cmd;
    }

    public List<Problem> problems = new List<Problem>();

    void Start()
    {
        saveKey = "LastProblem_" + SceneManager.GetActiveScene().name;

        level = 0;
        UpdateLevelUI();

        checkButton.onClick.AddListener(CheckSolution);
        nextProblemButton.onClick.AddListener(GoToNextProblem);
        nextProblemButton.interactable = false;

        LoadProblems();
        LoadPlayerProgress();
        LoadCurrentProblem();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        PlayerPrefs.SetInt(saveKey, currentProblemIndex);
    }

    void LoadProblems()
    {
        problems.Clear();

        problems.Add(new Problem
        {
            title = "Print Hello, world!",
            testInputs = new List<string> { "" },
            expectedOutputs = new List<string> { "Hello, world!" }
        });

        problems.Add(new Problem
        {
            title = "Sum of two numbers",
            testInputs = new List<string> { "5 3", "10 20", "7 8" },
            expectedOutputs = new List<string> { "8", "30", "15" }
        });

        problems.Add(new Problem
        {
            title = "Difference of two numbers",
            testInputs = new List<string> { "10 5", "20 8", "7 7" },
            expectedOutputs = new List<string> { "5", "12", "0" }
        });

        problems.Add(new Problem
        {
            title = "Multiply two numbers",
            testInputs = new List<string> { "3 4", "5 6", "2 10" },
            expectedOutputs = new List<string> { "12", "30", "20" }
        });

        problems.Add(new Problem
        {
            title = "Check even or odd",
            testInputs = new List<string> { "2", "7", "10" },
            expectedOutputs = new List<string> { "Even", "Odd", "Even" }
        });
    }

    void LoadPlayerProgress()
    {
        currentProblemIndex = PlayerPrefs.GetInt(saveKey, 0);
    }

    void LoadCurrentProblem()
    {
        resultText.text = $"Problem number {currentProblemIndex + 1}";
        outputField.text = "";
        inputField.text = "";
        nextProblemButton.interactable = false;
    }

    public void CheckSolution()
    {
        if (string.IsNullOrWhiteSpace(codeInputField.text))
        {
            resultText.text = $"Problem number {currentProblemIndex + 1}\nPlease enter code before submitting.";
            return;
        }

        resultText.text = $"Problem number {currentProblemIndex + 1}\nChecking...";
        StartCoroutine(RunAllTests());
    }

    private IEnumerator RunAllTests()
    {
        Problem problem = problems[currentProblemIndex];
        string userCode = codeInputField.text;
        bool allPassed = true;

        for (int i = 0; i < problem.testInputs.Count; i++)
        {
            string input = problem.testInputs[i];
            string expected = problem.expectedOutputs[i];
            string escapedInput = input.Replace("\"", "\\\"").Replace("\n", "\\n");
            string command = $"g++ -std=c++17 -o main main.cpp 2>&1 && echo \"{escapedInput}\" | ./main 2>&1";

            CodePayload payload = new CodePayload { src = userCode, cmd = command };
            string json = JsonUtility.ToJson(payload);

            using (UnityWebRequest request = new UnityWebRequest(coliruUrl, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    resultText.text = $"Problem number {currentProblemIndex + 1}\nError: {request.error}";
                    yield break;
                }
                else
                {
                    string actualOutput = request.downloadHandler.text.Trim();
                    if (actualOutput != expected)
                    {
                        resultText.text = $"Problem number {currentProblemIndex + 1}\nWrong";
                        allPassed = false;
                        break;
                    }
                }
            }
        }

        if (allPassed)
        {
            resultText.text = $"Problem number {currentProblemIndex + 1}\nCorrect";
            nextProblemButton.interactable = true;

            parentCheckSystem.IncreaseParentCheckChance();

            level += 3;
            UpdateLevelUI();
        }
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }
    }

    public void GoToNextProblem()
    {
        if (currentProblemIndex < problems.Count - 1)
        {
            currentProblemIndex++;
            LoadCurrentProblem();
        }
        else
        {
            if (!string.IsNullOrEmpty(winSceneName))
            {
                SceneManager.LoadScene(winSceneName);
            }
            else
            {
                Debug.LogError("Win scene name not set in Inspector!");
            }
        }
    }

    public void SkipProblem()
    {
        if (currentProblemIndex < problems.Count - 1)
        {
            parentCheckSystem.IncreaseParentCheckChance();
            currentProblemIndex++;
            LoadCurrentProblem();
        }
        else
        {
            GoToNextProblem();
        }
    }

    public void GoToLastProblem()
    {
        currentProblemIndex = problems.Count - 1;
        LoadCurrentProblem();
    }

    [ContextMenu("Skip One Problem")]
    private void ContextSkipProblem() => SkipProblem();

    [ContextMenu("Go To Last Problem")]
    private void ContextGoToLastProblem() => GoToLastProblem();

    [ContextMenu("Reset Progress")]
    private void ResetProgress()
    {
        PlayerPrefs.DeleteKey(saveKey);
        currentProblemIndex = 0;
        LoadCurrentProblem();
        Debug.Log("Progress reset for: " + saveKey);
    }
}
