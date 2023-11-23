using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject playButton;

    public void PlayIntro()
    {
        SceneManager.LoadScene("Intro");
        Debug.Log("play");

    }
}
