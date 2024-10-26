using UnityEngine;
using UnityEngine.UI;

public class GameLevelPlayingClockUI : MonoBehaviour
{
    private const string TIMER_IMAGE = "TimerImage";
    private Image timerImage;
    private void Awake()
    {
        timerImage = transform.Find(TIMER_IMAGE).GetComponent<Image>();
    }
    private void Update()
    {
        timerImage.fillAmount = GameManager.instance.GetLevelPlayingTimerNormalized();
    }
}
