using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathUI : MonoBehaviour
{

    public Button [ ] yesAndNo;
    public Transform respawnPosition;
    AudioSource source;

    public delegate void RespawnPlayerDelegate ( Transform transform );
    public static event RespawnPlayerDelegate respawnPlayerEvent;

    private void Awake ( )
    {
        yesAndNo [ 0 ].onClick.AddListener ( ( ) => RespawnPlayer ( ) );
        yesAndNo [ 1 ].onClick.AddListener ( ( ) => QuitToTheMenu ( ) );
        source = gameObject.GetComponent<AudioSource> ( );
    }

    private void OnEnable ( )
    {
        Player.playerDeathEvent += ActivateDeathUI;
        CheckPoint.CheckPointVisitEvent += CheckPointVisit;
    }

    private void OnDisable ( )
    {
        Player.playerDeathEvent -= ActivateDeathUI;
        CheckPoint.CheckPointVisitEvent -= CheckPointVisit;
    }

    public void CheckPointVisit(Transform transform, int checkpointID)
    {
        respawnPosition = transform;
    }

    public void ActivateDeathUI( Transform deathPos)
    {       
        gameObject.transform.GetChild ( 0 ).gameObject.SetActive ( true );
        source.Play ( );

    }

    /// <summary>
    /// Player Clicked Respawn
    /// </summary>
    void RespawnPlayer()
    {
        source.Stop ( );
        gameObject.transform.GetChild ( 0 ).gameObject.SetActive ( false );

        if (respawnPlayerEvent != null)
        {
            respawnPlayerEvent ( respawnPosition );
        }

    }

    /// <summary>
    /// Quit to the menu
    /// </summary>
    void QuitToTheMenu()
    {

        //gameObject.transform.GetChild ( 0 ).gameObject.SetActive ( false );

        Debug.Log ( "ExitGame" );
    }

}
