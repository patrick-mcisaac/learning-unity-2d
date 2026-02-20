using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;

    public GameState state;

    public enum GameState
    {
        WaitingToStart,
        Normal,
        Paused,
        GameOver
    }

    private void Update()
    {
        if (state == GameState.WaitingToStart)
        {
            Time.timeScale = 0f;
        }
        if (state == GameState.Normal)
        {
            Time.timeScale = 1f;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;

        DontDestroyOnLoad(this);
        state = GameState.WaitingToStart;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int points)
    {
        score += points;
    }
}