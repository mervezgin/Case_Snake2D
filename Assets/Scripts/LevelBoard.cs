using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelBoard : MonoBehaviour
{
    public static LevelBoard instance;
    private const string LEVEL_TEXT = "LevelText";
    private const string HIGHLEVEL_TEXT = "HighLevelText";
    private Text levelText;
    private void Awake()
    {
        instance = this;
        levelText = transform.Find(LEVEL_TEXT).GetComponent<Text>();
    }
    private void Start()
    {
        Score.OnHighLevelChanged += Score_OnHighLevelChanged;
        UpdateHighLevel();
    }
    private void Score_OnHighLevelChanged(object sender, EventArgs e)
    {
        UpdateHighLevel();
    }
    private void Update()
    {
        levelText.text = "Level : " + Score.GetLevel().ToString();
    }
    private void UpdateHighLevel()
    {
        int highLevel = Score.GetHighLevel();
        transform.Find(HIGHLEVEL_TEXT).GetComponent<Text>().text = "HIGH LEVEL\n" + highLevel.ToString();
    }
    public static void HideStatic()
    {
        instance.gameObject.SetActive(false);
    }
}
