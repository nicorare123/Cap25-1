using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] Enemies;

    public Transform[] SpawnPoints;

    // Start is called before the first frame update
    void Start()
    {  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRocate()
    {
        for(int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnEnemy(Enemies, SpawnPoints[i]);
        }
    }

    void SpawnEnemy(GameObject[] EnemyList, Transform Point)
    {
        if(EnemyList.Length == 0)
        {
            return;
        }

        GameObject enemyPrefab = EnemyList[Random.Range(0,EnemyList.Length)];

        Instantiate(enemyPrefab, Point.position, Quaternion.identity);
    }
}
