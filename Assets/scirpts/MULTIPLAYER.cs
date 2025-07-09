using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MultiplayerChecker : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField codeInputField;
    public TMP_InputField inputField;
    public TMP_InputField outputField;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI statusDisplayText;
    public Button checkButton;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI currentPlayerText;

    [Header("Canvas References")]
    public Canvas player1WinsCanvas;
    public Canvas player2WinsCanvas;

    [Header("Scene Management")]
    public string MainMenu;

    private int currentPlayer = 1;
    private float player1Time = 0f;
    private float player2Time = 0f;
    private bool isTiming = false;

    private List<Problem> shuffledProblems = new List<Problem>();

    private Problem currentProblem;

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
        LoadProblems();
        ShuffleProblems();
        checkButton.onClick.AddListener(CheckSolution);
        player1WinsCanvas.enabled = false;
        player2WinsCanvas.enabled = false;
        StartPlayerTurn(1);
    }

    void Update()
    {
        if (isTiming)
        {
            if (currentPlayer == 1)
                player1Time += Time.deltaTime;
            else
                player2Time += Time.deltaTime;

            timerText.text = $"{(currentPlayer == 1 ? player1Time : player2Time):F2}s";
        }
    }

    void LoadProblems()
    {
        problems.Clear();

        problems.Add(new Problem
        {
            title = "Sum of two numbers",
            testInputs = new List<string> { "5 3", "1 2" },
            expectedOutputs = new List<string> { "8", "3" }
        });

        problems.Add(new Problem
        {
            title = "Multiply two numbers",
            testInputs = new List<string> { "2 3", "4 5" },
            expectedOutputs = new List<string> { "6", "20" }
        });

        problems.Add(new Problem
{
    title = "Sum of Squares from 1 to N",
    testInputs = new List<string> { "5" },
    expectedOutputs = new List<string> { "55" } // 1²+2²+3²+4²+5²=55
});

problems.Add(new Problem
{
    title = "Check if Number is Even or Odd",
    testInputs = new List<string> { "8", "7" },
    expectedOutputs = new List<string> { "Even", "Odd" }
});

problems.Add(new Problem
{
    title = "Calculate Factorial of N",
    testInputs = new List<string> { "5" },
    expectedOutputs = new List<string> { "120" } // 5! = 120
});

problems.Add(new Problem
{
    title = "Determine if Year is Leap Year",
    testInputs = new List<string> { "2000", "1900" },
    expectedOutputs = new List<string> { "Leap", "Not leap" }
});

problems.Add(new Problem
{
    title = "Count the Number of Digits in an Integer",
    testInputs = new List<string> { "123456" },
    expectedOutputs = new List<string> { "6" }
});

problems.Add(new Problem
{
    title = "Check if Number is Perfect",
    testInputs = new List<string> { "28", "10" },
    expectedOutputs = new List<string> { "Yes", "No" }
});

problems.Add(new Problem
{
    title = "Sum Numbers Until Zero is Encountered",
    testInputs = new List<string> { "3 5 7 0" },
    expectedOutputs = new List<string> { "15" }
});

problems.Add(new Problem
{
    title = "Check if Number is a Power of Two",
    testInputs = new List<string> { "8", "10" },
    expectedOutputs = new List<string> { "Yes", "No" }
});

problems.Add(new Problem
{
    title = "Count Frequency of a Digit in a Number",
    testInputs = new List<string> { "1222342 2" },
    expectedOutputs = new List<string> { "4" } // '2' appears 4 times
});

problems.Add(new Problem
{
    title = "Check if Two Numbers Share at Least One Common Digit",
    testInputs = new List<string> { "123 345" },
    expectedOutputs = new List<string> { "Yes" }
});

problems.Add(new Problem
{
    title = "Sum of Even Numbers up to N",
    testInputs = new List<string> { "10" },
    expectedOutputs = new List<string> { "30" } // 2+4+6+8+10=30
});

problems.Add(new Problem
{
    title = "Find Minimum and Maximum in a List of Numbers",
    testInputs = new List<string> { "4 8 1 6 3" },
    expectedOutputs = new List<string> { "1 8" }
});

problems.Add(new Problem
{
    title = "Check if a String is a Palindrome",
    testInputs = new List<string> { "radar", "hello" },
    expectedOutputs = new List<string> { "Yes", "No" }
});

problems.Add(new Problem
{
    title = "Remove Vowels from a String",
    testInputs = new List<string> { "computer" },
    expectedOutputs = new List<string> { "cmptr" }
});

problems.Add(new Problem
{
    title = "Sort Three Numbers in Ascending Order",
    testInputs = new List<string> { "5 2 9" },
    expectedOutputs = new List<string> { "2 5 9" }
});

problems.Add(new Problem
{
    title = "Check if Two Strings are Anagrams",
    testInputs = new List<string> { "listen silent", "apple pplea" },
    expectedOutputs = new List<string> { "Yes", "Yes" }
});

problems.Add(new Problem
{
    title = "Calculate Product of Digits in a Number",
    testInputs = new List<string> { "234" },
    expectedOutputs = new List<string> { "24" } // 2*3*4=24
});

problems.Add(new Problem
{
    title = "Convert Binary Number to Decimal",
    testInputs = new List<string> { "1011" },
    expectedOutputs = new List<string> { "11" }
});

problems.Add(new Problem
{
    title = "Convert Decimal Number to Binary",
    testInputs = new List<string> { "10" },
    expectedOutputs = new List<string> { "1010" }
});

problems.Add(new Problem
{
    title = "Find the Second Largest Number in a List",
    testInputs = new List<string> { "4 6 2 9 7" },
    expectedOutputs = new List<string> { "7" }
});

problems.Add(new Problem
{
    title = "Count the Number of Words in a Sentence",
    testInputs = new List<string> { "This is a test" },
    expectedOutputs = new List<string> { "4" }
});

problems.Add(new Problem
{
    title = "Calculate Integer Square Root",
    testInputs = new List<string> { "16" },
    expectedOutputs = new List<string> { "4" }
});

problems.Add(new Problem
{
    title = "Sum of Odd Digits in a Number",
    testInputs = new List<string> { "12345" },
    expectedOutputs = new List<string> { "9" } // 1+3+5=9
});

problems.Add(new Problem
{
    title = "Multiply Two Numbers Without Using the * Operator",
    testInputs = new List<string> { "3 4" },
    expectedOutputs = new List<string> { "12" }
});

problems.Add(new Problem
{
    title = "Count the Number of Divisors of a Number",
    testInputs = new List<string> { "12" },
    expectedOutputs = new List<string> { "6" } // Divisors:1,2,3,4,6,12
});

problems.Add(new Problem
{
    title = "Sum of All Prime Numbers up to N",
    testInputs = new List<string> { "10" },
    expectedOutputs = new List<string> { "17" } // Primes:2,3,5,7 sum=17
});

problems.Add(new Problem
{
    title = "List All Divisors of a Number",
    testInputs = new List<string> { "15" },
    expectedOutputs = new List<string> { "1 3 5 15" }
});

problems.Add(new Problem
{
    title = "Capitalize the First Letter of Each Word in a Sentence",
    testInputs = new List<string> { "this is a test" },
    expectedOutputs = new List<string> { "This Is A Test" }
});

problems.Add(new Problem
{
    title = "Sum of the First N Even Numbers",
    testInputs = new List<string> { "4" },
    expectedOutputs = new List<string> { "20" } // 2+4+6+8=20
});

problems.Add(new Problem
{
    title = "Check if a Number is an Armstrong Number",
    testInputs = new List<string> { "153", "123" },
    expectedOutputs = new List<string> { "Yes", "No" }
});

problems.Add(new Problem
{
    title = "Find the Minimum Digit in a Number",
    testInputs = new List<string> { "509381" },
    expectedOutputs = new List<string> { "0" }
});

    }

    void ShuffleProblems()
    {
        shuffledProblems = new List<Problem>(problems);
        for (int i = 0; i < shuffledProblems.Count; i++)
        {
            int rand = Random.Range(i, shuffledProblems.Count);
            var temp = shuffledProblems[i];
            shuffledProblems[i] = shuffledProblems[rand];
            shuffledProblems[rand] = temp;
        }
    }

    void LoadRandomProblem()
    {
        if (shuffledProblems.Count == 0)
        {
            Debug.LogError("No problems available.");
            return;
        }

        int randomIndex = Random.Range(0, shuffledProblems.Count);
        currentProblem = shuffledProblems[randomIndex];
        shuffledProblems.RemoveAt(randomIndex);

        resultText.text = currentProblem.title;

        outputField.text = "";
        statusDisplayText.text = "";
    }

    public void CheckSolution()
    {
        if (string.IsNullOrWhiteSpace(codeInputField.text))
        {
            statusDisplayText.text = "Please enter code before submitting.";
            return;
        }

        statusDisplayText.text = "Checking...";
        StartCoroutine(RunAllTests());
    }

    private IEnumerator RunAllTests()
    {
        if (currentProblem == null)
        {
            statusDisplayText.text = "No problem loaded.";
            yield break;
        }

        string userCode = codeInputField.text;
        bool allPassed = true;

        for (int i = 0; i < currentProblem.testInputs.Count; i++)
        {
            string input = currentProblem.testInputs[i];
            string expected = currentProblem.expectedOutputs[i];
            string escapedInput = input.Replace("\"", "\\\"").Replace("\n", "\\n");

            string command = $"g++ -std=c++17 -o main main.cpp 2>&1 && echo \"{escapedInput}\" | ./main 2>&1";
            CodePayload payload = new CodePayload { src = userCode, cmd = command };
            string json = JsonUtility.ToJson(payload);

            using (UnityWebRequest request = new UnityWebRequest("http://coliru.stacked-crooked.com/compile", "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    statusDisplayText.text = "Error: " + request.error;
                    yield break;
                }

                string actualOutput = request.downloadHandler.text.Trim();
                if (actualOutput != expected)
                {
                    statusDisplayText.text = "Wrong";
                    allPassed = false;
                    break;
                }
            }
        }

        if (allPassed)
        {
            statusDisplayText.text = "Correct";
            yield return new WaitForSeconds(1f);
            NextTurnOrShowWinner();
        }
    }

    void StartPlayerTurn(int player)
    {
        currentPlayer = player;
        isTiming = true;
        currentPlayerText.text = $"{player}";
        LoadRandomProblem();

        // Set the codeInputField with the appropriate template depending on the player
        if (currentPlayer == 1)
        {
            codeInputField.text =
@"#include <iostream>
using namespace std;

int main() {
   
    return 0;
}";
        }
        else
        {
            codeInputField.text =
@"#include <iostream>
using namespace std;

int main() {
    
    return 0;
}";
        }

        outputField.text = "";
        inputField.text = ""; // clear input field
    }

    void NextTurnOrShowWinner()
    {
        isTiming = false;

        if (currentPlayer == 1)
            StartPlayerTurn(2);
        else
            ShowWinner();
    }

    void ShowWinner()
    {
        if (player1Time < player2Time)
            player1WinsCanvas.enabled = true;
        else
            player2WinsCanvas.enabled = true;

        StartCoroutine(GoToMainMenuAfterDelay());
    }

    IEnumerator GoToMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(MainMenu);
    }
}
