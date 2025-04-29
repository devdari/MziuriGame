using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    //სცენის ცვლილება
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex); //სცენაზე შესვლის კოდი
    }
}
