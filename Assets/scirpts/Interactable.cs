using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [Header("PC UI Settings")]
    public GameObject pcUIPanel;
    public TextMeshProUGUI interactionPromptText;

    [Header("Book System")]
    public BookManager bookManager;

    [Header("Light Switch Dependency")]
    public SceneLightSwitch sceneLightSwitch;

    [Header("Player Reference")]
    public GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Optional Freeze Indicator")]
    public SpriteRenderer freezeIndicator;

    [Header("Additional Sprite")]
    public SpriteRenderer secondarySpriteRenderer;

    [Header("Audio")]
    public AudioClip sitAtPCClip; // 🎵 NEW: sound when sitting at the PC with lights on

    private bool playerInRange = false;
    private bool pcActive = false;
    private bool hidingWithoutPC = false;

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

        if (player == null)
        {
            Debug.LogWarning("Player reference not assigned!");
        }
        else
        {
            rb = player.GetComponent<Rigidbody2D>();
            animator = player.GetComponent<Animator>();
            spriteRenderer = player.GetComponent<SpriteRenderer>();
        }

        if (freezeIndicator != null)
            freezeIndicator.enabled = false;
    }

    void Update()
    {
        bool altPressed = Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt);

        if (altPressed && (pcActive || hidingWithoutPC || (bookManager != null && bookManager.bookCanvas.activeSelf)))
        {
            CloseAll();
        }
        else if (altPressed && playerInRange && !pcActive && !hidingWithoutPC)
        {
            FreezePlayer(true);

            if (sceneLightSwitch != null && sceneLightSwitch.IsLightOn())
            {
                // 🎵 Play sound only if lights are on and sound is assigned
                if (sitAtPCClip != null)
                {
                    AudioSource.PlayClipAtPoint(sitAtPCClip, transform.position);
                }

                TogglePC(); // open PC
            }
            else
            {
                hidingWithoutPC = true;
                Debug.Log("Hiding without accessing PC (light is off)");
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

        if (hidingWithoutPC)
        {
            FreezePlayer(false);
            hidingWithoutPC = false;
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
    }
void FreezePlayer(bool freeze)
{
    if (rb != null)
    {
        if (freeze)
            rb.linearVelocity = Vector2.zero; // 🧊 Stop all movement

        rb.constraints = freeze ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;
    }

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
                if (sceneLightSwitch != null && sceneLightSwitch.IsLightOn())
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
