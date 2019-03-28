﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Yleinen StateContoller aseta viholliselle componentiksi ja huolehdi että animatorin animaatio parametrit on samalla nimellä, sekä silmät asetettu paikoilleen.
/// </summary>
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
    public float radius;

    #region Necessary Variables
    public Transform eyes; //silmät aseta manuaalisesti
    public GameObject head;
    public LayerMask enemyLayer;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D col;
    [HideInInspector] public Vector2 targetDir;
    [HideInInspector] public Vector3 leftDirection;
    [HideInInspector] public Vector3 rightDirection;
    [HideInInspector] public RaycastHit2D gaze;
    public GameObject dmgTextPrefab;
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
        targetDir = new Vector2 ( 1, 0 );
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

    /// <summary>
    /// Asettaa otuksen oikein päin
    /// </summary>
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

    /// <summary>
    /// Vaihtaa uuteen stateen
    /// </summary>
    /// <param name="nextState"></param>
    public void TransitionToState ( State nextState )
    {
        if ( nextState != remainState )
        {
            currentState = nextState;
            OnExitState ( );
        }
    }

    /// <summary>
    /// Lisää joka frame 1 stateTimeElapsed muuttujaan
    /// </summary>
    /// <param name="duration">ottaa vastaan durationi parametrin</param>
    /// <returns>Palauttaa booleanin onko statetimeelapsed enemmän kuin duration</returns>
    public bool CheckIfCountDownElapsed ( float duration )
    {
        
        stateTimeElapsed += Time.deltaTime;
        return ( stateTimeElapsed >= duration );

    }

    /// <summary>
    /// Kun State asetetaan stateTimeElapsed 0
    /// </summary>
    private void OnExitState ( )
    {
        stateTimeElapsed = 0;
    }

    /// <summary>
    /// Jos törmää esineeseen tai toiseen viholliseen kääntyy ympäri
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag ( "TreasureChest" ) || collision.gameObject.CompareTag ( "Door" ) || collision.gameObject.CompareTag ( "Wall" ) || collision.gameObject.CompareTag("Enemy"))
        {
            if ( dirRight ) { rb.velocity += Vector2.left; dirRight = false; } else { rb.velocity += Vector2.right; dirRight = true; }
            
        }
    }

    /// <summary>
    /// Perus liikkumisen animointi nopeuksien mukaan
    /// </summary>
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

    /// <summary>
    /// Katsoo ollaanko menossa laidanyli, jos ollaan niin käännytään ympäri
    /// </summary>
    void CheckLedge ( )
    {

        gaze = Physics2D.Raycast ( eyes.transform.position, -Vector2.up, sightDistance );

        if ( gaze.collider == false )
        {
            dirRight = dirRight ? false : true;
        }
    }

    /// <summary>
    /// Perus hyökkäys animaatio
    /// </summary>
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

    /// <summary>
    /// Tekee vahinkoa pelaajaan
    /// </summary>
    /// <param name="player"></param>
    public void DealDamage(Player player)
    {
        player.TakeDamage ( enemyStats.attackDamage );
    }

    /// <summary>
    /// Laskee tulevan vahingon lopullisen määrän
    /// </summary>
    /// <param name="dmg">Tuleva vahinko</param>
    public void TakeDamage ( float dmg )
    {            
        
        Debug.Log ( enemyStats.name + " PAIN SOUNDS ,,, " );
        enemyStats.health -= ( dmg - enemyStats.armor );
        Debug.Log ( dmg - enemyStats.armor );

        dmgTextPrefab.GetComponent<ScrollingCombatText> ( ).SpawnText ( dmg - enemyStats.armor , new Vector2(transform.position.x, head.transform.position.y));

        if (enemyStats.health <= 0)
        {
            Die ( );
        }

    }

    public void Die()
    {

        Debug.Log ( enemyStats.name + " IS DEAD" );
        //Instantiate death prefab
        Destroy ( gameObject );

    }

    #region Coroutines
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
    #endregion
}
