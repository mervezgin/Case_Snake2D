using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        SceneManager.LoadScene(scene.ToString());
    }
}
