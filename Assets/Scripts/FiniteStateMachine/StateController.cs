using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [Header ("Currently In This State")]
    public State currentState;
    [Header ( "State To Remain" )]
    public State remainState;

    //EnemyStats? Scriptable Object Class?
    [Header ( "EnemyStats Class" )]
    public EnemyStats enemyStats;

    #region SomeMovement variables
    public float attackJump; //could be in enemyStats
    public float moveSpeedScale; //could be in enemyStats
    public float attackDistance; //could be in enemyStats
    public float searchDuration; //could be in enemyStats
    public float spotDistance; //could be in enemyStats
    public float sightDistance; //could be in enemyStats
    #endregion

    public float stateTimeElapsed;

    #region Necessary Variables
    public Transform eyes; //silmät
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D col;
    [HideInInspector] public Vector2 pos;
    [HideInInspector] public Vector3 leftDirection;
    [HideInInspector] public Vector3 rightDirection;
    [HideInInspector] public RaycastHit2D gaze;
    #endregion

    #region Booleans
    public bool dirRight;
    public bool aiActive;
    public bool attackRdy;
    #endregion

    private void Awake ( )
    {
        //GetComponents
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
        //Debug.Log ( currentState.name );

        if ( !aiActive )
        {
            return;
        }

        currentState.UpdateState ( this );
        AnimateMovement ( );
        CheckLedge ( );
        CheckDirection ( );
    }

    public void CheckDirection()
    {
        if ( dirRight )
        {
           
            transform.eulerAngles = rightDirection;
        }
        else
        {
          
            transform.eulerAngles = leftDirection;
        }
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
        if ( collision.gameObject.CompareTag ( "TreasureChest" ) || collision.gameObject.CompareTag ( "Door" ) || collision.gameObject.CompareTag ( "Wall" ) || collision.gameObject.CompareTag("Enemy"))
        {
            if ( dirRight ) { rb.velocity += Vector2.left; dirRight = false; } else { rb.velocity += Vector2.right; dirRight = true; }
            
        }
    }
    public void AnimateMovement ( )
    {
        if ( rb.velocity.x >= 0.01f )
        {          
            animator.SetBool ( "Walk", true );
        }
        else if ( rb.velocity.x <= -0.01f )
        {         
            animator.SetBool ( "Walk", true );
        }
        else
        {
            animator.SetBool ( "Walk", false );
        }
    }
    void CheckLedge ( )
    {

        gaze = Physics2D.Raycast ( eyes.transform.position, -Vector2.up, sightDistance );

        if ( gaze.collider == false )
        {
            dirRight = dirRight ? false : true;
        }
    }

    public void Attack ( )
    {
        Debug.Log ( enemyStats.name + " ATTACK" );
        if ( attackRdy )
        {
            attackRdy = false;
            rb.AddForce ( new Vector2( rb.velocity.x, 1) * attackJump, ForceMode2D.Impulse );
            animator.SetTrigger ( "Attack" );
            eyes.GetComponent<BoxCollider2D> ( ).enabled = true;
            StartCoroutine ( AttackHitBoxDuration() );
            StartCoroutine ( AttackCooldown ( ) );
        }
        
    }

    public void DealDamage(Player player)
    {
        player.TakeDamage ( enemyStats.attackDamage );
    }

    public void TakeDamage ( float dmg )
    {            
        
        Debug.Log ( enemyStats.name + " PAIN SOUNDS ,,, " );
        enemyStats.health -= ( dmg - enemyStats.armor );

        if(enemyStats.health <= 0)
        {
            Die ( );
        }

    }

    public void Die()
    {

        Debug.Log ( enemyStats.name + " IS DEAD" );

    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds ( enemyStats.attackSpeed );
        attackRdy = true;
    }

    IEnumerator AttackHitBoxDuration()
    {
        yield return new WaitForSeconds ( 1 * enemyStats.attackSpeed );
        eyes.GetComponent<BoxCollider2D> ( ).enabled = false;

    } 
}
