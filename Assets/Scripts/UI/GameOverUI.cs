using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI statsText;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ResetTotalScore();
            SceneLoader.LoadScene(SceneLoader.Scenes.MainMenu);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        mainMenuButton.Select();
        statsText.text = "Final Score: " + GameManager.Instance.GetTotalScore();
    }
}
