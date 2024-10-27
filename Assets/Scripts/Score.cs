using System;
using Unity.VisualScripting;
using UnityEngine;

public static class Score
{
    private const string HIGHSCORE = "highScore";
    public static event EventHandler OnHighScoreChanged;
    private static int score;
    private static int level;
    public static void InitialStatic()
    {
        OnHighScoreChanged = null;
        score = 0;
        level = 0;
    }
    public static int GetScore()
    {
        return score;
    }
    public static void AddScore()
    {
        score += 10;
    }
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGHSCORE, 0);
    }
    public static bool TrySetNewHighScore()
    {
        return TrySetNewHighScore(score);
    }
    public static bool TrySetNewHighScore(int score)
    {
        int highScore = GetHighScore();
        if (score > highScore)
        {
            PlayerPrefs.SetInt(HIGHSCORE, score);
            PlayerPrefs.Save();
            if (OnHighScoreChanged != null)
            {
                OnHighScoreChanged(null, EventArgs.Empty);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

}
