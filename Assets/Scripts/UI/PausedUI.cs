using System;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    public static PausedUI Instance;
    public event EventHandler<OnPauseUnPauseEvent> OnPauseUnPause;
    public class OnPauseUnPauseEvent : EventArgs
    {
        public GameManager.GameState state;
    }

    [SerializeField] public Slider sfxSlider;
    [SerializeField] public Slider musicSlider;


    private void Start()
    {
        Instance = this;
        // Lander.Instance.OnStateChange += Lander_OnStateChange;
        PlayerControls.Instance.OnPausePressed += PlayerControls_OnPausePressed;
        Hide();
        musicSlider.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        OnPauseUnPause?.Invoke(this, new OnPauseUnPauseEvent()
        {
            state = GameManager.GameState.Normal
        });
    }

    private void Show()
    {
        gameObject.SetActive(true);
        OnPauseUnPause?.Invoke(this, new OnPauseUnPauseEvent()
        {
            state = GameManager.GameState.Paused
        });
    }

    private void PlayerControls_OnPausePressed(object sender, EventArgs e)
    {
        if (GameManager.Instance.state == GameManager.GameState.Normal)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void FillVolume(float sfx, float music)
    {
        sfxSlider.value = Mathf.Pow(10f, sfx / 20);
        musicSlider.value = Mathf.Pow(10f, music / 20f);
    }
}
