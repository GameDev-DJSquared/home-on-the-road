using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance { private set; get; }

    [SerializeField] TextMeshProUGUI timeText;


    const int MINUTES_IN_HOUR = 60;
    const int HOURS_IN_DAY = 24;

    float hours = 22;
    float minutes = 0;
    int day = 1;

    float timePassed = 0f;

    bool stopped = false;

    [SerializeField] float timeRate = 1f;

    public event System.Action<int, int> OnTimeChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // safety in case of duplicates
        }
    }

    void Update()
    {
        if (stopped) return;


        minutes += timeRate * Time.deltaTime;
        timePassed += timeRate * Time.deltaTime;

        if (minutes >= MINUTES_IN_HOUR)
        {
            minutes -= MINUTES_IN_HOUR;
            hours++;
        }

        if (hours >= HOURS_IN_DAY)
        {
            hours -= HOURS_IN_DAY;
            day++;
        }


        int displayHour = (int)hours % 12;
        if (displayHour == 0) displayHour = 12; // 12-hour clock fix

        string amPm = hours >= 12 ? "PM" : "AM";
        string formattedTime = $"{displayHour:D2}:{(int)(Mathf.Floor(minutes / 10) * 10):D2} {amPm}";

        timeText.text = formattedTime;


        if(hours == 7 && minutes < 60)
        {
            GameManager.instance.FinishGame(false);
        }
    }

    public int GetMinutes() => (int)minutes + (int)hours * MINUTES_IN_HOUR;

    public int[] GetTime() => new int[] { (int)hours, (int)minutes };

    public int GetDay() => day;

    public void SetNewTime(int newHours, int newMinutes)
    {
        hours = Mathf.Clamp(newHours, 0, HOURS_IN_DAY - 1);
        minutes = Mathf.Clamp(newMinutes, 0, MINUTES_IN_HOUR - 1);
    }

    public void Resume() => stopped = false;

    public void Stop() => stopped = true;

    public float GetTimeElapsed()
    {
        return timePassed;
    }
}
