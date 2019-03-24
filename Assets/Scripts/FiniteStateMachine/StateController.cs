using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{

    public State currentState;
    public State remainState;

    //EnemyStats? Scriptable Object Class?
    public EnemyStats enemyStats;
  
    public float moveSpeedScale;
    public float attackCooldown;
    public float attackDistance;
    public float attackCD; 
    public float searchDuration; 
    public float spotDistance;
    public float sightDistance;

    public Transform eyes; //silmät
    public Transform chaseTarget;
    public Animator animator;
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public Vector2 pos;
    public Vector3 leftDirection;
    public Vector3 rightDirection;
    public RaycastHit2D gaze;
    public float stateTimeElapsed;
    public bool dirRight;
    public bool aiActive;
    public bool attackRdy;

    private void Awake ( )
    {
        //GetComponents
        eyes = transform.GetChild ( 0 );
        animator = gameObject.GetComponent<Animator> ( );
        rb = gameObject.GetComponent<Rigidbody2D> ( );
        col = gameObject.GetComponent<BoxCollider2D> ( );
        chaseTarget = GameObject.FindGameObjectWithTag ( "Player" ).transform;
        pos = new Vector2 ( 1, 0 );
        leftDirection = new Vector3 ( 0, -180, 0 );
        rightDirection = new Vector3 ( 0, 0, 0 );
    
    }

    private void Update ( )
    {

        Debug.Log ( currentState.name );

        if ( !aiActive )
        {
            return;
        }

        currentState.UpdateState ( this );
        AnimateMovement ( );
        TurnAround ( );

    }

    public void TransitionToState ( State nextState )
    {
        if ( nextState != remainState )
        {
            currentState = nextState;
            OnExitState ( );
        }
    }

    public bool CheckIfCountDownElapsed ( float duration )
    {
        stateTimeElapsed += Time.deltaTime;
        return ( stateTimeElapsed >= duration );
    }

    private void OnExitState ( )
    {
        stateTimeElapsed = 0;
    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Door" ) || collision.gameObject.CompareTag ( "Wall" ) )
        {
            if ( dirRight )
            {
                transform.eulerAngles = leftDirection;
                dirRight = false;
            }
            else if ( !dirRight )
            {
                transform.eulerAngles = rightDirection;
                dirRight = true;
            }
        }
    }
    public void AnimateMovement ( )
    {
        if ( rb.velocity.x >= 0.1f )
        {
            if ( rb.velocity.x >= 0.2f )
            {
                dirRight = true;
                transform.eulerAngles = rightDirection;
            }
            animator.SetBool ( "Walk", true );

        }
        else if ( rb.velocity.x <= -0.1f )
        {
            if ( rb.velocity.x <= -0.2f )
            {
                dirRight = false;
                transform.eulerAngles = leftDirection;
            }

            animator.SetBool ( "Walk", true );

        }
        else
        {
            animator.SetBool ( "Walk", false );
        }
    }
    void TurnAround ( )
    {

        gaze = Physics2D.Raycast ( eyes.transform.position, -Vector2.up, sightDistance );

        if ( gaze.collider == false )
        {
            if ( dirRight )
            {
                transform.eulerAngles = leftDirection;
                dirRight = false;
            }
            else
            {
                transform.eulerAngles = rightDirection;
                dirRight = true;
            }
        }
    }

    public void Attack ( )
    {
        Debug.Log ( "ATTACK" );
        if ( attackRdy )
        {       
            rb.AddForce ( new Vector2 ( rb.velocity.x,  transform.position.y + 2 * enemyStats.jumpForce )  * enemyStats.jumpForce, ForceMode2D.Impulse );
            animator.SetTrigger ( "Attack" );
            attackRdy = false;
        }
        StartCoroutine ( AttackCooldown ( enemyStats.attackSpeed ) );
    }

    IEnumerator AttackCooldown( float attackSpeed)
    {
        yield return new WaitForSeconds ( attackSpeed );
        attackRdy = true;
    }
}
