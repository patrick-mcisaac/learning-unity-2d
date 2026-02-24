using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private AudioMixer mixer;

    public static float sfxVolume;
    public static float musicVolume;

    private enum Volume
    {
        MasterVolume,
        SFXVolume,
        MusicVolume
    }

    private void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            SetMusicVolume(musicVolumeSlider.value);
        });

        sfxVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            SetSFXVolume(sfxVolumeSlider.value);
        });



        LoadSavedVolume();
        if (PausedUI.Instance)
        {

            PausedUI.Instance.FillVolume(sfxVolume, musicVolume);
        }
    }



    private void SetMusicVolume(float value)
    {
        // volume = Mathf.FloorToInt(value * maxVolume);
        musicVolume = Mathf.Log10(value) * 20;
        mixer.SetFloat(Volume.MusicVolume.ToString(), musicVolume);
        PlayerPrefs.SetFloat(Volume.MusicVolume.ToString(), musicVolume);

    }

    private void SetSFXVolume(float value)
    {
        // volume = Mathf.FloorToInt(value * maxVolume);
        sfxVolume = Mathf.Log10(value) * 20;
        mixer.SetFloat(Volume.SFXVolume.ToString(), sfxVolume);
        PlayerPrefs.SetFloat(Volume.SFXVolume.ToString(), sfxVolume);
    }

    private void LoadSavedVolume()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat(Volume.MasterVolume.ToString(), 0f);
        float savedMusicVolume = PlayerPrefs.GetFloat(Volume.MusicVolume.ToString(), 0f);
        float savedSFXVolume = PlayerPrefs.GetFloat(Volume.SFXVolume.ToString(), 0f);

        mixer.SetFloat(Volume.MusicVolume.ToString(), savedMusicVolume);
        mixer.SetFloat(Volume.SFXVolume.ToString(), savedSFXVolume);
    }

}