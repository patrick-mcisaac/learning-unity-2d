using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private int volume = 5;
    private int maxVolume = 10;

    public MusicManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
