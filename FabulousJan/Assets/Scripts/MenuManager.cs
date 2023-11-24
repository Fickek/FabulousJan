using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    string loadScene = "Intro";

    public void PlayIntro()
    {
        SceneManager.LoadScene(loadScene);
        Debug.Log("play");

    }
}
