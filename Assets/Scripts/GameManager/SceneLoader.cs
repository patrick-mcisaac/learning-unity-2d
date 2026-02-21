using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes
    {
        MainMenu,
        GameScene,
        GameOverScene
    }

    public static void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

}