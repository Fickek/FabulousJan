using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int _lives;
    private int _score;

    private void Start()
    {
        NewGame();
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

    public void LevelFailed()
    {
        //_lives--;

        RestartLevel();

        if (_lives <= 0)
        {
            //RestartLevel();
        }
        else {
            //Reload level
        }
        
    }
    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void PauseLevel() => Time.timeScale = 0;
    public void PlayLevel() => Time.timeScale = 1;
    


}