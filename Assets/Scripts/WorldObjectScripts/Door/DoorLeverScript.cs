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

    public delegate void LeverActivationDelegate ( float eventDuration );
    public static event LeverActivationDelegate leverActivationEvent;

    public delegate void LeverActivationFloatingTextDelegate ( Vector2 position, string message, Color color );
    public static event LeverActivationFloatingTextDelegate leverActivationFloatingTextEvent;

    public delegate void LeverActivationInfoDelegate ( string message, Color color, float fontSize, float fadeTime );
    public static event LeverActivationInfoDelegate leverActivationInfoEvent;

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
                foreach ( var door in doors )
                {
                    if(!door.isOpen)
                    {
                        OnLeverActivation ( );
                        door.Aukene ( );
                    }
                }
            }
        }
    }
    public void OnLeverActivation()
    {
        if ( leverActivationEvent != null )
        {
            leverActivationEvent ( eventDuration );
        }
        if ( leverActivationFloatingTextEvent != null)
        {
            leverActivationFloatingTextEvent ( transform.position, "Clunk", Color.yellow );       
        }
        if(leverActivationInfoEvent != null)
        {
            leverActivationInfoEvent ( message, Color.white, messageFontSize , messageDisplayTime);
        }
    }
}
