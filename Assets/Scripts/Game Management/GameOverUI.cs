using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private TextMeshProUGUI FinalScoreText;

    private void Awake()
    {
        MainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MenuScene);
        });
    }

    private void Start()
    {
        FinalScoreText.text = $"Final Score: {GameManager.Instance.GetTotalScore()}";
        MainMenuButton.Select();
    }

}
