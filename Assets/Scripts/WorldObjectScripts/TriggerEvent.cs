using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{

    private void OnTriggerStay2D ( Collider2D collision )
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetButtonDown("Interaction"))
        {
            transform.parent.SendMessage ( "Triggered" );
        }
    }

}
