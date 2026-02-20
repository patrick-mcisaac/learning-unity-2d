using TMPro;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField] private TextMeshPro launchPadText;
    [SerializeField] private int launchPadBonusPoints;

    private void Awake()
    {
        launchPadText.text = $"x{launchPadBonusPoints}";
    }
}
