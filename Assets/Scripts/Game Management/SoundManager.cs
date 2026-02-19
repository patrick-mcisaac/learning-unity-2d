using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioClip fuelPickupAudioClip;
    [SerializeField] AudioClip coinPickupAudioClip;
    [SerializeField] AudioClip crashAudioClip;
    [SerializeField] AudioClip landingSuccessAudioCLip;

    public static SoundManager Instance { get; private set; }

    private const int SOUND_VOLUME_MAX = 10;
    private static int soundVolume = 6;

    public event EventHandler onSoundVolumeChanged;

    private void Awake()
    {
        Instance = this;  
    }
    private void Start()
    {
        Lander.Instance.OnCoinPickup += Instance_OnCoinPickup;
        Lander.Instance.OnFuelPickup += Instance_OnFuelPickup;
        Lander.Instance.OnLanded += Instance_OnLanded;
    }

    private void Instance_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.LandingType == Lander.LandingType.Success)
        {
            AudioSource.PlayClipAtPoint(landingSuccessAudioCLip, Camera.main.transform.position, getSOundVolumeNormalized());
        }
        else
        {
            AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position, getSOundVolumeNormalized());
        }
    }

    private void Instance_OnFuelPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupAudioClip, Camera.main.transform.position, getSOundVolumeNormalized());
    }

    private void Instance_OnCoinPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupAudioClip, Camera.main.transform.position, getSOundVolumeNormalized());
    }

    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % (SOUND_VOLUME_MAX);
        onSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetSoundVolume()
    {
        return soundVolume;
    }

    public float getSOundVolumeNormalized()
    {
        return (float)soundVolume / SOUND_VOLUME_MAX;
    }
}
