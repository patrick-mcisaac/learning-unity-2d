using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
    }

    private void Start()
    {
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        SoundManager.Instance.onSoundVolumeChanged += Instance_onSoundVolumeChanged;

        thrusterAudioSource.Pause();
    }

    private void Instance_onSoundVolumeChanged(object sender, System.EventArgs e)
    {
        thrusterAudioSource.volume = SoundManager.Instance.getSOundVolumeNormalized();
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        if(!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }

    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        thrusterAudioSource.Pause();
    }
}
