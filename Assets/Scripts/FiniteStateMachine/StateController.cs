using Kryz.CharacterStats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Yleinen StateContoller aseta viholliselle componentiksi ja huolehdi että animatorin animaatio parametrit on samalla nimellä, sekä silmät asetettu paikoilleen.
/// </summary>
[RequireComponent ( typeof ( EnemyStats ) )]
public class StateController : MonoBehaviour
{

    [Tooltip ( "Currently In This State" )]
    public State currentState;
    [Tooltip ( "State To Remain" )]
    public State remainState;

    //EnemyStats? Scriptable Object Class?
    [Tooltip ( "EnemyStats Class" )]
    public EnemyStats enemyStats;

    //Jos näissä haluaa saada selville tietyn vihollisen niin lisätään Transform/GameObject parametri
    #region Enemy Delegates
    public delegate void EnemyNotifyDelegate ( Transform transform, string message, Color color );
    public static event EnemyNotifyDelegate enemyNotifyEvent;

    public delegate void EnemyTakeDamageDelegate ( GameObject origin, float damage, DamageType damageType );
    public static event EnemyTakeDamageDelegate enemyTakeDamageEvent;

    public delegate void EnemyDealDamageDelegate ( GameObject target, float damage, DamageType damageType, int level );
    public static event EnemyDealDamageDelegate enemyDealDamageEvent;

    public delegate void EnemyDeathDelegate ( Transform transform, int xp, StateController origin );
    public static event EnemyDeathDelegate enemyDeathEvent;

    public delegate IEnumerator EnemyFlashDelegate ( GameObject source, float time, Color color, bool isFlashSpam );
    public static event EnemyFlashDelegate enemyFlashEvent;

    public delegate void EnemyHealHealthDelegate ( float amount );
    public static event EnemyHealHealthDelegate enemyHealHealthEvent;

    public delegate void EnemyRestoreManaDelegate ( float amount );
    public static event EnemyRestoreManaDelegate enemyRestoreManaEvent;
    #endregion

    #region Public Variables
    [Header ( "Range and scalar variables" )]
    [Tooltip ( "For Enemies that jump on basic attack" )]
    public float attackJump; //could be in enemyStats
    [Tooltip ( "Scalar for movespeed" )] [Range ( 0.1f, 5f )] public float moveSpeedScale; //could be in enemyStats
    [Tooltip ( "The Melee attack state distance from enemy eyes to target" )] [Range ( 0.01f, 5f )] public float attackDistance; //could be in enemyStats
    [Tooltip ( "Duration for Enemy search after losing the target" )] [Range ( 0.1f, 10f )] public float searchDuration; //could be in enemyStats
    [Tooltip ( "Enemy spot range, from eyes to the target. Used in patrol to chasestate" )] [Range ( 0.1f, 10f )] public float spotDistance; //could be in enemyStats
    [Tooltip ( "Raycast total distance" )] [Range ( 0.1f, 20f )] public float sightDistance; //could be in enemyStats
    [Tooltip ( "Turns on when event happens nearby" )] [Range ( 0.1f, 10f )] public float alertedByEventDuration;
    [Tooltip ( "The Elapsed time in current state" )] public float stateTimeElapsed;
    [Tooltip ( "Radius for randomstuff" )] [Range ( 0.01f, 5f )] public float radius;
    [Tooltip ( "Radius for hunching different events and flying enemys spot distance" )] [Range ( 0.01f, 5f )] public float senseArea;
    [Tooltip ( "Radius for circleCasts" )] [Range ( 0.01f, 5f )] public float circleCastRadius;
    public float fallToDeathHeight;
    float slowEffect;

    [ Tooltip ( "For SpiderBoss" )] public float riseUpTime;
    public float riseUpTimeCounter;
    public bool isUpPosition;
    public bool changedPos;
    public bool phase1;
    public bool phase2;
    public bool phase3;
    public bool phase4;

    [Header ( "Flying enemy motion variables" )]
    [Range ( 0.1f, 5f )] public float amplitude;
    [Range ( 0.1f, 5f )] public float frequency;
    [Range ( 0.1f, 5f )] public float flyingPatrolDirectionTime;
    [Space]
    #endregion

