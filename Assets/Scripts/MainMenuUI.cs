using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        StaticsScene,
    }
    private Scene scene;
    private const string PLAY_BUTTON = "PlayButton";
    private const string QUIT_BUTTON = "QuitButton";
    private const string STATICS_BUTTON = "StaticsButton";

    private Button playButton;
    private Button quitButton;
    private Button staticsButton;
    private void Awake()
    {
        playButton = transform.Find(PLAY_BUTTON).GetComponent<Button>();
        quitButton = transform.Find(QUIT_BUTTON).GetComponent<Button>();
        staticsButton = transform.Find(STATICS_BUTTON).GetComponent<Button>();

        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(Scene.GameScene.ToString());
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        staticsButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(Scene.StaticsScene.ToString());
        });
        Time.timeScale = 1.0f;
    }
}
