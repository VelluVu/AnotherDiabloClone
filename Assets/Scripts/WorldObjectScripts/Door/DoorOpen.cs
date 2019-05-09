using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    public GameObject inputInfoPrefab;
    GameObject newInputInfo;

    private void OnTriggerStay2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Player" ) && Input.GetButtonDown ( "Interaction" ) )
        {
            //transform.parent.SendMessage ( "DoorConditionCheck" );
            transform.parent.GetComponent<DoorScript> ( ).DoorConditionCheck ( );
        }
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Player" ) && !transform.parent.GetComponent<DoorScript> ( ).isOpen)
        {
            if ( newInputInfo == null )
            {
                newInputInfo = Instantiate ( inputInfoPrefab, new Vector3 ( transform.position.x, transform.position.y + 1.5f, 0 ), Quaternion.identity, transform );

            }
            else
            { 
                newInputInfo.SetActive ( true );
            }
        }
    }

    private void OnTriggerExit2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Player" ) )
        {
            newInputInfo.SetActive ( false );
        }
    }

}
