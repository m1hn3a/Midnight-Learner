using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterN3 : MonoBehaviour
{
    [Header("PC UI Settings")]
    public GameObject pcUIPanel;
    public TextMeshProUGUI interactionPromptText;

    [Header("Book System")]
    public BookManager bookManager;

    [Header("Light Switch Dependency")]
    public SceneLightSwitch sceneLightSwitch;

    [Header("Player & Sprites")]
    public GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer freezeIndicator;
    public SpriteRenderer secondarySpriteRenderer;

    private bool playerInRange = false;
    private bool pcActive = false;

    public static event System.Action<bool> OnPCToggled;

    void Start()
    {
        if (pcUIPanel != null)
            pcUIPanel.SetActive(false);
        else
            Debug.LogWarning("PC UI Panel not assigned!");

        if (interactionPromptText != null)
            interactionPromptText.gameObject.SetActive(false);
        else
            Debug.LogWarning("Interaction Prompt Text not assigned!");

        if (sceneLightSwitch == null)
            Debug.LogWarning("SceneLightSwitch reference not assigned!");

        if (player != null)
        {
            rb = player.GetComponent<Rigidbody2D>();
            animator = player.GetComponent<Animator>();
            spriteRenderer = player.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogWarning("Player reference not assigned!");
        }

        if (freezeIndicator != null)
            freezeIndicator.enabled = false;

        Debug.Log("Current selected upgrade: " + UpgradeManager.SelectedUpgrade);
    }

    void Update()
    {
        bool altPressed = Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt);

        if (altPressed && (pcActive || (bookManager != null && bookManager.bookCanvas.activeSelf)))
        {
            CloseAll(); // Close book and PC
        }
        else if (altPressed && playerInRange && !pcActive)
        {
            bool canUsePC = (sceneLightSwitch != null && sceneLightSwitch.IsLightOn()) ||
                            UpgradeManager.SelectedUpgrade == UpgradeType.UsePCWithoutLight;

            if (canUsePC)
            {
                TogglePC();
            }
            else
            {
                Debug.Log("PC requires light to be on.");
            }
        }
    }

    void CloseAll()
    {
        if (bookManager != null && bookManager.bookCanvas.activeSelf)
        {
            bookManager.CloseBook();
        }

        if (pcActive)
        {
            TogglePC();
        }
    }

    void TogglePC()
    {
        pcActive = !pcActive;
        pcUIPanel.SetActive(pcActive);

        FreezePlayer(pcActive);

        Cursor.visible = pcActive;
        Cursor.lockState = pcActive ? CursorLockMode.None : CursorLockMode.Locked;

        if (interactionPromptText != null)
            interactionPromptText.gameObject.SetActive(!pcActive && playerInRange);

        OnPCToggled?.Invoke(pcActive);
    }

    void FreezePlayer(bool freeze)
    {
        if (rb != null)
            rb.constraints = freeze ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;

        if (animator != null)
            animator.enabled = !freeze;

        if (spriteRenderer != null)
            spriteRenderer.enabled = !freeze;

        if (secondarySpriteRenderer != null)
            secondarySpriteRenderer.enabled = !freeze;

        if (freezeIndicator != null)
            freezeIndicator.enabled = freeze;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!pcActive && interactionPromptText != null)
            {
                bool canShowPrompt = (sceneLightSwitch != null && sceneLightSwitch.IsLightOn()) ||
                                     UpgradeManager.SelectedUpgrade == UpgradeType.UsePCWithoutLight;

                if (canShowPrompt)
                {
                    interactionPromptText.text = "Press ALT to use PC";
                    interactionPromptText.gameObject.SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (pcActive)
                TogglePC();

            if (interactionPromptText != null)
                interactionPromptText.gameObject.SetActive(false);
        }
    }
}
