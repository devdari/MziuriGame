using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeSceneScript : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingBar;
    public GameObject mapCheckPoints;
    public static string sceneName = "";

    IEnumerator ChangeSceneAsync(int sceneIndex)
    {
        if(sceneName == "Menu")
        {
            mapCheckPoints.SetActive(false);
            loadingScreen.SetActive(true);
        }       
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.fillAmount = progressValue;
            yield return null;
        }
    }

    //სცენის ცვლილება
    public void ChangeScene(int sceneIndex)
    {
        if(EventSystem.current.currentSelectedGameObject.name == "Start")
        {
            sceneName = "Menu";
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "PlayAgain")
        {
            sceneName = "Game";
        }
        StartCoroutine(ChangeSceneAsync(sceneIndex)); //სცენაზე შესვლის კოდი
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
