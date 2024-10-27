using UnityEngine;

public static class Score
{
    private const string HIGHSCORE = "highScore";
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGHSCORE, 0);
    }
    public static bool TrySetNewHighScore(int score)
    {
        int highScore = GetHighScore();
        if (score > highScore)
        {

        }
    }
}
