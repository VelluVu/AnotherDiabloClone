using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    Animator doorAnimator;
    BoxCollider2D doorCollider;

    private void Start ( )
    {
        doorAnimator = gameObject.GetComponent<Animator> ( );
        doorCollider = gameObject.GetComponent<BoxCollider2D> ( );
    }

    public void Aukene()
    {
        
        doorAnimator.SetTrigger ( "OpenDoor" );
        doorCollider.enabled = false;
        gameObject.GetComponent<DoorScript> ( ).enabled = false;

    }
    

}
