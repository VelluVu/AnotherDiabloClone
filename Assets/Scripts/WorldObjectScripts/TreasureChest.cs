using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    Animator treasureAnimator;
    BoxCollider2D treasureCollider;

    private void Start ( )
    {
        treasureAnimator = gameObject.GetComponent<Animator> ( );
        treasureCollider = gameObject.GetComponent<BoxCollider2D> ( );
    }

    public void Aukene ( )
    {

        treasureAnimator.SetTrigger ( "Open" );
        treasureCollider.enabled = false;
        gameObject.GetComponent<TreasureChest> ( ).enabled = false;

    }

}
