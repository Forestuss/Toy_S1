using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene("SceneLD de fou");
    }

    public void OnParametresButton()
    {
        
    }

    public void OnCreditsButton() 
    {

    }

    public void OnQuitButton()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}
