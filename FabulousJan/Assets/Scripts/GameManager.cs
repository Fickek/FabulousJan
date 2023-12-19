using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameManager Instance;

    //[SerializeField] private GameObject spawnPoint;

    //[SerializeField] private GameObject playerPrefab;

    private int _lives;
    private int _score;

    [SerializeField] private string playLevel = "";
    [SerializeField] private string tutorialScene = "";
    [SerializeField] private string menuScene = "";


    private void Awake()
    {
        //MakeSingelton();
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


    //public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //public void RestartLevel() => Instantiate(playerPrefab, new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y), Quaternion.identity);
     

    //public void PauseLevel() => Time.timeScale = 0;
    //public void PlayLevel() => Time.timeScale = 1;

    public void PlayLevel() => SceneManager.LoadScene(playLevel);
    public void TutorialScene() => SceneManager.LoadScene(tutorialScene);
    public void MenuScene() => SceneManager.LoadScene(menuScene);
    public void QuitGame() => Application.Quit();

}