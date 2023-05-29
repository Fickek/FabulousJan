using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void LevelComplected() 
    {
        _score += 1000;
        //Load new level
    }

    public void LevelFailed()
    {
        _lives--;

        if(_lives <= 0)
        {
            NewGame();
        }
        else {
            //Reload level
        }
        
    }

}
