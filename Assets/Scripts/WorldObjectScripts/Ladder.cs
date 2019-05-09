using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    BoxCollider2D col;
    SpriteRenderer [ ] srs;
    public ClimpObjectType climpObjectType;
    Vector2 colBounds;

    private void Start ( )
    {
        if ( climpObjectType != ClimpObjectType.Robe)
        {
            col = gameObject.GetComponent<BoxCollider2D> ( );
            srs = gameObject.GetComponentsInChildren<SpriteRenderer> ( );
        
            colBounds = new Vector2 ( srs[0].sprite.bounds.size.x, 0 );

            for ( int i = 0 ; i < srs.Length ; i++ )
            {
                colBounds.y += srs [ i ].sprite.bounds.size.y;
            }

            col.offset = new Vector2 ( 0, colBounds.y / 2 - col.size.y /2);
            col.size = colBounds;
        }
    }
  
}
