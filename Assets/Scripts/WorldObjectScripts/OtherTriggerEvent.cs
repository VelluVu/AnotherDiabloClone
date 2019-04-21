using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherTriggerEvent : MonoBehaviour
{

    public GameObject myParent;
    public string TriggeringLayerName;

    public delegate void TriggerEnterEventDelegate ( GameObject parent );
    public static event TriggerEnterEventDelegate TriggerEnterEvent;

    public delegate void TriggerExitEventDelegate ( GameObject parent );
    public static event TriggerExitEventDelegate TriggerExitEvent;

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        Debug.Log ( collision.gameObject.layer );
        if ( collision.gameObject.layer == LayerMask.NameToLayer ( TriggeringLayerName ) )
        {
            if ( TriggerEnterEvent != null )
            {
                TriggerEnterEvent ( myParent );
            }
        }
    }

    private void OnTriggerExit2D ( Collider2D collision )
    {
        Debug.Log ( collision.gameObject.layer );
        if ( collision.gameObject.layer == LayerMask.NameToLayer ( TriggeringLayerName ) )
        {
            if ( TriggerExitEvent != null )
            {
                TriggerExitEvent ( myParent );
            }
        }
    }
}
