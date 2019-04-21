using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    Animator treasureAnimator;
    Collider2D treasureCollider;
    Collider2D triggerArea;
    bool canOpen;

    private void OnEnable ( )
    {
        OtherTriggerEvent.TriggerEnterEvent += OnTriggerEnterArea;
        OtherTriggerEvent.TriggerExitEvent += OnTriggerExitArea;
    }
    private void OnDisable ( )
    {
        OtherTriggerEvent.TriggerEnterEvent -= OnTriggerEnterArea;
        OtherTriggerEvent.TriggerExitEvent -= OnTriggerExitArea;
    }

    private void Start ( )
    {
        treasureAnimator = gameObject.GetComponent<Animator> ( );
        treasureCollider = gameObject.GetComponent<Collider2D> ( );
        triggerArea = gameObject.GetComponentInChildren<Collider2D> ( );
    }

    public void OnTriggerEnterArea ( GameObject targetObject )
    {
        if ( gameObject == targetObject )
        {
            canOpen = true;
        }
    }

    public void OnTriggerExitArea ( GameObject targetObject )
    {
        if ( gameObject == targetObject )
        {
            canOpen = false;
        }
    }

    private void Update ( )
    {
        Aukene ( );
    }
    public void Aukene ( )
    {
        if ( canOpen && Input.GetButton("Interaction"))
        {

            treasureAnimator.SetTrigger ( "Open" );
            triggerArea.enabled = false;
            treasureCollider.enabled = false;
            gameObject.GetComponent<TreasureChest> ( ).enabled = false;
        }
    }
}
