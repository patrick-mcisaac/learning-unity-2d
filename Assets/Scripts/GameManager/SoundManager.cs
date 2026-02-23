using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupAudio;
    [SerializeField] private AudioClip fuelPickupAudio;
    [SerializeField] private AudioClip crashAudio;
    [SerializeField] private AudioClip successfulLandingAudio;

    private AudioSource audioSource;

    private int maxVolume = 10;
    private int volume = 6;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success)
        {
            audioSource.PlayOneShot(successfulLandingAudio, GetVolumeNormalized());
        }
        else
        {
            audioSource.PlayOneShot(crashAudio, GetVolumeNormalized());
        }
    }

    private void Lander_OnCoinPickup(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(coinPickupAudio, GetVolumeNormalized());
    }

    private void Lander_OnFuelPickup(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(fuelPickupAudio, GetVolumeNormalized());
    }

    public int GetVolume()
    {
        return volume;
    }

    public void SetVolume()
    {
        volume++;
    }

    private float GetVolumeNormalized()
    {
        return (float)volume / maxVolume;
    }

}