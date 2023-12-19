using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectSource;

    public bool sound = true;

    void Awake()
    {
        MakeSingelton();
    }

    void MakeSingelton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SoundOff()
    {
        sound = !sound;
    }

    public void PlaySoundFX(AudioClip clip, float volume)
    {
        if (sound) _effectSource.PlayOneShot(clip, volume);    
        
        if(UI.isGamePaused) 
        {
            _effectSource.ignoreListenerPause = true;
            _musicSource.ignoreListenerPause = true;
        }
        else 
        {
            _effectSource.ignoreListenerPause = false;
            _musicSource.ignoreListenerPause = false;
        }

    }

    public void PlaySoundEnv(AudioClip clip)
    {
        _musicSource.PlayOneShot(clip);
    }


}
