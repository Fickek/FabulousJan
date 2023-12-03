using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class WinController : MonoBehaviour
{

    //public GameObject player;
    //public GameObject ballSpawner;

    //private PlayableDirector director;
    //public GameObject controlPanel;


    [SerializeField] private string sceneName;

    void Awake()
    {
        //player = GetComponent<GameObject>();
        //ballSpawner = GameObject.FindGameObjectWithTag("BallSpawner");
        //director = GetComponent<PlayableDirector>();
        //director.played += Director_Played; 
        //director.stopped += Director_Stopped;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("play");

        SceneManager.LoadScene(sceneName);

        //timeline.SetActive(false);
        //controlPanel.SetActive(true);
        //director.Play();

    }

    //private void Director_Stopped(PlayableDirector obj) { controlPanel.SetActive(true); }
    //private void Director_Played(PlayableDirector obj) { controlPanel.SetActive(false); }

}
