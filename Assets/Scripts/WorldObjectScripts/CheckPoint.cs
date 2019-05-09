using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pelaajan interactatessa activoi menun, aiheuittaa hirviöiden respawnin ja pysäyttää ajan
/// </summary>
public class CheckPoint : MonoBehaviour
{
   
    //public GameObject checkPointMenu;
    bool interactingCheckPoint;
    bool ableToInteractCheckPoint;
    public AreaName areaName;
    public bool visited;

    public delegate void CheckPointDelegate (  );
    public static event CheckPointDelegate checkPointEvent;

    public delegate void CheckPointVisitDelegate ( Transform transform, int checkPointID );
    public static event CheckPointVisitDelegate CheckPointVisitEvent;


    private void OnEnable ( )
    {
        PlayerDeathUI.respawnPlayerEvent += ResetPlayerPosition;
        CheckPointUIScript.onExitCheckPointUIExitEvent += ExitCheckPoint;
    }

    private void OnDisable ( )
    {
        PlayerDeathUI.respawnPlayerEvent -= ResetPlayerPosition;
        CheckPointUIScript.onExitCheckPointUIExitEvent -= ExitCheckPoint;
    }

    public void ExitCheckPoint ( )
    {
        interactingCheckPoint = false;

        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
       
        if ( collision.gameObject.CompareTag("Player")  && !interactingCheckPoint )
        {

            ableToInteractCheckPoint = true;
            visited = true;

            
            //FindObjectOfType<SaveLoadManager> ( ).SaveGame ( ( int ) areaName );
            

            if (CheckPointVisitEvent != null)
            {
                CheckPointVisitEvent(transform, (int) areaName);
                
            }
        }
    }

    private void OnTriggerExit2D ( Collider2D collision )
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            ableToInteractCheckPoint = false;
            interactingCheckPoint = false;
        }
    }

    private void Update ( )
    {

        EnterCheckPoint ( );

    }

    public void EnterCheckPoint()
    {

        if ( ableToInteractCheckPoint && Input.GetButtonDown ( "Interaction" ) && !interactingCheckPoint )
        {

            interactingCheckPoint = true;

            if ( checkPointEvent != null )
            {
                checkPointEvent ( );
            }
            
            Time.timeScale = 0;
        }
    }

    public void ResetPlayerPosition( Transform transform )
    {

        //if ( checkPointEvent != null )
        //{
        //    checkPointEvent ( );
        //}
    }
}
