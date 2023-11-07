using UnityEngine;
using UnityEngine.UI;

public class TimeDisplayController : MonoBehaviour
{
    public Text timerDisplay;

    private void Start()
    {
        UpdateTimerDisplay(0f);
    }

    public void UpdateTimerDisplay(float newTime)
    {
        string text = "Fire started: " + newTime.ToString("0") + " min ago";
        timerDisplay.text = text;
    }
}