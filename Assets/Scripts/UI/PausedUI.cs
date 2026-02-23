using System;
using UnityEngine;

public class PausedUI : MonoBehaviour
{
    public static PausedUI Instance;
    public event EventHandler<OnPauseUnPauseEvent> OnPauseUnPause;
    public class OnPauseUnPauseEvent : EventArgs
    {
        public GameManager.GameState state;
    }

    private void Awake()
    {
    }

    private void Start()
    {
        Instance = this;
        // Lander.Instance.OnStateChange += Lander_OnStateChange;
        PlayerControls.Instance.OnPausePressed += PlayerControls_OnPausePressed;
        Hide();
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
}
