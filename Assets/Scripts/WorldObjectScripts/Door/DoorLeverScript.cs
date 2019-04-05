using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLeverScript : MonoBehaviour
{

    [Header ("Lista ovista mitä voi avata tällä vivulla")]
    public List<DoorScript> doors = new List<DoorScript> ( );

    private void OnTriggerStay2D ( Collider2D collision )
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetButton("Interaction"))
            {
                foreach ( var door in doors )
                {
                    if(!door.isOpen)
                    {
                        door.Aukene ( );
                    }
                }
            }
        }
    }

}
