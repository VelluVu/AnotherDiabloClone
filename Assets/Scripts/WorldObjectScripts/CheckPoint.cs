using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pelaajan interactatessa activoi menun, aiheuittaa hirviöiden respawnin ja pysäyttää ajan
/// </summary>
public class CheckPoint : MonoBehaviour
{
   
    public GameObject checkPointMenu;
    bool interactingCheckPoint;
    bool ableToInteractCheckPoint;
    public AreaName areaName;

    public delegate void CheckPointDelegate (  );
    public static event CheckPointDelegate checkPointEvent;


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

        EnterCheckPoint ( );


    }

    public void EnterCheckPoint()
    {
        if ( ableToInteractCheckPoint && Input.GetButtonDown ( "Interaction" ) )
        {
            if ( checkPointEvent != null )
            {
                checkPointEvent ( );
            }

            interactingCheckPoint = true;

            Time.timeScale = 0;
        }
    }

    public void ResetPlayerPosition( Transform transform )
    {

        transform.position = this.transform.position;

        //if ( checkPointEvent != null )
        //{
        //    checkPointEvent ( );
        //}
    }
}
