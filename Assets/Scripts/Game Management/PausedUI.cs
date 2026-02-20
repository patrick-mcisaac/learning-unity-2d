using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI soundButtonText;
    [SerializeField] private TextMeshProUGUI musicButtonText;


    private void Awake()
    {
        Time.timeScale = 1f;
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnPauseGame();
            Hide();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MenuScene);
        });

        soundButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSoundVolume();
            soundButtonText.text = $"Sound {SoundManager.Instance.GetSoundVolume()}";
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            musicButtonText.text = $"Music {MusicManager.Instance.GetMusicVolume()}";
        });
    }

    public void Start()
    {
        soundButtonText.text = $"Sound {SoundManager.Instance.GetSoundVolume()}";
        musicButtonText.text = $"Music {MusicManager.Instance.GetMusicVolume()}";
        GameManager.Instance.OnGamePaused += OnGamePaused;
        GameManager.Instance.OnGameUnPaused += OnGameUnPaused;
        Hide();
    }

    private void OnGamePaused(object sender, EventArgs e)
    {
        Show();

    }
    private void OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();

    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
