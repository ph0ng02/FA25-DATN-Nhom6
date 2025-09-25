using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;

    void Start()
    {
        float masterVolume, musicVolume;
        audioMixer.GetFloat("MasterVolume", out masterVolume);
        audioMixer.GetFloat("MusicVolume", out musicVolume);

        masterSlider.value = Mathf.Pow(10, masterVolume / 20f);
        musicSlider.value = Mathf.Pow(10, musicVolume / 20f);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}
