
using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private int score = 0;
    private float time;

    public static GameManager Instance;

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
        DontDestroyOnLoad(this.gameObject);

    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    private void LanderOnCoinPickup(object sender, EventArgs e)
    {
        AddScore(100);
    }

    private void LanderOnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
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


}
