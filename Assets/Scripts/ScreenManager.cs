using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
   public string gameScene;
   
   public string settingScene;

   public string instructionScene;

   public string startScene;
    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene);
    }

    public void LoadSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(settingScene);
    }

    public void LoadInstructions()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(instructionScene);
    }

    public void LoadStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(startScene);
    }
}
