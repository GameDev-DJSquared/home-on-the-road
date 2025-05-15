using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    [SerializeField] GameObject youDiedText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI advanceText;
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject endScreen;


    static int startingQuota = 70;
    static float quotaMultiplier = 1.5f;

    public static int night = 1;
    public static int quota = 70;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Debug.LogWarning("Two GameManager Instances");

        }
        instance = this;

        //Debug.Log("current value gotten: " + DropOffZone.GetTotalValue());
        //Debug.Log("total possible value: " + FoodManager.instance.GetTotalPossibleValue());
    }

    // Update is called once per frame
    void Update()
    {
        if(endScreen.activeInHierarchy && InputManager.instance.GetInteractPressed())
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        }

        if(InputManager.instance.GetRestartPressed())
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
            quota = startingQuota;
            night = 1;
        }

        if(InputManager.instance.GetPausePressed())
        {
            if(Time.timeScale == 1)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }


    public void Pause()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        paused = true;
    }

    public void UnPause()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void FinishGame(bool died)
    {
        Time.timeScale = 0f;
        paused = true;
        mainUI.SetActive(false);
        endScreen.SetActive(true);
        youDiedText.SetActive(died);
        Debug.Log("finished Game");
        if(!died && DropOffZone.GetTotalValue() >= quota)
        {
            if(!advanceText.gameObject.activeInHierarchy)
            {
                advanceText.gameObject.SetActive(true);

            }
            night++;
            quota += DropOffZone.GetTotalValue() - quota;

            quota = (int)(quota * quotaMultiplier);
        } else
        {
            advanceText.gameObject.SetActive(false);

            night = 1;
            quota = startingQuota;
        }
        scoreText.text = "Final Score: " + GetScore();
    }


    public int GetScore()
    {
        float value = (DropOffZone.GetTotalValue());
        value *= 100;
        value /= TimeManager.instance.GetTimeElapsed() * 0.005f;
        return (int)value;
    }

    
}
