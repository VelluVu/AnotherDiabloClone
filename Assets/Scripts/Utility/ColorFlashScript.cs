using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlashScript : MonoBehaviour
{

    private void OnEnable ( )
    {
        Player.playerFlashEvent += ColorFlash;
        StateController.enemyFlashEvent += ColorFlash;
     }

    private void OnDisable ( )
    {
        Player.playerFlashEvent -= ColorFlash;
        StateController.enemyFlashEvent -= ColorFlash;
    }

    public IEnumerator ColorFlash ( GameObject source , float time, Color color, bool isFlashSpam )
    {
        SpriteRenderer [ ] renderers;
        renderers = source.GetComponentsInChildren<SpriteRenderer> ( );
        //Color _color = renderers [ 0 ].color;

        if ( !isFlashSpam )
        {
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
        else
        {
            for ( int t = 0 ; t < 5 ; t++ )
            {
                for ( int i = 0 ; i < renderers.Length ; i++ )
                {
                    if ( renderers [ i ] != null )
                    {
                        renderers [ i ].color = color;
                    }
                }

                yield return new WaitForSeconds ( time * 0.10f );

                for ( int i = 0 ; i < renderers.Length ; i++ )
                {
                    if ( renderers [ i ] != null )
                    {
                        renderers [ i ].color = Color.white;
                    }
                }
                yield return new WaitForSeconds ( time * 0.10f );
            }
        }
    }

}
