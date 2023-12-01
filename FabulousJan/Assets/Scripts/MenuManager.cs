using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private string loadScene = "Intro";
    public void NextLevel() => SceneManager.LoadScene(loadScene);

}
