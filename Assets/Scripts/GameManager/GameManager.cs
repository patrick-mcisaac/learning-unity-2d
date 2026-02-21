using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private static int levelNumber = 1;

    private int score = 0;
    public GameState state;

    public enum GameState
    {
        WaitingToStart,
        Normal,
        Paused,
        GameOver,
        Loading
    }

    [SerializeField] private List<Level> levelList;

    private void Update()
    {
        if (state == GameState.WaitingToStart ||
            state == GameState.Paused ||
            state == GameState.Loading)
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
    }

    public void Start()
    {
        LoadCurrentLevel();
        state = GameState.WaitingToStart;

        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    public int GetScore()
    {
        return score;
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

    private void SpawnNextLevel()
    {
        levelNumber += 1;

        if (GetLevel() != null)
        {
            SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
        }
        // else
        // {
        //     SceneLoader.LoadScene(SceneLoader.Scenes.GameOverScene);
        // }

    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score * e.scoreMultiplier);
    }
}