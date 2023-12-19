using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class UI : MonoBehaviour
{

    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;
    public GameObject MobileController;

    [SerializeField] private AudioClip effectClickSound;
    [SerializeField] private AudioClip effectVHSSound;

    public void Resume() 
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        AudioListener.pause = false;

    }

    public void Pause() 
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = .0f;
        isGamePaused = true;

        AudioListener.pause = true;

    }

    public void playSoundEffect() 
    {
        SoundManager.Instance.PlaySoundFX(effectClickSound, 1f);
    }

    public void playSoundEnviroment()
    {
        if(isGamePaused) SoundManager.Instance.PlaySoundEnv(effectVHSSound);
    }

}
