using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SceneLightSwitch : MonoBehaviour
{
    [Header("Light Settings")]
    public Light2D sceneLight;
    public float lightOnIntensity = 1f;
    public float lightOffIntensity = 0f;
    public float fadeSpeed = 1f;

    [Header("Light Colors")]
    public Color onColor = new Color32(255, 237, 165, 255);   // Light on
    public Color offColor = new Color32(165, 171, 255, 255);  // Light off

    [Header("Audio Settings")]
    public AudioClip lightOnSound;
    public AudioClip lightOffSound;

    private AudioSource audioSource;
    private bool isLightOn = false;
    private bool inRange = false;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            isLightOn = !isLightOn;

            // Play the correct sound
            if (audioSource != null)
            {
                AudioClip clipToPlay = isLightOn ? lightOnSound : lightOffSound;
                if (clipToPlay != null)
                    audioSource.PlayOneShot(clipToPlay);
            }

            StopAllCoroutines();
            StartCoroutine(FadeLight(
                isLightOn ? lightOnIntensity : lightOffIntensity,
                isLightOn ? onColor : offColor
            ));
        }
    }

    private IEnumerator FadeLight(float targetIntensity, Color targetColor)
    {
        while (!Mathf.Approximately(sceneLight.intensity, targetIntensity) || sceneLight.color != targetColor)
        {
            sceneLight.intensity = Mathf.MoveTowards(sceneLight.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            sceneLight.color = Color.Lerp(sceneLight.color, targetColor, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        sceneLight.intensity = targetIntensity;
        sceneLight.color = targetColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            inRange = false;
    }

    // ✅ This method MUST be here
    public bool IsLightOn()
    {
        return isLightOn;
    }
}
