using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.IO;
using System.Collections;

public class CppCompiler : MonoBehaviour
{
    // UI fields that will be linked in the Unity inspector
    public TMP_InputField outputInputField;  // Where the output will be displayed
    public TMP_InputField codeInputField;    // Where the user will type the C++ code
    public GameObject compileButton;         // Button to trigger the compile action

    // Paths to the compiler (make sure to set the path to g++/MinGW on the system)
    private string compilerPath = "g++"; // "g++" for macOS/Linux, or "mingw32-g++.exe" for Windows
    private string temporaryFilePath = "temp_code.cpp"; // Temp file to store C++ code

    // This function will be called when the user clicks "Compile"
    public void OnCompileButtonClicked()
    {
        string code = codeInputField.text;  // Get the code entered by the user

        outputInputField.text = "";  // Clear previous output

        // Start compiling the code in the background
        StartCoroutine(CompileCode(code));
    }

    // This function sends the code to a temporary file, runs the compiler, and captures the output
    private IEnumerator CompileCode(string code)
    {
        // Step 1: Write the code to a temporary file
        File.WriteAllText(temporaryFilePath, code);

        // Step 2: Start the compiler process
        string compileCommand = $"{compilerPath} {temporaryFilePath} -o temp_program";
        string executeCommand = "temp_program"; // Command to run the compiled program

        // Step 3: Run the compiler and capture output (errors or success)
        yield return new WaitForSeconds(0.5f);  // Wait a little to ensure file is written

        // Clear previous output to avoid overlapping errors or output
        outputInputField.text = "Compiling...";

        string output = ExecuteCommand(compileCommand); // Run the compiler command
        string executionOutput = ExecuteCommand(executeCommand); // Run the compiled program

        // Step 4: Process and display the output (compiler errors or program output)
        if (!string.IsNullOrEmpty(output))
        {
            outputInputField.text = output; // Display compiler errors (if any)
        }
        else
        {
            outputInputField.text = executionOutput; // Display program output (if compiled successfully)
        }

        // Clean up the generated files after execution
        File.Delete(temporaryFilePath);
        File.Delete("temp_program"); // Delete the compiled program
    }

    // Executes a command and returns the output (standard output or error)
    private string ExecuteCommand(string command)
    {
        string output = "";

        try
        {
            // Use different shell commands for macOS/Linux or Windows
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash", // For macOS/Linux
                Arguments = $"-c \"{command}\"", // Run the command
                RedirectStandardOutput = true, // Capture the output
                RedirectStandardError = true,  // Capture the error
                UseShellExecute = false, // Don't use the system shell
                CreateNoWindow = true // Don't create a window
            };

            // For Windows use the appropriate shell
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                processStartInfo.FileName = "cmd.exe"; // For Windows
                processStartInfo.Arguments = $"/C {command}"; // Run the command
            }

            Process process = Process.Start(processStartInfo);
            output = process.StandardOutput.ReadToEnd(); // Get the standard output
            string error = process.StandardError.ReadToEnd(); // Get the error output
            if (!string.IsNullOrEmpty(error)) output = error; // Show errors if any
            process.WaitForExit();
        }
        catch (System.Exception ex)
        {
            output = "Error: " + ex.Message;
        }

        return output;
    }
}
