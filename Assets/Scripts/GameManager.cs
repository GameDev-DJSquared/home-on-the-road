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
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject endScreen;

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
        if(InputManager.instance.GetRestartPressed())
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
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
