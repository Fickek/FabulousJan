using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public float changeTime;
    public string sceneName;

    void Update() 
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0) SceneManager.LoadScene(sceneName);
    }


}
