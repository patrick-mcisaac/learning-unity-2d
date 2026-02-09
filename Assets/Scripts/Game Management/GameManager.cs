
using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private int score = 0;

    private void Start()
    {
        Lander.Instance.OnCoinPickup += LanderOnCoinPickup;
        Lander.Instance.OnLanded += LanderOnLanded;
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




}
