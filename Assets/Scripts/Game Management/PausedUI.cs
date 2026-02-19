using System;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

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
    }

    public void Start()
    {
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
