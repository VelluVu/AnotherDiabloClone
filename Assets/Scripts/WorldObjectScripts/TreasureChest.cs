using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    Animator treasureAnimator;
    Collider2D treasureCollider;
    Collider2D triggerArea;

    
   
    private void Start ( )
    {
        treasureAnimator = gameObject.GetComponent<Animator> ( );
        treasureCollider = gameObject.GetComponent<Collider2D> ( );
        triggerArea = gameObject.GetComponentInChildren<Collider2D> ( );
    }

    public void Aukene ( )
    {

        treasureAnimator.SetTrigger ( "Open" );
        triggerArea.enabled = false;
        treasureCollider.enabled = false;
        gameObject.GetComponent<TreasureChest> ( ).enabled = false;
        

    } 

    public void Triggered()
    {
        Aukene ( );
    }

}
