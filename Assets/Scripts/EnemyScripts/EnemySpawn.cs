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
    public LayerMask playerLayer;
    public bool isSpawn;
    public float spawnCheckRadius;

    private void OnEnable ( )
    {
        CheckPoint.checkPointEvent += CheckPointCall;
        PlayerDeathUI.respawnPlayerEvent += RespawnEnemy;
    }

    private void OnDisable ( )
    {
        CheckPoint.checkPointEvent -= CheckPointCall;
        PlayerDeathUI.respawnPlayerEvent -= RespawnEnemy;
    }

    private void Update ( )
    {
        if ( !isSpawn )
        {
            SpawnEnemy ( );
        }
        if ( isSpawn && spawnedEnemy != null )
        {
            if ( Physics2D.OverlapCircle ( transform.position, spawnCheckRadius, playerLayer ) == null )
            {
                spawnedEnemy.transform.position = transform.position;
                spawnedEnemy.SetActive ( false );
            }
            else
            {
                spawnedEnemy.SetActive ( true );
            }
        }
    }
    public void CheckPointCall ( )
    {
        isSpawn = false;
    }

    public void SpawnEnemy ( )
    {

        if ( Physics2D.OverlapCircle ( transform.position, spawnCheckRadius, playerLayer ) )
        {
            if ( !isSpawn )
            {
                isSpawn = true;

                if ( spawnedEnemy == null )
                {
                    spawnedEnemy = Instantiate ( enemies [ Random.Range ( 0, enemies.Count ) ], transform );
                }
            }
        }
    }

    public void RespawnEnemy ( Transform playerPos )
    {

        isSpawn = false;

    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere ( transform.position, 0.1f );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere ( transform.position, spawnCheckRadius );
    }
}
