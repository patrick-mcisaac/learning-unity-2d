
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    private int score = 0;
    private float time;
    private bool isTimerActive;

    public static GameManager Instance;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelList;

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
        foreach (GameLevel gameLevel in gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == levelNumber)
            {
                GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
                cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraTargetStartTransform();
                CinemachineCameraZoom2d.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicAmount());
            }
        }
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
        Debug.Log(score);
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }

    public void LoadNextLevel()
    {
        levelNumber++;
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }
}
