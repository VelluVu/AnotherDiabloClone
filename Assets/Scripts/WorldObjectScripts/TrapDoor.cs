using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{

    BoxCollider2D col;
    BoxCollider2D triggerCol;
    SpriteRenderer sr;
    Animator anim;
    public float reactionTime;
    AudioSource source;

    public delegate void TrapDoorSoundDelegate ( AudioSource source, ObjectSoundType objSound );
    public static event TrapDoorSoundDelegate TrapDoorSoundEvent;

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
        sr = gameObject.GetComponentInChildren<SpriteRenderer> ( );
        triggerCol = transform.GetChild ( 1 ).GetComponent<BoxCollider2D>();
        anim = gameObject.GetComponentInChildren<Animator> ( );
        source = gameObject.GetComponent<AudioSource> ( );
    }

    public void TriggerTrap( GameObject triggerTarget )
    {
        if ( triggerTarget == gameObject )
        {
            if ( TrapDoorSoundEvent != null )
            {
                TrapDoorSoundEvent ( source, ObjectSoundType.TrapDoor );
            }
            StartCoroutine ( ReactionTime ( reactionTime ) );
        }
    }

    IEnumerator ReactionTime( float time)
    {
        yield return new WaitForSeconds ( time );
        anim.SetTrigger ( "TriggerTrap" );
        col.enabled = false;
        triggerCol.enabled = false;
        StartCoroutine ( FadeSpriteToFullAlpha ( sr , 0.5f) );
        gameObject.layer = LayerMask.NameToLayer("Default");       
        
    }

    public IEnumerator FadeSpriteToFullAlpha(SpriteRenderer spriteRenderer, float t)
    {
        spriteRenderer.color = new Color ( spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0 );
        while ( spriteRenderer.color.a < 1.0f )
        {
            spriteRenderer.color = new Color ( spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + ( Time.deltaTime / t ) );
            yield return null;
        }
    }
}
