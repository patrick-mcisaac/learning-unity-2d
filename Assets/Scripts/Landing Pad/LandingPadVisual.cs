using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultiplierTextMesh;

    private void Awake()
    {
        LandingPad landingPad = gameObject.GetComponent<LandingPad>();

        scoreMultiplierTextMesh.text = $"x{landingPad.ScoreMultiplier()}";

    }

}
