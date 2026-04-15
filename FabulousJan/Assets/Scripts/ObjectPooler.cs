using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;


    void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);

            }
            poolDict.Add(pool.tag, objectPool);
        }
    }


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) 
    {
        if (!poolDict.ContainsKey(tag)) return null;

        GameObject objectToSpawn = poolDict[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDict[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}