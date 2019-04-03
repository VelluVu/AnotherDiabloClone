﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingCombatText : MonoBehaviour
{

    TextMeshProUGUI mesh;

    private void Start ( )
    {
        mesh = gameObject.GetComponentInChildren<TextMeshProUGUI> ( );
        StartCoroutine ( FadeTextToZeroAlpha ( 3f, mesh ));
        Destroy ( gameObject, 3f );
    }

    private void Update ( )
    {
        transform.eulerAngles = new Vector3 ( 0,0,0 );
        transform.Translate ( Vector2.up * 0.8f * Time.deltaTime );      
    }

    /// <summary>
    /// Default värinen numero teksti
    /// </summary>
    /// <param name="value"></param>
    /// <param name="spawnPos"></param>
    public void SpawnText( float value, Vector2 spawnPos)
    {
        string dmgText = value.ToString ( );

        //negatiivista dmg voi tulla
        if ( value < 0)
        {
            value = 0;
            dmgText = "block";
        }
        GameObject text = Instantiate ( gameObject, spawnPos, Quaternion.identity);
        text.GetComponentInChildren<TextMeshProUGUI> ( ).text = dmgText;

    }

    /// <summary>
    /// Valinnaisen Värinen Numero teksti
    /// </summary>
    /// <param name="value"></param>
    /// <param name="spawnPos"></param>
    /// <param name="textColor"></param>
    public void SpawnText ( float value, Vector2 spawnPos, Color textColor )
    {
        string dmgText = value.ToString ( );

        //negatiivista dmg voi tulla
        if ( value < 0 )
        {
            value = 0;
            dmgText = "block";
        }

        GameObject text = Instantiate ( gameObject, spawnPos, Quaternion.identity );
        TextMeshProUGUI textMesh = text.GetComponentInChildren<TextMeshProUGUI> ( );
        textMesh.color = textColor;
        textMesh.text = dmgText;

    }

    /// <summary>
    /// Default värinen floating text, string parametsilla
    /// </summary>
    /// <param name="message"></param>
    /// <param name="spawnPos"></param>
    public void SpawnText ( string message, Vector2 spawnPos)
    {
        
        GameObject text = Instantiate ( gameObject, spawnPos, Quaternion.identity );
        text.GetComponentInChildren<TextMeshProUGUI> ( ).text = message;

    }

    /// <summary>
    /// Värin vaihto jos haluaa esim ilmoituksia loot nostoista jne..
    /// </summary>
    /// <param name="message"></param>
    /// <param name="spawnPos"></param>
    /// <param name="textColor"></param>
    public void SpawnText ( string message, Vector2 spawnPos, Color textColor )
    {

        GameObject text = Instantiate ( gameObject, spawnPos, Quaternion.identity );
        TextMeshProUGUI textMesh = text.GetComponentInChildren<TextMeshProUGUI> ( );
        textMesh.color = textColor;
        textMesh.text = message;

    }

    public IEnumerator FadeTextToFullAlpha ( float t, TextMeshProUGUI i )
    {
        i.color = new Color ( i.color.r, i.color.g, i.color.b, 0 );
        while ( i.color.a < 1.0f )
        {
            i.color = new Color ( i.color.r, i.color.g, i.color.b, i.color.a + ( Time.deltaTime / t ) );
            yield return null;
        }
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