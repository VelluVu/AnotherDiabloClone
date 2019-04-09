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
   
    public GameObject checkPointMenu;
    bool interactingCheckPoint;
    bool ableToInteractCheckPoint;

    public delegate void CheckPointDelegate (  );
    public static event CheckPointDelegate checkPointEvent;

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

        if (ableToInteractCheckPoint && Input.GetButtonDown ( "Interaction" ) )
        {

            if(checkPointEvent != null)
            {
                checkPointEvent (  );
            }
        
            interactingCheckPoint = true;
            
            Time.timeScale = 0;
        }

        if ( Input.GetButtonDown ( "Cancel" ) && interactingCheckPoint)
        {
            interactingCheckPoint = false;
           
            Time.timeScale = 1;
        }
    }
}
