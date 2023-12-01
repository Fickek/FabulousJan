using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    private AudioSource _audioFX;
    [SerializeField] private AudioSource _audioEnv;
    public bool sound = true;

    void Awake()
    {
        MakeSingelton();
        _audioFX = GetComponent<AudioSource>();
        _audioEnv = GetComponent<AudioSource>();
    }

    private void Update()
    {
        PlaySoundEnv();
    }

    void MakeSingelton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SoundOff()
    {
        sound = !sound;
    }

    public void PlaySoundFX(AudioClip clip, float volume)
    {
        if (sound)
            _audioFX.PlayOneShot(clip, volume);        
    }

    public void PlaySoundEnv()
    {
            _audioEnv.Play();
    }

}
