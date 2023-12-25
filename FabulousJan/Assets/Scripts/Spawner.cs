using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject _barrelSpawner;          
    [SerializeField] private float _averageTime = 3f;

    private float _timer = 3;

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _averageTime) 
        {
            Instantiate(_barrelSpawner, transform.position, Quaternion.identity);
            _timer -= _averageTime;
        }
       
    }

}