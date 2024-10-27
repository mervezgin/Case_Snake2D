using System;
using Unity.VisualScripting;
using UnityEngine;

public static class Score
{
    private const string HIGHSCORE = "highScore";
    private const string HIGHLEVEL = "highLevel";
    public static event EventHandler OnHighScoreChanged;
    public static event EventHandler OnHighLevelChanged;
    private static int score;
    private static int level;
    public static void InitialStatic()
    {
        OnHighScoreChanged = null;
        OnHighLevelChanged = null;
        score = 0;
        level = 1;
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
    public static int GetLevel()
    {
        return level;
    }
    public static void AddLevel()
    {
        level += 1;
    }
    public static int GetHighLevel()
    {
        return PlayerPrefs.GetInt(HIGHSCORE, 1);
    }
    public static bool TrySetNewHighLevel()
    {
        return TrySetNewHighLevel(level);
    }
    public static bool TrySetNewHighLevel(int level)
    {
        int highLevel = GetHighLevel();
        if (level > highLevel)
        {
            PlayerPrefs.SetInt(HIGHLEVEL, level);
            PlayerPrefs.Save();
            if (OnHighLevelChanged != null)
            {
                OnHighLevelChanged(null, EventArgs.Empty);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
