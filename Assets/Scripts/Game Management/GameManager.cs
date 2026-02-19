
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private static int totalScore = 0;
    private int score = 0;
    private float time;
    private bool isTimerActive;

    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }

    public static GameManager Instance;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelList;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Lander.Instance.OnCoinPickup += LanderOnCoinPickup;
        Lander.Instance.OnLanded += LanderOnLanded;
        Lander.Instance.OnStateChange += LanderOnStateChange;

        GameInput.Instance.OnMenuButtonPressed += OnMenuButtonPressed;


        LoadCurrentLevel();

    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraTargetStartTransform();
        CinemachineCameraZoom2d.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicAmount());
    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == levelNumber)
            {
                return gameLevel;
            }
        }
        return null;
    }

    private void LanderOnCoinPickup(object sender, EventArgs e)
    {
        AddScore(100);
    }

    private void LanderOnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void LanderOnStateChange(object sender, Lander.OnStateChangeArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;

        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2d.Instance.SetNormalOrthographicSize();
        }
    }

    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;

    }

    public int GetScore()
    {
        return score;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public float GetTime()
    {
        return time;
    }

    public void LoadNextLevel()
    {
        totalScore += score;
        levelNumber++;
        if (GetGameLevel() == null)
        {
            // No more levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            // Still have more levels 
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }

    public void Retry()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
    }

    private void OnMenuButtonPressed(object sender, EventArgs e)
    {
        PauseUnPauseGame();
    }

    public void PauseUnPauseGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnPauseGame();
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
