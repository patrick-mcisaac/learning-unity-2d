using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private static int levelNumber = 1;

    private int score = 0;

    private static int totalScore;

    public GameState state;

    public enum GameState
    {
        WaitingToStart,
        Normal,
        Paused,
        GameOver

    }

    [SerializeField] private List<Level> levelList;

    private void Update()
    {
        if (state == GameState.WaitingToStart ||
            state == GameState.Paused)
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
        Instance = this;
    }

    public void Start()
    {
        LoadCurrentLevel();
        state = GameState.WaitingToStart;

        Lander.Instance.OnLanded += Lander_OnLanded;
        PausedUI.Instance.OnPauseUnPause += PausedUI_OnPauseUnPause;

    }

    public int GetScore()
    {
        return score;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void ResetTotalScore()
    {
        totalScore = 0;
    }

    public void AddScore(int points)
    {
        score += points;
    }

    private Level GetLevel()
    {
        foreach (Level level in levelList)
        {
            if (level.GetLevelNumber() == levelNumber)
            {
                return level;
            }
        }
        return null;
    }

    private void LoadCurrentLevel()
    {
        Level level = GetLevel();
        Level spawnedLevel = Instantiate(level, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnedLevel.GetLanderTransformStart();
        CinemachineCameraZoom2D.Instance.SetCameraPosition(spawnedLevel.GetCameraStartPosition());
        CinemachineCameraZoom2D.Instance.SetCameraOrthoAmount(spawnedLevel.GetZoomedOutOrthoAmount());
    }

    public void SpawnNextLevel()
    {
        levelNumber += 1;
        totalScore += score;

        if (GetLevel() != null)
        {
            SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
        }
        else
        {
            levelNumber = 1;
            SceneLoader.LoadScene(SceneLoader.Scenes.GameOver);
        }

    }

    public void Retry()
    {
        SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        state = GameState.GameOver;
        AddScore(e.score * e.scoreMultiplier);
    }

    private void PausedUI_OnPauseUnPause(object sender, PausedUI.OnPauseUnPauseEvent e)
    {
        state = e.state;
    }
}