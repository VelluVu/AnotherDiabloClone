using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{

    public Color gizmoColor;
    public Vector3 triggerArea;
    public AreaName triggerAreaName;
    bool triggered;
    BoxCollider2D box;

    public delegate void EnterAreaDelegate ( AreaName areaName );
    public static event EnterAreaDelegate EnterAreaEvent;

    private void Awake ( )
    {
        box = gameObject.GetComponent<BoxCollider2D> ( );
        box.size = new Vector2(triggerArea.x, triggerArea.y);
    }

    private void OnEnable ( )
    {
        EnterAreaEvent += MusicChanged;
    }

    private void OnDisable ( )
    {
        EnterAreaEvent -= MusicChanged;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag ( "Player" ) && !triggered )
        {
            triggered = true;

            if ( EnterAreaEvent != null )
            {
                EnterAreaEvent ( triggerAreaName );
            }
        }
    }

    public void MusicChanged(AreaName areaN)
    {
       if (areaN != triggerAreaName)
        {
            triggered = false;
        }
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube ( transform.position, triggerArea );

    }

}
