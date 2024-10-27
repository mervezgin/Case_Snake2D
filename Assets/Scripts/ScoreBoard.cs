using UnityEngine;
using UnityEngine.UI;


public class ScoreBoard : MonoBehaviour
{
    private const string SCORE_TEXT = "ScoreText";
    private const string HIGHSCORE_TEXT = "HighScoreText";
    private Text scoreText;
    private void Awake()
    {
        scoreText = transform.Find(SCORE_TEXT).GetComponent<Text>();
        int highScore = Score.GetHighScore();
        transform.Find(HIGHSCORE_TEXT).GetComponent<Text>().text = "HIGH SCORE\n" + highScore.ToString();
    }
    private void Update()
    {
        scoreText.text = "Score : " + GameManager.GetScore().ToString();
    }
}
