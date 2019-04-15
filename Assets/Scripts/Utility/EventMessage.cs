using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventMessage : MonoBehaviour
{

    public TextMeshProUGUI text;
    bool appearing;

    private void OnEnable ( )
    {
        DoorLeverScript.leverActivationInfoEvent += OnMessageReceive;
    }

    private void OnDisable ( )
    {
        DoorLeverScript.leverActivationInfoEvent -= OnMessageReceive;
    }

    public void OnMessageReceive( string message, Color color, float fontSize, float fadeTime )
    {
        appearing = true;
        text.text = message;
        text.color = color;
        text.fontSize = fontSize;

        StartCoroutine ( FadeTextToFullAlpha ( fadeTime*0.5f, text ) );
              
    }

    public IEnumerator FadeTextToFullAlpha ( float t, TextMeshProUGUI i )
    {
        Debug.Log ( appearing );      
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
        Debug.Log ( appearing );
        i.color = new Color ( i.color.r, i.color.g, i.color.b, 1 );
        while ( i.color.a > 0.0f )
        {
            i.color = new Color ( i.color.r, i.color.g, i.color.b, i.color.a - ( Time.deltaTime / t ) );
            yield return null;
        }
    }

}
