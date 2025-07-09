using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ParentCheckSystem : MonoBehaviour
{
    [Header("Dependencies")]
    public SceneLightSwitch lightSwitch;
    public GameObject parentWarningSprite;
    public GameObject parentCheckingVisual; // Visual object to show during parent check
    public GameObject sleepingSpriteUI;
    public Collider2D bedArea;
    public Rigidbody2D playerRb;

    [Header("Player Settings")]
    public GameObject playerVisuals;

    [Header("Parent Check Settings")]
    public int parentCheckChance = 3;
    public int incrementAmount = 4;
    [Range(0f, 10f)] public float warningDelay = 3f;

    [Header("Parent Check Timing")]
    public float parentCheckMinTime = 5f;
    public float parentCheckMaxTime = 10f;
    public float parentStayDuration = 2f;

    [Header("Scene Transition")]
    public string transitionSceneName = "Transition3"; // Changed to Transition3 for Night 3

    [Header("Caught UI")]
    public GameObject caughtCanvas;
    public float caughtDisplayDuration = 5f;

    [Header("Audio")]
    public AudioClip bedEnterClip;   // Played when player gets in bed
    public AudioClip caughtClip;     // NEW: Played when player is caught

    private bool isSleeping = false;
    private bool parentsComing = false;
    private Transform playerTransform;

    void Start()
    {
        parentWarningSprite.SetActive(false);
        sleepingSpriteUI.SetActive(false);
        if (caughtCanvas != null)
            caughtCanvas.SetActive(false);
        if (parentCheckingVisual != null)
            parentCheckingVisual.SetActive(false); // Hide parent visual at start

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;

        // Apply upgrades
        if (UpgradeManager.SelectedUpgrade == UpgradeType.LongerWarningTime)
            warningDelay = 8f;
        else if (UpgradeManager.SelectedUpgrade == UpgradeType.SlowerParentCheckRate)
        {
            parentCheckMinTime = 10f;
            parentCheckMaxTime = 15f;
        }

        StartCoroutine(ParentCheckRoutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerTransform != null)
        {
            Vector2 playerPos = playerTransform.position;

            if (bedArea != null && bedArea.OverlapPoint(playerPos))
            {
                if (!isSleeping)
                    GoToSleep();
                else
                    WakeUp();
            }
        }
    }

    private void GoToSleep()
    {
        isSleeping = true;
        sleepingSpriteUI.SetActive(true);

        if (playerVisuals != null)
            playerVisuals.SetActive(false);

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (bedEnterClip != null)
        {
            AudioSource.PlayClipAtPoint(bedEnterClip, transform.position);
        }
    }

    private void WakeUp()
    {
        isSleeping = false;
        sleepingSpriteUI.SetActive(false);

        if (playerVisuals != null)
            playerVisuals.SetActive(true);

        if (playerRb != null)
        {
            playerRb.constraints = RigidbodyConstraints2D.None;
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private IEnumerator ParentCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(parentCheckMinTime, parentCheckMaxTime));

            if (Random.Range(0, 100) < parentCheckChance && !parentsComing)
            {
                StartCoroutine(HandleParentEntry());
            }
        }
    }

    private IEnumerator HandleParentEntry()
    {
        parentsComing = true;

        if (parentWarningSprite != null)
            parentWarningSprite.SetActive(true);

        yield return new WaitForSeconds(warningDelay);

        if (parentWarningSprite != null)
            parentWarningSprite.SetActive(false);

        if (parentCheckingVisual != null)
            parentCheckingVisual.SetActive(true); // Show parent check visual

        yield return new WaitForSeconds(parentStayDuration);

        if (!lightSwitch.IsLightOn() && isSleeping)
        {
            ParentsLeave();
        }
        else
        {
            StartCoroutine(HandleCaught());
        }

        parentsComing = false;
    }

    private IEnumerator HandleCaught()
    {
        if (caughtCanvas != null)
            caughtCanvas.SetActive(true);

        if (caughtClip != null)
        {
            AudioSource.PlayClipAtPoint(caughtClip, transform.position); // NEW: play caught sound
        }

        // Reset upgrade when caught
        UpgradeManager.SelectedUpgrade = UpgradeType.None;

        yield return new WaitForSeconds(caughtDisplayDuration);

        SceneManager.LoadScene(transitionSceneName);
    }

    private void ParentsLeave()
    {
        if (parentCheckingVisual != null)
            parentCheckingVisual.SetActive(false); // Hide visual again

        WakeUp();
    }

    public void IncreaseParentCheckChance()
    {
        parentCheckChance = Mathf.Clamp(parentCheckChance + incrementAmount, 0, 100);
    }
}
