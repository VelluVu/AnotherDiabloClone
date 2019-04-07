using System.Collections;
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