    #region Necessary Variables
    [Header ( "Components for raycasts and checks" )]
    [Tooltip ( "Transform for Look and Ledgecheck Raycasts" )] public Transform eyes;
    [Tooltip ( "Enemy Back" )] public Transform back;
    [Tooltip ( "Head position and possibly for hit data etc." )] public GameObject head;
    public Transform attackBox;

    [Tooltip ( "LayerMasks for raycast detection" )] public LayerMask friendlyTargetLayer;
    [Tooltip ( "LayerMasks for raycast detection" )] public LayerMask groundLayer;
    [Tooltip ( "LayerMasks for raycast detection" )] public LayerMask targetLayer;
    [Tooltip ( "LayerMasks for raycast detection" )] public LayerMask blockSightLayer;

    [Tooltip ( "Humanoid enemy weapons" )] public EnemyWeaponHolder weaponLeft;
    [Tooltip ( "Humanoid enemy weapons" )] public EnemyWeaponHolder weaponRight;

    public Transform chaseTarget;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D col;
    [HideInInspector] public Vector2 targetDir;
    [HideInInspector] public Vector3 leftDirection;
    [HideInInspector] public Vector3 rightDirection;
    [HideInInspector] public RaycastHit2D gaze;
    [Tooltip ( "Phase Positions for Boss" )] public List<Transform> positions = new List<Transform> ( );
    #endregion

    #region Booleans
    [Header ( "Enemy Booleans" )]
    public bool dirRight;
    public bool aiActive;
    public bool attackRdy;
    public bool hasTurn;
    public bool hasSplit;
    public bool alertedByEvent;
    public bool notHatchedEgg;
    bool slowed;
    #endregion

    private void Start ( )
    {
        //GetComponents
        animator = gameObject.GetComponent<Animator> ( );

        if ( animator == null )
        {
            animator = gameObject.GetComponentInChildren<Animator> ( );
        }
        rb = gameObject.GetComponent<Rigidbody2D> ( );
        col = gameObject.GetComponent<BoxCollider2D> ( );
        targetDir = new Vector2 ( 1, 0 );
        leftDirection = new Vector3 ( 0, -180, 0 );
        rightDirection = new Vector3 ( 0, 0, 0 );
        attackRdy = true;
        slowEffect = 1;
        if ( enemyStats.enemyType != EnemyType.Boss )
        {
            aiActive = true; //Asetetaan herätessä aktiiviseksi bossi asetetaan aktiiviseksi kun on saapunut alueelle
        }
    }

    private void OnEnable ( )
    {
        StartCoroutine ( AlertedByEventTime ( alertedByEventDuration ) );
        StartCoroutine ( AttackHitBoxDuration ( ) );
        StartCoroutine ( AttackCooldown() );
        StartCoroutine ( RemoveSlow ( ) );
        StartCoroutine ( EggCd ( ) );
        Player.playerDealDamageEvent += TakeDamage;
        Player.playerDeathEvent += PlayerIsDeadWeWin;
        CheckPoint.checkPointEvent += DestroySpawnings;
        PlayerDeathUI.respawnPlayerEvent += ResetEnemiesOnPlayerRespawn;
        enemyTakeDamageEvent += OnEnemyTakeDamage;
        SpiderEgg.hatchEvent += Hatched;
        SpiderEgg.eggDestroyedEvent += HatchedDestroyed; //Tähän vois lisätä omat functiot vaik et hämy menee sekasi ja hyökkää pelaajaan
    }

    private void OnDisable ( )
    {
        Player.playerDealDamageEvent -= TakeDamage;
        Player.playerDeathEvent -= PlayerIsDeadWeWin;
        CheckPoint.checkPointEvent -= DestroySpawnings;
        PlayerDeathUI.respawnPlayerEvent -= ResetEnemiesOnPlayerRespawn;
        enemyTakeDamageEvent -= OnEnemyTakeDamage;
        SpiderEgg.hatchEvent -= Hatched;
        SpiderEgg.eggDestroyedEvent -= HatchedDestroyed;
    }

