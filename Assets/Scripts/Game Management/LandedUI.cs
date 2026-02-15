using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI restartButtonText;

    private Action nextButtonClickAction;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            nextButtonClickAction();
        });
    }

    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;

        Hide();
    }



    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.LandingType == Lander.LandingType.Success)
        {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
            nextButtonClickAction = GameManager.Instance.LoadNextLevel;
            restartButtonText.text = "CONTINUE";

        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>CRASH!</color>";
            nextButtonClickAction = GameManager.Instance.Retry;
            restartButtonText.text = "RESTART";
        }

        statsTextMesh.text =
            Mathf.Round(e.landingSpeed * 2f) + "\n" +
            MathF.Round(e.dotVector * 100f) + "\n" +
            "x" + e.ScoreMultiplier + "\n" +
            e.score;

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
