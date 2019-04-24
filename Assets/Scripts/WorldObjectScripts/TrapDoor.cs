using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{

    BoxCollider2D col;
    Animator anim;
    public float reactionTime;

    private void OnEnable ( )
    {
        OtherTriggerEvent.TriggerEnterEvent += TriggerTrap;
    }

    private void OnDisable ( )
    {
        OtherTriggerEvent.TriggerEnterEvent -= TriggerTrap;
    }

    private void Start ( )
    {
        col = gameObject.GetComponentInChildren<BoxCollider2D> ( );
        anim = gameObject.GetComponentInChildren<Animator> ( );
    }

    public void TriggerTrap( GameObject triggerTarget )
    {
        if ( triggerTarget == gameObject )
        {
            StartCoroutine ( ReactionTime ( reactionTime ) );
        }
    }

    IEnumerator ReactionTime( float time)
    {
        yield return new WaitForSeconds ( time );
        anim.SetTrigger ( "TriggerTrap" );
        col.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        
    }

}
