using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Image FuelUI;

    [SerializeField] private Image arrowUp;
    [SerializeField] private Image arrowDown;
    [SerializeField] private Image arrowLeft;
    [SerializeField] private Image arrowRight;

    private void Update()
    {

        arrowUp.gameObject.SetActive(Lander.Instance.GetLinearVelocityY() > 0);
        arrowDown.gameObject.SetActive(Lander.Instance.GetLinearVelocityY() < 0);
        arrowRight.gameObject.SetActive(Lander.Instance.GetLinearVelocityX() > 0);
        arrowLeft.gameObject.SetActive(Lander.Instance.GetLinearVelocityX() < 0);

        pointsText.text =
       GameManager.Instance.GetScore() + "\n" +
       Mathf.Abs(Mathf.Round(Lander.Instance.GetLinearVelocityY() * 10f)) + "\n" +
       Mathf.Abs(Mathf.Round(Lander.Instance.GetLinearVelocityX() * 10f)) + "\n"
       ;

        FuelUI.fillAmount = Lander.Instance.GetFuelAmountNormalized();
    }
}
