using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    [SerializeField] private float _changeTime;
    [SerializeField] private string _sceneName;

    void Update() 
    {
        _changeTime -= Time.deltaTime;
        if (_changeTime <= 0 && _sceneName != null) SceneManager.LoadScene(_sceneName);
    }

    public void SkipCutScene() => SceneManager.LoadScene(_sceneName);

}
