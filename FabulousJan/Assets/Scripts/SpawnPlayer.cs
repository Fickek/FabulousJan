using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    public static SpawnPlayer instance;

    public GameObject PlayerPrefab;
    public GameObject spawnPoint;


    void Awake()
    {
        MakeSingelton();
    }

    public void SpawnPlayerToPointPosition() 
    {
        Instantiate(PlayerPrefab, new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y), Quaternion.identity);
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

}
