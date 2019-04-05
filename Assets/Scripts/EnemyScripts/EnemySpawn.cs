using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawnaa randomilla listassa olevan vihollisen
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    [Header ( "Lista mahdollisista vihollisista mitä spawnaa" )]
    public List<GameObject> enemies = new List<GameObject> ( );
    GameObject spawnedEnemy;
    public bool isSpawn;

    // Start is called before the first frame update
    void Start ( )
    {
        SpawnEnemy ( );
    }

    private void Update ( )
    {
        if(spawnedEnemy == null)
        {
            isSpawn = false;
        }
    }

    public void SpawnEnemy ( )
    {
        spawnedEnemy = Instantiate ( enemies[Random.Range ( 0, enemies.Count )], transform );
        isSpawn = true;
    }
}
