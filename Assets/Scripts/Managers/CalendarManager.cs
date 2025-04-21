using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    public static CalendarManager instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI Day_Text;
    [SerializeField] private TextMeshProUGUI Clock_Text;

    [Header("Time")]
    public int CurrentDay = 1;
    public int CurrentHour = 6;
    public int CurrentMinute = 0;

    public float timeMultiplier = 60f; // 60 = 1 real second = 1 in-game minute
    private float timeElapsed = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime * timeMultiplier;

        while (timeElapsed >= 60f)
        {
            timeElapsed -= 60f;
            CurrentMinute++;
            if (CurrentMinute >= 60)
            {
                CurrentMinute = 0;
                CurrentHour++;

                if (CurrentHour >= 24)
                {
                    CurrentHour = 0;
                    UpdateDay();
                }
            }

            UpdateClock();
        }
    }

    private void UpdateClock()
    {
        if (Clock_Text != null)
        {
            Clock_Text.text = $"{CurrentHour:D2}:{CurrentMinute:D2}";
        }
    }

    private void UpdateDay()
    {
        CurrentDay++;
        if (Day_Text != null)
        {
            Day_Text.text = $"Day {CurrentDay}";
        }

        // Additional per-day logic can be placed here
        Debug.Log("A new day has started: Day " + CurrentDay);
    }
}
