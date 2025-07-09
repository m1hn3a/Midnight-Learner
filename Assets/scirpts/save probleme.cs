using UnityEngine;

public class SimpleProblemSaver : MonoBehaviour
{
    private const string Key = "CurrentProblemIndex";

    public int currentProblemIndex = 0;

    void Start()
    {
        // Load saved value on start
        currentProblemIndex = PlayerPrefs.GetInt(Key, 0);
        Debug.Log("[SimpleProblemSaver] Loaded problem index: " + currentProblemIndex);
    }

    void Update()
    {
        // For testing, increase problem index with space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentProblemIndex++;
            Debug.Log("[SimpleProblemSaver] Increased problem index to: " + currentProblemIndex);
            PlayerPrefs.SetInt(Key, currentProblemIndex);
            PlayerPrefs.Save();
            Debug.Log("[SimpleProblemSaver] Saved problem index: " + currentProblemIndex);
        }
    }
}
