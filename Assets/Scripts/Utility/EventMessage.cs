using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventMessage : MonoBehaviour
{

    public TextMeshProUGUI text;

    private void OnEnable ( )
    {
        DoorLeverScript.LeverActivationInfoEvent += DoorLeverActivateMessage;
        ElevatorLever.OnElevatorLeverActivateEvent += ElevatorLeverActivateMessage;
        DoorScript.doorActivateMessageEvent += DoorMessageActivateMessage;
        
    }

    private void OnDisable ( )
    {
        DoorLeverScript.LeverActivationInfoEvent -= DoorLeverActivateMessage;
        ElevatorLever.OnElevatorLeverActivateEvent -= ElevatorLeverActivateMessage;
        DoorScript.doorActivateMessageEvent -= DoorMessageActivateMessage;
    }

    public void DoorMessageActivateMessage ( string message, Color color, float fontSize, float fadeTime )
    {

        text.text = message;
        text.color = color;
        text.fontSize = fontSize;

        StartCoroutine ( FadeTextToFullAlpha ( fadeTime * 0.5f, text ) );

    }

    public void DoorLeverActivateMessage( string message, Color color, float fontSize, float fadeTime )
    {
       
        text.text = message;
        text.color = color;
        text.fontSize = fontSize;

        StartCoroutine ( FadeTextToFullAlpha ( fadeTime*0.5f, text ) );
              
    }

    public void ElevatorLeverActivateMessage(Vector2 position, float fadeTime)
    {
        
        text.text = "An Elevator Started Moving Somewhere";
        text.color = Color.white;
        text.fontSize = 64;
   
        StartCoroutine ( FadeTextToFullAlpha ( fadeTime * 0.5f, text ) );
    }

    public IEnumerator FadeTextToFullAlpha ( float t, TextMeshProUGUI i )
    {          
        i.color = new Color ( i.color.r, i.color.g, i.color.b, 0 );
        while ( i.color.a < 1.0f )
        {
            i.color = new Color ( i.color.r, i.color.g, i.color.b, i.color.a + ( Time.deltaTime / t ) );
            yield return null;
        }
        StartCoroutine ( FadeTextToZeroAlpha ( t , text ) );

    }

    public IEnumerator FadeTextToZeroAlpha ( float t, TextMeshProUGUI i )
    {     
        i.color = new Color ( i.color.r, i.color.g, i.color.b, 1 );
        while ( i.color.a > 0.0f )
        {
            i.color = new Color ( i.color.r, i.color.g, i.color.b, i.color.a - ( Time.deltaTime / t ) );
            yield return null;
        }
    }

}
