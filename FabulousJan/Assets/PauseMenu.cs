using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;
    public AudioSource pressButton;

    public void Resume() 
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause() 
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = .0f;
        isGamePaused = true;
    }

    public void playSoundEffect() 
    {
        pressButton.Play();
    }

}
