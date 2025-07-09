using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CaughtHandler : MonoBehaviour
{
    [Header("Caught Canvas")]
    public GameObject caughtCanvas; // Assign your disabled canvas in Inspector

    [Header("Transition Scene Name")]
    public string transitionSceneName; // e.g., "TransitionNight1"

    [Header("Caught Delay Settings")]
    public float displayDuration = 5f; // Time to show the canvas

    // Call this method when the player is caught
    public void PlayerCaught()
    {
        StartCoroutine(HandleCaughtSequence());
    }

    private IEnumerator HandleCaughtSequence()
    {
        caughtCanvas.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        SceneManager.LoadScene(transitionSceneName);
    }
}
