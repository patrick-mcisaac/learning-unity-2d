using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class ThrusterAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioMixer mixer;

    private Coroutine activeFade;
    private bool isThrusterActive = false;

    private enum Volume
    {
        ThrusterVolume
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.Pause();
        Lander.Instance.OnUpForce += Lander_OnUpForce;
        Lander.Instance.OnRightForce += Lander_OnRightForce;
        Lander.Instance.OnLeftForce += Lander_OnLeftForce;
        Lander.Instance.OnBeforeForce += Lander_OnBeforeForce;
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            isThrusterActive = true;
            ToggleThrusterAudio();
        }
    }
    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            isThrusterActive = true;
            ToggleThrusterAudio();
        }
    }
    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            isThrusterActive = true;
            ToggleThrusterAudio();
        }
    }
    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        isThrusterActive = false;
        ToggleThrusterAudio();
        audioSource.Pause();
    }

    private IEnumerator FadeMixer(float targetLinear)
    {


        float fadeDuration = 0.3f;

        mixer.GetFloat(Volume.ThrusterVolume.ToString(), out float currentDecibel);
        float startLinear = Mathf.Pow(10, currentDecibel / 20f);
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float currentLinear = Mathf.Lerp(startLinear, targetLinear, elapsed / fadeDuration);

            // Convert to decibels (using 0.0001 to avoid Log10 of zero)
            float newDecibel = Mathf.Log10(Mathf.Max(currentLinear, 0.0001f)) * 20f;
            mixer.SetFloat(Volume.ThrusterVolume.ToString(), newDecibel);
            yield return null;
        }

        mixer.SetFloat(Volume.ThrusterVolume.ToString(), Mathf.Log10(Mathf.Max(targetLinear, 0.0001f)));




        isThrusterActive = false;
        activeFade = null;

    }

    private void ToggleThrusterAudio()
    {

        float targetValue = isThrusterActive ? 1f : 0f;
        if (activeFade != null)
        {
            StopCoroutine(activeFade);
        }
        activeFade = StartCoroutine(FadeMixer(targetValue));
    }
}
