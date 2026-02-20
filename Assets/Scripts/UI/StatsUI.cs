using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Image FuelUI;

    private void Update()
    {
        statsText.text =
       GameManager.Instance.GetScore() + "\n" +
       0 + "\n" +
       0 + "\n"
       ;

        FuelUI.fillAmount = Lander.Instance.GetFuelAmountNormalized();
    }
}
