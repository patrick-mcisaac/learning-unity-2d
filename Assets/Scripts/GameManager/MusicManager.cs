using System;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private int volume = 5;
    private int maxVolume = 10;

    public MusicManager Instance;
    [SerializeField] private Slider musicVolumeSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            SetVolume(musicVolumeSlider.value);
        });
    }

    public int GetVolume()
    {
        return volume;
    }

    private void SetVolume(float value)
    {
        volume = Mathf.FloorToInt(value * maxVolume);
    }

    private float GetVolumeNormalized()
    {
        return (float)volume / maxVolume;
    }
}
