using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private GameObject playerPrefab;

    private int _lives;
    private int _score;

    [SerializeField] private string playLevel = "";
    [SerializeField] private string tutorialScene = "";
    [SerializeField] private string menuScene = "";


    private void Awake()
    {
        NewGame();
        MakeSingelton();
    }

    private void NewGame()
    {
        _score = 0;
        _lives = 3;
    }

    public void LevelComplete() 
    {
        _score += 1000;
        //Load new level
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

    public void LevelFailed()
    {
        //_lives--;

        if (_lives <= 0)
        {
            //RestartLevel();
        }
        else 
        {
            RestartLevel();
        }

        
    }
    //public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void RestartLevel() => Instantiate(playerPrefab, new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y), Quaternion.identity);
     

    //public void PauseLevel() => Time.timeScale = 0;
    //public void PlayLevel() => Time.timeScale = 1;

    public void PlayLevel() => SceneManager.LoadScene(playLevel);
    public void TutorialScene() => SceneManager.LoadScene(tutorialScene);
    public void MenuScene() => SceneManager.LoadScene(menuScene);
    public void QuitGame() => Application.Quit();

}