    private void Update ( )
    {
        //Debug.Log ( currentState.name );
        if ( Input.GetButtonDown ( "Submit" ) )
        {
            aiActive = true;
        }

        if ( !aiActive )
        {
            return;
        }

        currentState.UpdateState ( this );

        AnimateMovement ( );
        //Lentävän vihollisen ei tarvi pelätä tippumista
        if ( enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            CheckLedge ( );
        }

        CheckDirection ( );
        EnemyFallToDeath ( );
        CheckSlows ( );
    }

    private void CheckSlows ( )
    {
        if ( slowed )
        {
            enemyStats.moveSpeed.BaseValue *= slowEffect;
        }
        else
        {
            slowEffect = 1;
        }
    }

    /// <summary>
    /// Asettaa otuksen oikein päin
    /// </summary>
    public void CheckDirection ( )
    {
        if ( dirRight )
        {
            transform.eulerAngles = rightDirection;
        }
        else if ( !dirRight )
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
        if ( collision.gameObject.CompareTag ( "InteractableObject" ) || collision.gameObject.CompareTag ( "Enemy" ) || collision.gameObject.CompareTag ( "Wall" ) )
        {
            if ( dirRight )
            {
                rb.velocity += Vector2.left;
                dirRight = false;
            }
            else
            {
                rb.velocity += Vector2.right;
                dirRight = true;
            }

        }
    }

    public void DestroySpawnings ( )
    {
        if ( enemyStats.enemyType != EnemyType.Boss )
        {
            Destroy ( gameObject );
        }
        if ( enemyStats.enemyType == EnemyType.Boss )
        {
            enemyStats.ResetEnemy ( );
        }

    }

