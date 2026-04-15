using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class UI : MonoBehaviour
{

    public static bool isGamePaused = false;

    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private AudioClip _effectClickSound;
    [SerializeField] private AudioClip _effectVHSSound;

    public void Resume() 
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        AudioListener.pause = false;
    }

    public void Pause() 
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = .0f;
        isGamePaused = true;

        AudioListener.pause = true;

    }

    private void OnEnable()
    {
        playSoundEnviroment();
    }

    public void playSoundEffect() 
    {
        SoundManager.Instance.PlaySoundFX(_effectClickSound, 1f);
    }

    public void playSoundEnviroment() 
    {
        if(isGamePaused) SoundManager.Instance.PlaySoundEnv(_effectVHSSound);
    }

}
