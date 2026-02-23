using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bannerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    private Action nextButtonAction;
    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonAction();
        });
    }

    private void Start()
    {
        nextButton.Select();
        Lander.Instance.OnLanded += Lander_OnLanded;
        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        Show();
        if (e.landingType == Lander.LandingType.Success)
        {
            bannerText.text = "SUCCESSFUL LANDING!";
            nextButtonAction = GameManager.Instance.SpawnNextLevel;
            buttonText.text = "CONTINUE";
            scoreText.text = (e.score * e.scoreMultiplier).ToString();
        }
        else
        {
            bannerText.text = "CRASH!";
            nextButtonAction = GameManager.Instance.Retry;
            buttonText.text = "RETRY";
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}