using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance { private set; get; }

    const int MINUTES_IN_HOUR = 60;
    const int HOURS_IN_DAY = 24;

    float hours = 17;
    float minutes = 0;
    int day = 1;

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
}
