using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DayState
{
    Day,
    Night
}

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

    [Header("State")]
    [SerializeField] private DayState state;

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
        // Skip to the next day when R is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            SkipToNextDay();
        }

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

                // Update Day/Night Cycle
                if (state == DayState.Night && CurrentHour >= 6) state = DayState.Day;
                else if (state == DayState.Day && CurrentHour >= 18) state = DayState.Night;
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
        TileManager.instance.UpdateSeededTiles(); // Update seeded tiles for the new day
        // Additional per-day logic can be placed here
        Debug.Log("A new day has started: Day " + CurrentDay);
    }

    private void SkipToNextDay()
    {
        CurrentHour = 0;
        CurrentMinute = 0;
        timeElapsed = 0f;
        state = DayState.Day; // Optionally reset day state
        UpdateClock();
        UpdateDay();
    }

}
