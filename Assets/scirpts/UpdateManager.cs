using UnityEngine;
public enum UpgradeType
{
    None,
    SkipProblem,
    SlowerParentCheckRate,
    UsePCWithoutLight,   // 👈 required for Night3UpgradeSelector
    LongerWarningTime    // 👈 required for upgradeSelector.cs and parinti1.cs
}


public class UpgradeManager : MonoBehaviour
{
    public static UpgradeType SelectedUpgrade = UpgradeType.None;

    private void Awake()
    {
        // Make sure there's only one UpgradeManager in the game
        if (FindObjectsOfType<UpgradeManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Persist between scenes (only in builds, as you wanted)
#if !UNITY_EDITOR
        DontDestroyOnLoad(gameObject);
#endif
    }
}
