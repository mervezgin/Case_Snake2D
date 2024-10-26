using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string COUNTDOWN_TEXT = "CountdownText";
    private Text countdownText;
    private void Awake()
    {
        countdownText = transform.Find(COUNTDOWN_TEXT).GetComponent<Text>();
    }
    private void Start()
    {
        GameManager.instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }
    private void Update()
    {
        countdownText.text = Mathf.Ceil(GameManager.instance.GetCountdownToStartTimer()).ToString();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