    public void ResetEnemiesOnPlayerRespawn ( Transform playerPos )
    {
        if ( enemyStats.enemyType != EnemyType.Boss )
        {
            Destroy ( gameObject );
        }
        if ( enemyStats.enemyType == EnemyType.Boss )
        {
            enemyStats.ResetEnemy ( );
            chaseTarget = null;
            phase1 = phase2 = phase3 = phase4 = false;
            aiActive = true;
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

        gaze = Physics2D.Raycast ( eyes.transform.position, -Vector2.up, 3, groundLayer );

        if ( gaze.collider == false )
        {
            if ( dirRight )
            {
                dirRight = false;
            }
            else
            {
                dirRight = true;
            }
        }
    }

    public void Hatched ( GameObject parent )
    {
        if ( parent == gameObject )
        {
            notHatchedEgg = false;
        }
    }

    public void HatchedDestroyed ( GameObject parent )
    {
        if ( parent == gameObject )
        {
            StartCoroutine ( EggCd ( ) );
        }
    }

    /// <summary>
    /// Perus hyökkäys animaatio
    /// </summary>
    public void Attack ( )
    {
        if ( attackRdy )
        {
            attackRdy = false;
            rb.AddForce ( new Vector2 ( rb.velocity.x * 2, 2 ) * attackJump, ForceMode2D.Impulse );
            attackBox.GetComponent<BoxCollider2D> ( ).enabled = true;
            animator.SetTrigger ( "Attack" );
            StartCoroutine ( AttackCooldown ( ) );
        }
    }

    public void OnEnemyHeal ( float amount )
    {

        enemyStats.RestoreEnemyHealth ( amount );

        if ( enemyHealHealthEvent != null )
        {
            enemyHealHealthEvent ( amount );
        }
    }

    public void OnEnemyRestoreMana ( float amount )
    {

        enemyStats.RestoreEnemyMana ( amount );

        if ( enemyRestoreManaEvent != null )
        {
            enemyRestoreManaEvent ( amount );
        }
    }
    // laittaa tekemaan reapetillä damagee
    public void setDotToTarget ( GameObject target, float dotDamage, int duration, DamageType damageType )
    {
        StartCoroutine ( dotActivated ( target, dotDamage, duration, damageType ) );
    }

    IEnumerator dotActivated ( GameObject target, float dotDamage, int duration, DamageType damageType )
    {

        int counter = 0;
        while ( counter < duration )
        {

            TakeDamage ( target, dotDamage, damageType );
            counter++;
            yield return new WaitForSeconds ( 1 );
        }

    }

    /// <summary>
    /// Tekee vahinkoa pelaajaan
    /// </summary>
    /// <param name="player"></param>
    public void DealDamage ( GameObject target, float weaponDamage, DamageType damageType )
    {
        if ( target != null )
        {
            float calculatedDamage = enemyStats.attackDamage.Value + weaponDamage;

            if ( enemyDealDamageEvent != null )
            {
                enemyDealDamageEvent ( target, calculatedDamage, damageType, enemyStats.level );
            }
        }
    }

    /// <summary>
    /// Laskee tulevan vahingon lopullisen määrän
    /// </summary>
    /// <param name="dmg">Tuleva vahinko</param>
    public void TakeDamage ( GameObject target, float dmg, DamageType damageType )
    {
        Color color = Color.red;
        float calculatedDamage;

        if ( target == gameObject )
        {
            if ( damageType == DamageType.Raw )
            {
                color = Color.red;
                calculatedDamage = Mathf.Round ( dmg );

                if ( calculatedDamage <= 0 )
                {
                    calculatedDamage = 0;
                }
                else
                {
                    enemyStats.health.BaseValue -= calculatedDamage;
                }
            }
            else if ( damageType == DamageType.Poison )
            {
                color = Color.magenta;
                calculatedDamage = dmg * ( 1 - ( ( enemyStats.poisonResistance.Value ) / ( 20 * enemyStats.level + enemyStats.poisonResistance.Value) ) );
                Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                StartCoroutine ( OverTimeDamage ( calculatedDamage / 5f, 5f, 0.5f, 0.1f, color, damageType ) );
            }
            else if ( damageType == DamageType.Fire )
            {
                color = Color.red;
                calculatedDamage = dmg * ( 1 - ( ( enemyStats.fireResistance.Value ) / ( 20 * enemyStats.level + enemyStats.fireResistance.Value) ) );
                Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                StartCoroutine ( OverTimeDamage ( calculatedDamage / 2.5f, 2.5f, 0.1f, 0.01f, color, damageType ) );
            }
            else if ( damageType == DamageType.Cold )
            {
                color = Color.cyan;
                calculatedDamage = dmg * ( 1 - ( ( enemyStats.coldResistance.Value ) / ( 20 * enemyStats.level + enemyStats.coldResistance.Value ) ) );
                Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                enemyStats.health.BaseValue -= calculatedDamage;
                slowEffect = calculatedDamage * 0.1f;
                StartCoroutine ( RemoveSlow ( ) );
            }
            else if ( damageType == DamageType.Lightning )
            {
                color = Color.yellow;
                calculatedDamage = dmg * ( 1 - ( ( enemyStats.lightningResistance.Value ) / ( 20 * enemyStats.level + enemyStats.lightningResistance.Value ) ) );
                //armor lisää dmg enemmän crit?
                Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                enemyStats.health.BaseValue -= calculatedDamage;

            }
            else
            {
                calculatedDamage = dmg - enemyStats.armor.Value;
                calculatedDamage = Mathf.Round ( calculatedDamage );

                if ( calculatedDamage <= 0 )
                {
                    calculatedDamage = 0;
                }
                else
                {
                    enemyStats.health.BaseValue -= calculatedDamage;
                }
            }

            if ( enemyNotifyEvent != null )
            {
                enemyNotifyEvent ( head.transform, calculatedDamage.ToString ( ), color );
            }

            if ( enemyFlashEvent != null )
            {
                StartCoroutine ( enemyFlashEvent ( gameObject, 0.1f, color, true ) );
            }

            if ( enemyTakeDamageEvent != null )
            {
                enemyTakeDamageEvent ( gameObject, calculatedDamage, damageType );
            }

            if ( enemyStats.health.Value <= 0 )
            {
                Die ( );
            }
        }

    }

    public void PlayerIsDeadWeWin ( Transform pTransform )
    {
        aiActive = false; // lopettaa toiminnan kun pelaaja kuolee

    }

    public void Die ( )
    {
        if ( enemyDeathEvent != null )
        {
            enemyDeathEvent ( head.transform, enemyStats.xpReward, this );
        }

        if ( enemyStats.groundEnemyType == GroundEnemyType.Splitter && !hasSplit )
        {
            hasSplit = true;
            Vector2 leftSplitPos = new Vector2 ( transform.position.x + Random.Range ( 0f, -0.4f ), transform.position.y );
            Vector2 rightSplitPos = new Vector2 ( transform.position.x + Random.Range ( 0f, 0.4f ), transform.position.y );
            GameObject leftSplitted = Instantiate ( gameObject, leftSplitPos, Quaternion.identity );
            GameObject rightSplitted = Instantiate ( gameObject, rightSplitPos, Quaternion.identity );
            leftSplitted.GetComponent<StateController> ( ).hasSplit = true;
            rightSplitted.GetComponent<StateController> ( ).hasSplit = true;
            leftSplitted.transform.localScale *= 0.6f;
            rightSplitted.transform.localScale *= 0.6f;
            leftSplitted.GetComponent<StateController> ( ).dirRight = false;
            rightSplitted.GetComponent<StateController> ( ).dirRight = true;
            leftSplitted.GetComponent<StateController> ( ).chaseTarget = null;
            rightSplitted.GetComponent<StateController> ( ).chaseTarget = null;
            gameObject.SetActive ( false );
        }
        else
        {
            Debug.Log ( enemyStats.name + " IS DEAD" );
            //Instantiate death prefab
            Destroy ( gameObject );
        }

    }

    public void OnEnemyTakeDamage ( GameObject origin, float damage, DamageType damageType )
    {
        Collider2D [ ] cols = Physics2D.OverlapCircleAll ( transform.position, 3f, targetLayer );
        Debug.Log ( gameObject.name + " Hoksaa : Toinen vihu pulassa" );
        for ( int i = 0 ; i < cols.Length ; i++ )
        {
            if ( cols [ i ].gameObject.CompareTag ( "Player" ) )
            {
                Debug.Log ( cols [ i ].name + " Lähellä" );
                chaseTarget = cols [ i ].gameObject.transform;
                alertedByEvent = true;
                break;
            }
        }

        StartCoroutine ( AlertedByEventTime ( alertedByEventDuration ) );

    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere ( transform.position, senseArea );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere ( transform.position, circleCastRadius );

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere ( transform.position, radius );
    }

    public void EnemyFallToDeath ( )
    {
        if ( transform.position.y <= -fallToDeathHeight )
        {
            Die ( );
        }
    }

    #region Coroutines
    IEnumerator AttackCooldown ( )
    {
        yield return new WaitForSeconds ( enemyStats.attackSpeed.Value );
        attackRdy = true;
        attackBox.GetComponent<BoxCollider2D> ( ).enabled = false;
    }

    IEnumerator OverTimeDamage ( float dmg, float duration, float tickSpeed, float colorFlashSpeed, Color collor, DamageType damageType )
    {
        for ( int i = 0 ; i < duration ; i++ )
        {
            yield return new WaitForSeconds ( tickSpeed );

            enemyStats.health.BaseValue -= dmg;

            if ( enemyNotifyEvent != null )
            {
                enemyNotifyEvent ( head.transform, dmg.ToString ( ), collor );
            }
            if ( enemyFlashEvent != null )
            {
                StartCoroutine ( enemyFlashEvent ( gameObject, colorFlashSpeed, collor, false ) );
            }
            if ( enemyTakeDamageEvent != null )
            {
                enemyTakeDamageEvent ( gameObject, dmg , damageType );
            }

        }
    }

    IEnumerator EggCd ( )
    {
        yield return new WaitForSeconds ( 1f );
        notHatchedEgg = false;
    }

    IEnumerator AttackHitBoxDuration ( )
    {
        yield return new WaitForSeconds ( enemyStats.attackSpeed.Value );
        attackBox.GetComponent<BoxCollider2D> ( ).enabled = false;

    }

    IEnumerator AlertedByEventTime ( float time )
    {
        yield return new WaitForSeconds ( time );
        alertedByEvent = false;
        Debug.Log ( gameObject.name + " No longer alerted from event" );
    }

    public IEnumerator WaitTime ( float time )
    {
        yield return new WaitForSeconds ( time );
        hasTurn = false;
    }

    public IEnumerator TurnAfterTime ( float time, bool turnDir )
    {
        yield return new WaitForSeconds ( time );
        dirRight = turnDir;
    }

    IEnumerator RemoveSlow ( )
    {
        yield return new WaitForSeconds ( 5f );
        slowed = false;
    }
    #endregion
}
