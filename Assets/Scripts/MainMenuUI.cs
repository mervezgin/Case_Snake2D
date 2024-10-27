using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
    }
    private Scene scene;
    private const string PLAY_BUTTON = "PlayButton";
    private const string QUIT_BUTTON = "QuitButton";

    private Button playButton;
    private Button quitButton;
    private void Awake()
    {
        playButton = transform.Find(PLAY_BUTTON).GetComponent<Button>();
        quitButton = transform.Find(QUIT_BUTTON).GetComponent<Button>();

        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(Scene.GameScene.ToString());
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale = 1.0f;
    }
}
