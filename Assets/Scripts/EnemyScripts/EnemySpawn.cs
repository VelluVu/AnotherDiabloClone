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

    private void OnEnable ( )
    {
        CheckPoint.checkPointEvent += CheckPointCall;
    }

    private void OnDisable ( )
    {
        CheckPoint.checkPointEvent -= CheckPointCall;
    }

    // Start is called before the first frame update
    void Start ( )
    {
        SpawnEnemy ( );
    }

    private void Update ( )
    {
        if ( spawnedEnemy == null )
        {
            isSpawn = false;
        }
    }

    public void CheckPointCall()
    {
        SpawnEnemy ( );
    }

    public void SpawnEnemy ( )
    {
       
        if ( !isSpawn )
        {
            isSpawn = true;
            spawnedEnemy = Instantiate ( enemies [ Random.Range ( 0, enemies.Count ) ], transform );
        }
        else
        {
            Destroy ( spawnedEnemy );
            isSpawn = false;
            SpawnEnemy ( );
        }
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere ( transform.position, 0.1f);
    }
}
