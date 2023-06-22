using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject _spawner;
    [SerializeField] private float _minTime = 2f;
    [SerializeField] private float _maxTime = 4f;

    [SerializeField] private float _averageTime = 3f;

    private void Start()
    {
        Spawn();
    }


    private void Spawn()
    {
        Instantiate(_spawner, transform.position, Quaternion.identity);
        Invoke(nameof(Spawn), _averageTime);

    }

}
