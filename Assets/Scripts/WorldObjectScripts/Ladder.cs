using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    BoxCollider2D col;
    SpriteRenderer [ ] srs;
    float bottom;
    float leftSide;

    private void Start ( )
    {
        col = gameObject.GetComponent<BoxCollider2D> ( );
        srs = gameObject.GetComponentsInChildren<SpriteRenderer> ( );

        bottom = srs [ 0 ].bounds.size.x;
        leftSide = srs.Length * srs [ 0 ].bounds.size.y;

        col.offset = new Vector2 (0, leftSide * 0.432f);
        col.size = new Vector2 ( bottom, leftSide );
    }


}
