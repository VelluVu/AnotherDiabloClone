using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlashScript : MonoBehaviour
{

    public IEnumerator ColorFlash ( float time, Color color )
    {

        SpriteRenderer [ ] renderers;
        renderers = gameObject.GetComponentsInChildren<SpriteRenderer> ( );
        //Color _color = renderers [ 0 ].color;

        for ( int i = 0 ; i < renderers.Length ; i++ )
        {
            if ( renderers [ i ] != null )
            {
                renderers [ i ].color = color;
            }
        }

        yield return new WaitForSeconds ( time );

        for ( int i = 0 ; i < renderers.Length ; i++ )
        {
            if ( renderers [ i ] != null )
            {
                renderers [ i ].color = Color.white;
            }
        }

    }

}
