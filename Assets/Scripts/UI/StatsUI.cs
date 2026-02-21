using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Image FuelUI;

    private void Update()
    {
        pointsText.text =
       GameManager.Instance.GetScore() + "\n" +
       0 + "\n" +
       0 + "\n"
       ;

        FuelUI.fillAmount = Lander.Instance.GetFuelAmountNormalized();
    }
}
