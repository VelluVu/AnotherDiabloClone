using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pelaajan interactatessa activoi menun, aiheuittaa hirviöiden respawnin ja pysäyttää ajan
/// Menu:
/// 1. Pelaaja voi asettaa levelpojoja / skillpojoja -> avaa toisen valikon
/// 2. Pelaaja voi vaihtaa skillejä, mitä osaa -> avaa toisen valikon
/// 3. Pelaaja voi portata erialueelle -> avaa kartan / valikon missä lista paikoista
/// 4. Pelaaja voi tallentaa/load 
/// </summary>
public class CheckPoint : MonoBehaviour
{
    Player player;
    public GameObject checkPointMenu;
    bool interactingCheckPoint;
    bool ableToInteractCheckPoint;
    GameObject [ ] enemySpawnPoints;

    public delegate void CheckPointDelegate ( );
    public static event CheckPointDelegate checkPointEvent;

    private void Awake ( )
    {
        enemySpawnPoints = GameObject.FindGameObjectsWithTag ( "EnemySpawn" );
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
       
        if ( collision.gameObject.CompareTag("Player")  && !interactingCheckPoint )
        {
            ableToInteractCheckPoint = true;
            player = GameObject.FindGameObjectWithTag ( "Player" ).GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D ( Collider2D collision )
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            ableToInteractCheckPoint = false;
        }
    }

    private void Update ( )
    {

        if (ableToInteractCheckPoint && Input.GetButtonDown ( "Interaction" ) )
        {

            if(checkPointEvent != null)
            {
                checkPointEvent ( );
            }

            foreach ( var spawn in enemySpawnPoints )
            {
                if ( !spawn.GetComponent<EnemySpawn> ( ).isSpawn )
                {
                    spawn.GetComponent<EnemySpawn> ( ).SpawnEnemy ( );
                }
            }

            if ( player != null )
            {
                player.HealHealth ( 0, true );
                player.RestoreMana ( 0, true );
            }

            interactingCheckPoint = true;
            //checkPointMenu.SetActive ( true );
            Time.timeScale = 0;
        }

        if ( Input.GetButtonDown ( "Cancel" ) && interactingCheckPoint)
        {
            interactingCheckPoint = false;
            //checkPointMenu.SetActive ( false );
            Time.timeScale = 1;
        }
    }
}
