using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLeverScript : MonoBehaviour
{

    [Header ("Lista ovista mitä voi avata tällä vivulla")]
    public List<DoorScript> doors = new List<DoorScript> ( );

    string message;
    public float messageFontSize;
    public float messageDisplayTime;
    public float eventDuration;

    public delegate void DoorLeverActivationInfoDurationDelegate ( float duration );
    public static event DoorLeverActivationInfoDurationDelegate DoorLeverActivationInfoDurationEvent;

    public delegate void DoorLeverActivationFloatingTextDelegate ( Vector2 position, string message, Color color );
    public static event DoorLeverActivationFloatingTextDelegate LeverActivationFloatingTextEvent;

    public delegate void DoorLeverActivationInfoDelegate ( string message, Color color, float fontSize, float fadeTime );
    public static event DoorLeverActivationInfoDelegate LeverActivationInfoEvent;

    private void Awake ( )
    {
        if (transform.parent.position.x < transform.position.x)
        {
            message = "Sound of an old door opening in the west";
        }
        else
        {
            message = "Sound of an old door opening in the east";
        }
    }

    private void OnTriggerStay2D ( Collider2D collision )
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetButton("Interaction"))
            {
                int doorCount = 0;

                foreach ( var door in doors )
                {                    
                    if (!door.isOpen)
                    {
                        doorCount++;

                        if ( doorCount <= 1 )
                        {
                            OnLeverActivation ( );
                        }
                        door.Aukene ( );
                    }
                }               
            }
        }
    }
    public void OnLeverActivation()
    {
        if(DoorLeverActivationInfoDurationEvent != null)
        {
            DoorLeverActivationInfoDurationEvent ( messageDisplayTime );
        }

        if ( LeverActivationFloatingTextEvent != null)
        {
            LeverActivationFloatingTextEvent ( transform.position, "Clunk", Color.yellow );       
        }
        if(LeverActivationInfoEvent != null)
        {
            LeverActivationInfoEvent ( message, Color.white, messageFontSize , messageDisplayTime);
        }
    }
}
