using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene_UI_Scripts : MonoBehaviour
{
    
   public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
        
    }
  
    public void BackToHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }

    public void GoStoreScene()
    {
        SceneManager.LoadScene("StoreScene");
    }
}
