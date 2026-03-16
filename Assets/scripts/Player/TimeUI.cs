using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Settings")]
    [SerializeField] private string dayPrefix = "Day: ";
    [SerializeField] private string timePrefix = " | ";
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private float warningThreshold = 60f; // seconds remaining before color changes

    void Awake()
    {
        if (timerText == null)
            timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (gameManager.Instance == null)
        {
            return;
        }

        timerText.text = dayPrefix + gameManager.Instance.DayCount + timePrefix + gameManager.Instance.FormattedTime;

        // Warn player when time is running low with red timer color
        if (gameManager.Instance.SecondsRemaining + (gameManager.Instance.MinutesRemaining * 60f) <= warningThreshold)
        {
            timerText.color = warningColor;
        }
        else
        {
            timerText.color = normalColor;
        }
    }
}