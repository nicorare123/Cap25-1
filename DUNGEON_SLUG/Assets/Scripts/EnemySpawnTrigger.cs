using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public EnemySpawn enemySpawn;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(enemySpawn != null)
            {
                enemySpawn.SpawnRocate();
                Destroy(gameObject);
            }
        }
    }
}
