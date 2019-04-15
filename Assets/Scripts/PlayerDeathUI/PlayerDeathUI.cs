using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathUI : MonoBehaviour
{

    public Button [ ] yesAndNo;
    Transform respawnPosition;
    public delegate void RespawnPlayerDelegate ( Transform transform );
    public static event RespawnPlayerDelegate respawnPlayerEvent;

    private void Awake ( )
    {
        yesAndNo [ 0 ].onClick.AddListener ( ( ) => RespawnPlayer ( ) );
        yesAndNo [ 1 ].onClick.AddListener ( ( ) => QuitToTheMenu ( ) );
    }

    private void OnEnable ( )
    {
        Player.playerDeathEvent += ActivateDeathUI;
    }

    private void OnDisable ( )
    {
        Player.playerDeathEvent -= ActivateDeathUI;
    }

    public void ActivateDeathUI( Transform playerPosition)
    {
        respawnPosition = playerPosition;
        gameObject.transform.GetChild ( 0 ).gameObject.SetActive ( true );

    }

    /// <summary>
    /// Player Clicked Respawn
    /// </summary>
    void RespawnPlayer()
    {

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
