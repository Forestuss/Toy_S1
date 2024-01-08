using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private string _sceneName = "SceneLD de fou";
    public string _SceneName => this._sceneName;

    private AsyncOperation _asyncOperation;

    private IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        this._asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        this._asyncOperation.allowSceneActivation = false;

        while (!this._asyncOperation.isDone)
        {
            yield return null;
        }
    }

    private void Start()
    {
        Debug.Log("Started Scene Preloading");
        this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this._sceneName));
    }

    public void OnPlayButton()
    {
        Debug.Log("OnPlayButton : Load Scene");
        this._asyncOperation.allowSceneActivation = true;
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



