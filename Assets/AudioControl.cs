using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeControl : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    private const string VolumePrefKey = "MasterVolume"; // key for PlayerPrefs

    void Start()
    {
        // Load saved volume or default to 1 (max volume)
        float savedVolume = PlayerPrefs.HasKey(VolumePrefKey) ? PlayerPrefs.GetFloat(VolumePrefKey) : 1f;

        slider.value = savedVolume;

        ApplyVolume(savedVolume);

        slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        ApplyVolume(volume);

        // Save to PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float volume)
    {
        if (volume <= 0.0001f)
            mixer.SetFloat("MasterVolume", -80f); // mute
        else
            mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
    }
    void Awake()
{
    DontDestroyOnLoad(gameObject);
}

}
