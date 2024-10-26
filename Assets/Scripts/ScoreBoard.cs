using UnityEngine;
using UnityEngine.UI;


public class ScoreBoard : MonoBehaviour
{
    private const string SCORE_TEXT = "ScoreText";
    private Text scoreText;
    private void Awake()
    {
        scoreText = transform.Find(SCORE_TEXT).GetComponent<Text>();
    }
    private void Update()
    {
        scoreText.text = "Score : " + GameManager.GetScore().ToString();
        Debug.Log("UPDATE SSCORE TEXT");
    }
}
