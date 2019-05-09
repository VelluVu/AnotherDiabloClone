using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Kryz.CharacterStats;
using UnityEngine.EventSystems;

/// <summary>
/// Pelaaja Luokka
/// </summary>
[RequireComponent ( typeof ( PlayerClass ), typeof ( PlayerInput ), typeof ( PlayerMovement ) )]
public class Player : MonoBehaviour
{

    #region Player Delegates
    public delegate void PlayerDeathDelegate ( Transform transform );
    public static event PlayerDeathDelegate playerDeathEvent;

    public delegate void PlayerTakeDamageDelegate ( float damage );
    public static event PlayerTakeDamageDelegate playerTakeDamageEvent;

    public delegate IEnumerator PlayerFlashDelegate ( GameObject source, float time, Color color, bool isFlashSpam );
    public static event PlayerFlashDelegate playerFlashEvent;

    public delegate void PlayerDealDamageDelegate ( GameObject enemy, float damage, bool critical, DamageType damageType, bool skill, float skillDmgPercent );
    public static event PlayerDealDamageDelegate playerDealDamageEvent;

    public delegate void PlayerNotifyDelecate ( Transform transform, string message, Color color );
    public static event PlayerNotifyDelecate playerNotifyEvent;

    public delegate void PlayerHealHealthDelegate ( float amount, bool restoreAll );
    public static event PlayerHealHealthDelegate playerHealHealthEvent;

    public delegate void PlayerRestoreManaDelegate ( float amount, bool restoreAll );
    public static event PlayerRestoreManaDelegate playerRestoreManaEvent;

    public delegate void PlayerUseManaDelegate ( float amount );
    public static event PlayerUseManaDelegate playerUseManaEvent;

    public delegate void PlayerAliveDelegate ( );
    public static event PlayerAliveDelegate playerAliveEvent;

    public delegate void PlayerSoundDelegate ( AudioSource source, PlayerSoundType playerSoundType );
    public static event PlayerSoundDelegate PlayerSoundEvent;

    #endregion

    #region GameObjects
    public GameObject head;
    public GameObject rightHand;
    public GameObject leftHand;
    GameObject ToolTip;
    public GameObject levelupEffect;
    public GameObject dashEffect;
    public Transform dashmoveParticlePos;
    ParticleSystem dashmoveeff;
    #endregion

    #region EssentialComponents
    [HideInInspector] Animator heroAnim;
    [HideInInspector] Rigidbody2D playerRB;
    PlayerMovement movement;
    public PlayerClass stats; //pelaajan statit public , koska halutaan päästä käsiksi pelaajan kautta.
    public LevelUp levelUp; //Tähänkin halutaan varmaan päästä vaikuttaan ulkopuolelta
    public Transform feet;
    public LayerMask whatIsGround;
    public LayerMask groundRayLayer;
    public LayerMask enemyLayer;
    public WeaponPlaceHolder main;
    public WeaponPlaceHolder off;
    AudioSource audioSource;
    public AudioSource walkAudio;
    RaycastHit hit;
    Vector2 mousePos;
    Vector2 wallNormal;
    #endregion

    #region Booleans
    [Header ( "Action Booleans" )]
    bool isMainAttackRdy = true;
    bool isOffAttackRdy = true;
    public bool isSpellRdy = true;
    public bool readyToDash = true;
    public bool directionRight;
    public bool canTurn = true;
    public bool stunned;
    public bool canWallJump;
    public bool isBlocking;
    public bool isAir;
    public bool isDashing;
    public bool mouseOnUI;
    public bool hitsGround;
    public bool takeFallDamage;
    public bool goingDown;
    public bool invulnerable;
    public bool slowed;
    public bool isDead = false;
    public bool isAlive = true;
    public bool climbingLadder;
    public bool climbingRobe;
    public bool jump;
    public bool leaping;
    public bool inCombat;
    public bool weakness;
    public bool stopped;
    float weaknessAmount;

    [Header ( "Ability Unlocks" )]
    public bool isDoubleJumpLearned;
    public bool isWallJumpLearned;
    public bool isDashLearned;
    #endregion

    #region NewMovementVariables
    [Header ( "New Movement Variables, Calculates on start don't try tweak during runtime" )]
    [Range ( 0.1f, 10f )] [Tooltip ( "Jump maxheight, when jump button pressed" )] public float jumpHeightMax = 4f;
    [Range ( 0.1f, 10f )] [Tooltip ( "Jump minheight, when jump button pressed fast" )] public float jumpHeightMin = 1f;
    [Range ( 0.1f, 1f )] [Tooltip ( "Time to reach the jump apex, lakipiste" )] public float timeToJumpApex = .4f;
    [Range ( 0.1f, 10f )] [Tooltip ( "Movement Speed" )] public float newMoveSpeed = 3;
    [Range ( 0.1f, 10f )] [Tooltip ( "Movement Speed" )] public float ladderClimbSpeed = 1.5f;
    [Range ( 0.1f, 10f )] [Tooltip ( "Movement Speed" )] public float robeClimbSpeed = 1.5f;
    [Range ( 0.01f, 1f )] [Tooltip ( "Acceleration in the Air" )] public float airAccelerationTime = .2f;
    [Range ( 0.01f, 1f )] [Tooltip ( "Acceleration in the Ground" )] public float groundAccelerationTime = .1f;
    [Range ( 0.1f, 10f )] [Tooltip ( "The Player's velocity down when moving against the wall" )] public float wallSlideSpeedMax = 3;
    [Range ( 0.01f, 1f )] [Tooltip ( "Reaction time for player, when deciding how to jump off from the wall" )] public float wallStickTime = .25f;
    [Tooltip ( "If horizontal Input is against wall and press jump button. The player moves to this direction" )] public Vector2 wallJumpClimb;
    [Tooltip ( "If no horizontal Input and press jump button. The player moves to this direction" )] public Vector2 wallJumpOff;
    [Tooltip ( "If horizontal Input is away from the wall and press jump button. The player moves to this direction" )] public Vector2 wallLeap;
    float newGravity;
    float jumpVelocityMax;
    float jumpVelocityMin;
    float velocityXSmoothing;
    float timeToWallUnstick;
    float slowEffect;
    Vector2 newVelocity;
    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    #endregion

    #region publicVariables  
    [Header ( "Movement Variables" )]
    [Range ( 0.1f, 5f )] public float speedScale;
    [Range ( 0.1f, 5f )] public float dashTime;
    [Range ( 0.1f, 5f )] public float dashCooldown;
    [Range ( 0.1f, 10f )] public float dashSpeed;

    [Header ( "Jump variables" )]
    [Range ( 0.1f, 5f )] public float extraJumpScale;
    [Range ( 0.1f, 5f )] public float jumpScale;
    [Tooltip ( "Extra jumps after intial jump" )] public int extraJumps;

    [Header ( "Fall Damage Variables" )]
    [Range ( 0.1f, 5f )] public float fallToDeathTime;
    [Range ( 0.1f, 5f )] public float lowFallDamageTime;
    [Range ( 0.1f, 5f )] public float fallDamageMultiplier;

    [Range ( 1f, 1.2f )] public float swingSpeed;
    #endregion

    #region Extra Variables

    int jumpsCount;
    private float fallStartYPos;
    private float fallDamage;
    float fallTime;
    float fallDamageMultiplierBonus;
    float nextSwing;
    #endregion

    #region Unity Monobehavior Functions
    private void Awake ( )
    {
        movement = gameObject.GetComponent<PlayerMovement> ( );
        stats = gameObject.GetComponent<PlayerClass> ( );
        ToolTip = FindObjectOfType<ToolTip> ( ).gameObject;
        main = rightHand.GetComponent<WeaponPlaceHolder> ( );
        off = leftHand.GetComponent<WeaponPlaceHolder> ( );
        Time.timeScale = 1;
    }
    private void OnEnable ( )
    {
        LevelUp.levelUpEvent += OnLevelUp;
        StateController.enemyDealDamageEvent += TakeDamage;
        CheckPoint.checkPointEvent += RestoreCharacter;
        PlayerDeathUI.respawnPlayerEvent += Alive;
        Projectile.projectileHitEvent += TakeDamage;
    }
    private void OnDisable ( )
    {
        LevelUp.levelUpEvent -= OnLevelUp;
        StateController.enemyDealDamageEvent -= TakeDamage;
        CheckPoint.checkPointEvent -= RestoreCharacter;
        PlayerDeathUI.respawnPlayerEvent -= Alive;
        Projectile.projectileHitEvent -= TakeDamage;
    }
    private void Start ( )
    {
        playerRB = gameObject.GetComponent<Rigidbody2D> ( );
        heroAnim = gameObject.GetComponentInChildren<Animator> ( );
        audioSource = gameObject.GetComponent<AudioSource> ( );
        dashmoveeff = gameObject.GetComponentInChildren<ParticleSystem> ( );
        slowEffect = 1;
        weaknessAmount = 1;
        newGravity = -( 2 * jumpHeightMax ) / ( timeToJumpApex * timeToJumpApex );
        jumpVelocityMax = Mathf.Abs ( newGravity ) * timeToJumpApex;
        jumpVelocityMin = Mathf.Sqrt ( 2 * Mathf.Abs ( newGravity ) * jumpHeightMin );
        OpenInventory ( );
    }
    private void Update ( )
    {
        newGravity = -( 2 * jumpHeightMax ) / ( timeToJumpApex * timeToJumpApex );
        jumpVelocityMax = Mathf.Abs ( newGravity ) * timeToJumpApex;
        jumpVelocityMin = Mathf.Sqrt ( 2 * Mathf.Abs ( newGravity ) * jumpHeightMin );

        CalculateVelocity ( );
        HandleWallSliding ( );
        HandleLadderClimbing ( );
        HandleRobeClimb ( );
        HandleSlows ( );

        if ( canTurn )
        {
            UpdateDirection ( );
        }

        if ( !isDead && !isBlocking && !stopped)
        {
            if ( !stunned )
            {
                movement.Move ( new Vector2 ( newVelocity.x * speedScale * slowEffect, newVelocity.y * jumpScale * slowEffect ) * Time.deltaTime );

            }
            else
            {
                movement.Move ( new Vector2 ( 0, newVelocity.y * jumpScale * slowEffect ) * Time.deltaTime );
            }

        }

        if ( newVelocity.x <= 0.1f && newVelocity.x >= -0.1f )
        {
            TurnMousePointerDir ( );
        }

        if ( movement.collisions.above || movement.collisions.below )
        {
            newVelocity.y = 0;
        }

        Grounded ( );

        FallToDeathCheck ( );

        if ( !isDashing )
        {
            WalkAnimation ( );
        }

        if ( stats.health.Value <= 0 )
        {
            Die ( );
        }

        if ( !climbingLadder && !climbingRobe )
        {
            heroAnim.speed = 1;
        }
    }
    #endregion

    #region Input Functions
    public void SetDirectionalInput ( Vector2 input )
    { directionalInput = input; }
    public void OnJumpInputDown ( )
    {
        if ( stunned )
        {
            return;
        }
        if ( !isBlocking )
        {
            jump = true;

            if ( wallSliding )
            {
                if ( wallDirX == directionalInput.x )
                {
                    newVelocity.x = -wallDirX * wallJumpClimb.x;
                    newVelocity.y = wallJumpClimb.y;

                }
                else if ( directionalInput.x == 0 )
                {
                    newVelocity.x = -wallDirX * wallJumpOff.x;
                    newVelocity.y = wallJumpOff.y;

                }
                else
                {
                    newVelocity.x = -wallDirX * wallLeap.x;
                    newVelocity.y = wallLeap.y;

                }
            }
            if ( movement.collisions.below || !movement.collisions.below && jumpsCount >= 0 )
            {

                jumpsCount--;
                newVelocity.y = jumpVelocityMax;

                if ( PlayerSoundEvent != null )
                {
                    PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerJump );
                }
                heroAnim.SetTrigger ( "Jump" );

            }
            if ( ( climbingLadder || climbingRobe ) && jump )
            {
                newVelocity.y = jumpVelocityMax;

                if ( PlayerSoundEvent != null )
                {
                    PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerJump );
                }
                heroAnim.SetTrigger ( "Jump" );

            }
        }
    }
    public void OnJumpInputUp ( )
    {

        if ( newVelocity.y > jumpVelocityMin )
        {
            newVelocity.y = jumpVelocityMin;
        }

    }
    public void GetSecondMouseButton ( )
    {
        if ( stunned )
        {
            return;
        }
        if ( off.weaponType == WeaponType.Shield )
        {
            if ( !isAir )
            {
                isBlocking = true;
                heroAnim.SetBool ( "Block", true );
            }
        }
        else
        {

            if ( off.weaponType == WeaponType.MeleeWeapon )
            {




            }
            if ( off.weaponType == WeaponType.RangedWeapon )
            {

            }

        }
    }
    public void GetSecondMouseButtonUp ( )
    {
        Debug.Log ( "Released block" );
        isBlocking = false;
        heroAnim.SetBool ( "Block", false );
    }
    public void OpenInventory ( )
    {
        if ( PlayerInventory.instance.gameObject.activeSelf == true )
        {
            PlayerInventory.instance.gameObject.SetActive ( false );
            if ( ToolTip.activeSelf )
            {
                ToolTip.SetActive ( false );
            }
        }
        else
        {
            PlayerInventory.instance.gameObject.SetActive ( true );
        }
    }
    public void GetFirstMouseButton ( )
    {
        if ( stunned )
        {
            return;
        }

        if ( !isBlocking && !mouseOnUI && Time.time > nextSwing )
        {
            nextSwing = Time.time + PlayerClass.instance.baseAttackSpeed.Value * 0.85f;

            if ( main.weaponType == WeaponType.MeleeWeapon || main.weaponType == WeaponType.TwoHandedMeleeWeapon )
            {
                if ( PlayerSoundEvent != null )
                {
                    PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerAttack );
                }

                heroAnim.SetFloat ( "attackAnimSpeed", stats.baseAttackSpeed.Value );
                heroAnim.SetBool ( "Attack", true );
                //StartCoroutine ( MainAttackCD ( PlayerClass.instance.baseAttackSpeed.Value ) );

            }
            if ( main.weaponType == WeaponType.RangedWeapon || main.weaponType == WeaponType.TwoHandedRangedWeapon )
            {

                heroAnim.SetBool ( "Attack", true );
                heroAnim.SetFloat ( "attackAnimSpeed", stats.baseAttackSpeed.Value );

            }
        }
    }
    public void GetFirstMouseButtonUp ( )
    {

        heroAnim.SetBool ( "Attack", false );
    }
    public void DashInputDown ( )
    {

        if ( !isDashing && readyToDash && ( !movement.collisions.right || !movement.collisions.left ) )
        {
            canTurn = false;
            readyToDash = false;
            //Debug.Log ( "Started Dashing" );
            playerRB.gravityScale = 0;
            isDashing = true;
            Destroy ( Instantiate ( dashEffect, feet.transform.position, Quaternion.identity ), dashTime + 0.1f );

            if ( directionRight )
            {
                dashmoveParticlePos.transform.rotation = Quaternion.Euler ( 0f, 0f, 0f );
                dashmoveeff.Play ( );
            }
            else
            {
                dashmoveParticlePos.transform.rotation = Quaternion.Euler ( 0f, 180f, 0f );
                dashmoveeff.Play ( );
            }


            movement.dashing = true;
            if ( PlayerSoundEvent != null )
            {
                PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerDash );
            }
            StartCoroutine ( DashTime ( ) );
            StartCoroutine ( DashCoolDown ( ) );
        }
    }
    public void DashInputHold ( )
    {

        if ( movement.collisions.right || movement.collisions.left )
        {
            isDashing = false;
            movement.dashing = false;
            if ( !directionRight )
            {
                transform.position = new Vector2 ( transform.position.x + 0.1f, transform.position.y );
            }
            else
            {
                transform.position = new Vector2 ( transform.position.x - 0.1f, transform.position.y );
            }
            return;
        }

        if ( !isDashing )
        {
            heroAnim.SetBool ( "Dash", false );
            invulnerable = false;
            canTurn = true;
            movement.dashing = false;
            dashmoveeff.Stop ( );
        }

        if ( isDashing )
        {

            if ( directionRight )
            {
                dashmoveParticlePos.transform.rotation = Quaternion.Euler ( 0f, 0f, 0f );

            }
            else
            {
                dashmoveParticlePos.transform.rotation = Quaternion.Euler ( 0f, 180f, 0f );

            }

            //Debug.Log ( "Holding Dash Key" );
            //Debug.Log ( "IS DASHING" );
            //heroAnim.SetBool ( "Walk", false );
            heroAnim.SetBool ( "Dash", true );
            invulnerable = true;

            if ( directionRight && isDashing && !movement.collisions.right )
            {

                newVelocity = new Vector2 ( dashSpeed , 0 );
                //transform.Translate ( new Vector2 (  * Time.deltaTime, 0 ) );
            }
            else if ( !directionRight && isDashing && !movement.collisions.left )
            {

                newVelocity = new Vector2 ( -dashSpeed , 0 );
                // transform.Translate ( new Vector2 (  * Time.deltaTime, 0 ) );

            }
        }
    }
    public void DashInputUp ( )
    {
        //Debug.Log ( "Stopped Dashing" );
        heroAnim.SetBool ( "Dash", false );
        isDashing = false;
        canTurn = true;
        movement.dashing = false;
        invulnerable = false;
        dashmoveeff.Stop ( );
    }
    #endregion

    #region Utility Functions
    public void SetMouseOnUI ( bool isMouseOnUI )
    { mouseOnUI = isMouseOnUI; }
    public void RestoreCharacter ( )
    {
        OnRestoreHealth ( stats.maxHealth.Value, true );
        OnRestoreMana ( stats.maxMana.Value, true );
    }
    void Grounded ( )
    {
        if ( movement.collisions.below )
        {
            hitsGround = true;
            jumpsCount = extraJumps;
            isAir = false;
            jump = false;
        }
    }
    void HandleWallSliding ( )
    {
        wallDirX = ( movement.collisions.left ) ? -1 : 1;
        wallSliding = false;

        if ( ( movement.collisions.left || movement.collisions.right ) && !movement.collisions.below && newVelocity.y < 0 )
        {
            wallSliding = true;

            if ( newVelocity.y < -wallSlideSpeedMax )
            {
                newVelocity.y = -wallSlideSpeedMax;
            }

            if ( timeToWallUnstick > 0 )
            {
                velocityXSmoothing = 0;
                newVelocity.x = 0;

                if ( directionalInput.x != wallDirX && directionalInput.x != 0 )
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }
    void HandleLadderClimbing ( )
    {
        if ( climbingLadder )
        {
            heroAnim.speed = 0;
            heroAnim.SetBool ( "ClimbLadder", true );
            if ( directionalInput.y > 0 )
            {
                heroAnim.speed = 1;
                jump = false;
                newVelocity.y = ladderClimbSpeed * speedScale;
            }
            if ( directionalInput.y < 0 )
            {
                heroAnim.speed = 1;
                jump = false;
                newVelocity.y = -ladderClimbSpeed * speedScale;
            }

            if ( directionalInput.y == 0 && !jump )
            {
                newVelocity.y = 0;
            }
        }
        else
        {
            heroAnim.SetBool ( "ClimbLadder", false );
        }
    }
    void HandleRobeClimb ( )
    {
        if ( climbingRobe )
        {
            heroAnim.SetBool ( "RobeClimb", true );
            if ( !jump && directionalInput.y == 0 && newVelocity.x != 0 )
            {
                newVelocity.y = 0;
            }
        }
        else
        {
            heroAnim.SetBool ( "RobeClimb", false );
        }

    }
    void HandleSlows ( )
    {
        if ( !slowed )
        {
            slowEffect = 1;
        }
        if ( slowed )
        {
            StartCoroutine ( RemoveSlow ( ) );
        }
    }
    private void ClimbRobe ( Collider2D collision )
    {

        climbingRobe = true;

        if ( climbingRobe )
        {
            heroAnim.speed = 0;
            if ( !jump )
            {

                newVelocity = collision.attachedRigidbody.velocity;

            }
            if ( directionalInput.x > 0 && !jump && climbingRobe && collision.attachedRigidbody.velocity.x > 0 || collision.attachedRigidbody.velocity.x == 0 )
            {
                collision.attachedRigidbody.velocity = new Vector2 ( newVelocity.x * swingSpeed, collision.attachedRigidbody.velocity.y );

            }
            if ( directionalInput.x < 0 && !jump && climbingRobe && collision.attachedRigidbody.velocity.x < 0 || collision.attachedRigidbody.velocity.x == 0 )
            {
                collision.attachedRigidbody.velocity = new Vector2 ( newVelocity.x * swingSpeed, collision.attachedRigidbody.velocity.y );

            }

            if ( directionalInput.y > 0 )
            {
                heroAnim.speed = 1;
                jump = false;
                newVelocity.y = robeClimbSpeed * speedScale;
                newVelocity.x = collision.attachedRigidbody.velocity.x;
            }

            if ( directionalInput.y < 0 )
            {
                heroAnim.speed = 1;
                jump = false;
                newVelocity.y = -robeClimbSpeed * speedScale * 0.5f;
                newVelocity.x = collision.attachedRigidbody.velocity.x;

            }

        }
    }
    private void ClimbLadder ( )
    {
        climbingLadder = true;
    }
    void CalculateVelocity ( )
    {
        float targetVelocityX = directionalInput.x * newMoveSpeed;
        newVelocity.x = Mathf.SmoothDamp ( newVelocity.x, targetVelocityX, ref velocityXSmoothing, ( movement.collisions.below ) ? groundAccelerationTime : airAccelerationTime );
        newVelocity.y += newGravity * Time.deltaTime;
    }
    void TurnMousePointerDir ( )
    {

        if ( !canTurn )
        {
            return;
        }

        MousePosition ( );

        if ( playerRB.velocity.x == 0 )
        {
            if ( mousePos.x > transform.position.x )
            {
                transform.localScale = new Vector3 ( 1, 1, 1 );
                directionRight = true;
            }
            if ( mousePos.x < transform.position.x )
            {
                transform.localScale = new Vector3 ( -1, 1, 1 );
                directionRight = false;
            }
        }
    }
    void MousePosition ( )
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint ( mousePos );
    }
    void UpdateDirection ( )
    {
        if ( !canTurn )
        {
            return;
        }

        if ( movement.collisions.faceDirection < 0 )
        {
            transform.localScale = new Vector3 ( -1, 1, 1 );
            directionRight = false;
        }
        if ( movement.collisions.faceDirection > 0 )
        {
            transform.localScale = new Vector3 ( 1, 1, 1 );
            directionRight = true;
        }
    }
    public void SetStun ( float time )
    {
        stunned = true;
        StartCoroutine ( StunDuration ( time ) );
    }
    public void CanTurn ( float time )
    {
        canTurn = false;
        StartCoroutine ( BlockTurning ( time ) );
    }
    public void ApplyWeakness ( float percent, float time )
    {
        weakness = true;
        weaknessAmount += percent * 0.01f;
        StartCoroutine ( WeaknessDuration ( time ) );
    }
    #endregion

    #region Animation Functions
    /// <summary>
    /// Animates player walk
    /// </summary>
    void WalkAnimation ( )
    {
        if ( stunned )
        {
            heroAnim.SetBool("Walk",false);
            return;
        }

        //Askeleiden nopeus, en vielä tiedä miten säädän kohilleen, joko soundplayer tai corouteeni
        if ( heroAnim.GetBool ( "Walk" ) )
        {
            if ( PlayerSoundEvent != null && !walkAudio.isPlaying )
            {
                PlayerSoundEvent ( walkAudio, PlayerSoundType.PlayerFootSteps );
                walkAudio.Play ( );
            }
        }

        if ( !heroAnim.GetBool ( "Walk" ) || isDashing || heroAnim.GetBool ( "Attack" ) || stunned || isBlocking )
        {
            walkAudio.Stop ( );
        }


        if ( newVelocity.x >= 0.05f && movement.collisions.below )
        {
            heroAnim.SetBool ( "Walk", true );
            directionRight = true;

        }
        else if ( newVelocity.x <= -0.05f && movement.collisions.below )
        {
            heroAnim.SetBool ( "Walk", true );
            directionRight = false;

        }
        else
        {
            heroAnim.SetBool ( "Walk", false );

        }
    }
    #endregion

    #region Action/Event Functions
    /// <summary>
    /// Osuessa viholliseen suorittaa tarvittavat calculaatiot
    /// </summary>
    /// <param name="target"></param>
    /// <param name="weaponPower"></param>
    public void DealDamage ( GameObject target, float weaponPower, DamageType damageType, bool skill, float skillDmgPercent )
    {
        if ( target != null )
        {
            float calculatedDamage = Random.Range ( ( int ) stats.baseDamage.Value, ( int ) stats.baseDamageMax.Value );

            if ( CheckForCrit ( ) )
            {
                calculatedDamage *= ( stats.criticalHitDamage.Value / 100 );

                if ( playerDealDamageEvent != null )
                {
                    playerDealDamageEvent ( target, calculatedDamage, true, damageType, skill, skillDmgPercent );
                }

                if ( PlayerSoundEvent != null )
                {
                    PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerHit );
                }

                Debug.Log ( "CriticalHit: " + ( stats.criticalHitDamage.Value / 100 ) );
            }
            else
            {
                if ( playerDealDamageEvent != null )
                {
                    playerDealDamageEvent ( target, calculatedDamage, false, damageType, skill, skillDmgPercent );
                }

                if ( PlayerSoundEvent != null )
                {
                    PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerHit );
                }
            }

            inCombat = true;
            StartCoroutine ( RemoveCombat ( ) );
        }
    }
    public bool CheckForCrit ( )
    {
        float randomValue = Random.value;
        if ( randomValue <= stats.criticalHitChance.Value / 100 )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Lisää pelaajalle hp
    /// </summary>
    /// <param name="amount"></param>
    public void OnRestoreHealth ( float amount, bool restoreAll )
    {

        if ( playerFlashEvent != null )
        {
            StartCoroutine ( playerFlashEvent ( gameObject, 0.3f, Color.green, false ) );
        }

        if ( playerNotifyEvent != null )
        {
            playerNotifyEvent ( head.transform, amount.ToString ( ), Color.green );
        }

        if ( restoreAll )
        {
            stats.FullyRestoreHealth ( );
        }
        else
        {
            stats.RestoreHealth ( amount );
        }

        if ( PlayerSoundEvent != null )
        {
            PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerRestoreHealth );
        }

        if ( playerHealHealthEvent != null )
        {
            playerHealHealthEvent ( amount, restoreAll );
        }
    }

    /// <summary>
    /// Lisää pelaajalle hp
    /// </summary>
    /// <param name="amount"></param>
    public void OnRestoreMana ( float amount, bool restoreAll )
    {

        if ( playerFlashEvent != null )
        {
            StartCoroutine ( playerFlashEvent ( gameObject, 0.3f, Color.blue, false ) );
        }

        if ( playerNotifyEvent != null )
        {
            playerNotifyEvent ( head.transform, Mathf.Round ( amount ).ToString ( ), Color.blue );
        }

        if ( restoreAll )
        {
            stats.FullyRestoreMana ( );
        }
        else
        {
            stats.RestoreMana ( amount );
        }

        if ( PlayerSoundEvent != null )
        {
            PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerRestoreMana );
        }

        if ( playerRestoreManaEvent != null )
        {
            playerRestoreManaEvent ( amount, restoreAll );
        }
    }

    /// <summary>
    /// Käyttää manaa taikoihin, voi aiheuttaa myös muita vaikutuksia
    /// </summary>
    /// <param name="amount"></param>
    public void OnManaUse ( float amount )
    {
        stats.mana.BaseValue -= amount;

        if ( playerUseManaEvent != null )
        {
            playerUseManaEvent ( amount );
        }
    }

    /// <summary>
    /// Player calculates defenses and how much damage is taken
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage ( GameObject target, float dmg, DamageType damageType, int level )
    {
        float calculatedDamage;
        Color color;
        //Debug.Log ( dmg );
        //Debug.Log ( damageType );

        if ( float.IsNaN ( dmg ) || invulnerable )
        {
            return;
        }
        float fullResistance = stats.allResistance.Value;
        fullResistance += PlayerClass.instance.damageTypes [ damageType ].Value;

        if ( isBlocking )
        {

            calculatedDamage = dmg * ( 1 - ( fullResistance / ( 20 * level + fullResistance ) ) );
            calculatedDamage = dmg * ( 1 - ( stats.block.Value / ( 5 * level + stats.block.Value ) ) );
            calculatedDamage = dmg * ( 1 - ( stats.armor.Value / ( 50 * level + stats.armor.Value ) ) );
            calculatedDamage = Mathf.Round ( calculatedDamage * weaknessAmount );

            if ( PlayerSoundEvent != null )
            {
                PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerBlock );
            }

            //Ei saa mennä negatiiviseksi
            if ( calculatedDamage < 0 )
            {
                calculatedDamage = 0;
            }

            color = Color.grey;
            stats.health.BaseValue -= calculatedDamage;

        }
        else
        {
            if ( damageType == DamageType.Raw )
            {
                color = Color.red;
                calculatedDamage = dmg;
                calculatedDamage -= stats.fallDamageReduction.Value;
                calculatedDamage = Mathf.Round ( calculatedDamage * weaknessAmount );
                stats.health.BaseValue -= calculatedDamage;
            }
            else if ( damageType == DamageType.Poison )
            {
                color = Color.magenta;
                calculatedDamage = dmg * ( 1 - ( ( fullResistance ) / ( 20 * level + fullResistance ) ) );
                //Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage * weaknessAmount );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                StartCoroutine ( OverTimeDamage ( calculatedDamage / 5f, 5f, 0.5f, 0.1f, color ) );
            }
            else if ( damageType == DamageType.Fire )
            {
                color = Color.red;
                calculatedDamage = dmg * ( 1 - ( ( fullResistance ) / ( 20 * level + fullResistance ) ) );
                //Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage * weaknessAmount );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                StartCoroutine ( OverTimeDamage ( calculatedDamage / 2.5f, 2.5f, 0.1f, 0.01f, color ) );
            }
            else if ( damageType == DamageType.Cold )
            {
                color = Color.cyan;
                calculatedDamage = dmg * ( 1 - ( ( fullResistance ) / ( 20 * level + fullResistance ) ) );
                //Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage * weaknessAmount );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                stats.health.BaseValue -= calculatedDamage;
                slowEffect = calculatedDamage * 0.1f;
                StartCoroutine ( RemoveSlow ( ) );
            }
            else if ( damageType == DamageType.Lightning )
            {
                color = Color.yellow;
                calculatedDamage = dmg * ( 1 - ( ( fullResistance ) / ( 20 * level + fullResistance ) ) );
                //armor lisää dmg enemmän crit?
                //Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage * weaknessAmount );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                stats.health.BaseValue -= calculatedDamage;

            }
            else
            {
                color = Color.red;
                calculatedDamage = dmg * ( 1 - ( fullResistance / ( 20 * level + fullResistance ) ) );
                calculatedDamage = dmg * ( 1 - ( stats.armor.Value / ( 50 * level + stats.armor.Value ) ) );
                //Debug.Log ( ( stats.armor.Value / ( 50 * level + stats.armor.Value ) ) );
                //Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage * weaknessAmount );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                stats.health.BaseValue -= calculatedDamage;
            }
        }
        inCombat = true;
        StartCoroutine ( RemoveCombat ( ) );

        if ( calculatedDamage <= 0 )
        {

            if ( playerFlashEvent != null )
            {
                StartCoroutine ( playerFlashEvent ( gameObject, 0.1f, Color.grey, true ) );
            }

            return;
        }

        if ( playerNotifyEvent != null )
        {
            playerNotifyEvent ( head.transform, calculatedDamage.ToString ( ), color );
        }
        if ( playerFlashEvent != null )
        {
            StartCoroutine ( playerFlashEvent ( gameObject, 0.1f, color, true ) );
        }
        if ( playerTakeDamageEvent != null )
        {
            playerTakeDamageEvent ( calculatedDamage );
        }
        if ( PlayerSoundEvent != null && !isBlocking )
        {
            PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerTakeDamage );
        }
    }

    /// <summary>
    /// Levelup hetkellä tapahtuvat jutut
    /// </summary>
    public void OnLevelUp ( )
    {

        //esim partikkeli efekti
        Destroy ( Instantiate ( levelupEffect, transform ), 1f );
        //ääni
        //OnRestoreHealth ( stats.maxHealth.Value, true );
        //OnRestoreMana ( stats.maxHealth.Value, true );

        if ( playerNotifyEvent != null )
        {
            playerNotifyEvent ( head.transform, "Level Up " + stats.playerLevel.Value, Color.cyan );
        }
        if ( playerFlashEvent != null )
        {
            StartCoroutine ( playerFlashEvent ( gameObject, 0.5f, Color.cyan, true ) );
        }

        if ( PlayerSoundEvent != null )
        {
            PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerLevelUp );
        }

    }

    /// <summary>
    /// Laskee falldamagen ja kuolettavan tippumisen ajan
    /// </summary>
    public void FallToDeathCheck ( )
    {
        if ( newVelocity.y > 0 || movement.collisions.below || movement.collisions.left || movement.collisions.right )
        {
            //Debug.Log ( "IS NOT GOING DOWN" );
            goingDown = false;

        }

        if ( !goingDown )
        {
            fallTime = 0;
            fallDamageMultiplierBonus = 0.1f;
        }

        if ( newVelocity.y > 0 )
        {
            takeFallDamage = false;
            hitsGround = false;
        }

        if ( newVelocity.y > 0 || !movement.collisions.below )
        {
            isAir = true;
        }

        if ( !goingDown && newVelocity.y < 0 && !movement.collisions.below )
        {
            //Debug.Log ( "IS GOING DOWN !!!" );
            goingDown = true;
            fallStartYPos = transform.position.y;
        }

        if ( goingDown )
        {
            //Debug.Log ( "IS GOING DOWN" );
            fallTime += Time.deltaTime;
            fallDamageMultiplierBonus += fallTime;
        }

        if ( isDashing || wallSliding || isDead || climbingLadder || climbingRobe || leaping )
        {
            fallTime = 0;
            fallDamageMultiplierBonus = 0.1f;
            takeFallDamage = false;
        }

        //Debug.Log ( fallTime );
        if ( fallTime > lowFallDamageTime )
        {
            takeFallDamage = true;
        }

        if ( newVelocity.y == 0 )
        {
            hitsGround = true;
        }

        if ( takeFallDamage && movement.collisions.hitsGround && movement.collisions.below )
        {
            if ( fallDamageMultiplierBonus < 0.1f )
            {
                fallDamageMultiplierBonus = 0.1f;
            }
            //Debug.Log ( fallStartYPos );
            fallDamage = Mathf.Sqrt ( playerRB.mass * 2 * Mathf.Abs ( Physics2D.gravity.y * ( fallStartYPos - transform.position.y ) ) * ( fallDamageMultiplier + fallDamageMultiplierBonus ) );
            TakeDamage ( gameObject, fallDamage, DamageType.Raw, 1 );
            takeFallDamage = false;
            hitsGround = false;

            //Debug.Log ( Mathf.Sqrt ( playerRB.mass * 2 * Mathf.Abs ( Physics2D.gravity.y  * (fallStartYPos - transform.position.y) ) * ( fallDamageMultiplier + fallDamageMultiplierBonus ) ) );
            //Debug.Log ( fallDamage );
            //Debug.Log ( Physics2D.gravity.y );
            //Debug.Log ( fallStartYPos );
            //Debug.Log ( transform.position.y );
            //Debug.Log ( fallDamageMultiplier );
            //Debug.Log ( fallDamageMultiplierBonus );

        }

        if ( fallTime > fallToDeathTime )
        {
            //Debug.Log ( "FELL TO DEATH" );
            fallTime = 0;

            if ( PlayerSoundEvent != null )
            {
                PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerFallToDeath );
            }

            Die ( );
        }
    }

    /// <summary>
    /// Respawnatessa tapahtuvat jutut
    /// </summary>
    /// <param name="respawnPos"></param>
    public void Alive ( Transform respawnPos )
    {
        if ( respawnPos == null )
        {
            transform.position = new Vector3 ( 0, 0, 0 );
        }
        else
        {
            transform.position = respawnPos.position;
        }
        isDead = false;
        isAlive = true;
        OnRestoreHealth ( stats.maxHealth.Value, true );
        OnRestoreMana ( stats.maxMana.Value, true );
        invulnerable = true;
        StartCoroutine ( Invulnerable ( 1f ) );

        if ( playerAliveEvent != null )
        {
            playerAliveEvent ( );
        }
    }

    /// <summary>
    /// Kuollessa tapahtuvat jutut
    /// </summary>
    public void Die ( )
    {
        if ( !isDead )
        {
            isAlive = false;
            isDead = true;
            OnRestoreHealth ( -stats.maxHealth.Value, false );
            OnRestoreMana ( -stats.maxMana.Value, false );

            if ( playerDeathEvent != null )
            {
                playerDeathEvent ( transform );
            }

            if ( PlayerSoundEvent != null )
            {
                PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerDeath );
            }

            //Debug.Log ( "I'm Dead" );
        }
    }

    public void FeelSlow ( float newSlowMult )
    {
        if ( slowEffect >= newSlowMult )
        {
            slowed = true;
            slowEffect = newSlowMult;
        }

        if ( slowEffect <= 0.1f )
        {
            slowed = true;
            slowEffect = 0.1f;
        }

    }
    public void SetLeaping ( float leapingDuration )
    {
        leaping = true;
        StartCoroutine ( LeapDuration ( leapingDuration ) );
    }
    public void SetStopped ( float stopDuration )
    {
        stopped = true;
        StartCoroutine ( StopDuration ( stopDuration ) );
    }
    #endregion

    #region Unity Collisions
    private void OnCollisionEnter2D ( Collision2D collision )
    {

        //Tämä voisi olla myös collision stayssa ehkä, jos on buginen niin siirtää sinne <antaa dashaa vihujen lävitse>
        if ( isDashing && collision.gameObject.CompareTag ( "Enemy" ) )
        {
            Physics2D.IgnoreCollision ( gameObject.GetComponent<BoxCollider2D> ( ), collision.collider, true );
        }

        if ( !isDashing && !isBlocking && collision.gameObject.CompareTag ( "Enemy" ) && !collision.gameObject.GetComponent<Projectile> ( ) )
        {
            Vector2 normalV = collision.GetContact ( 0 ).normal;
            //Debug.Log ( "normal " + normalV );
            //Vector2 reflection = Vector2.Reflect ( collision.GetContact ( 0 ).point, normalV );
            //Debug.Log ( "KnockBack " + normalV * jumpVelocityMax );

            if ( normalV.x == 0 )
            {
                if ( ( transform.position.x - collision.transform.position.x ) >= 0 )
                {
                    normalV.x = 1 * jumpHeightMax;
                    //Debug.Log ( 1 * jumpHeightMax );
                }
                else if ( ( transform.position.x - collision.transform.position.x ) < 0 )
                {
                    normalV.x = -1 * jumpHeightMax;
                    //Debug.Log ( -1 * jumpHeightMax );
                }
            }
            newVelocity = jumpVelocityMax * new Vector2 ( normalV.x * 0.5f, normalV.y * 0.5f );
            TakeDamage ( gameObject, 10f, DamageType.Raw, ( int ) stats.playerLevel.Value );
        }
    }
    private void OnCollisionExit2D ( Collision2D collision )
    {
        if ( !isDashing && collision.gameObject.CompareTag ( "Enemy" ) )
        {
            Physics2D.IgnoreCollision ( gameObject.GetComponent<BoxCollider2D> ( ), collision.collider, false );
        }


    }
    private void OnTriggerStay2D ( Collider2D collision )
    {
        if ( collision.gameObject.layer == LayerMask.NameToLayer ( "Ladder" ) )
        {

            ClimbLadder ( );

        }
        if ( collision.gameObject.layer == LayerMask.NameToLayer ( "Rope" ) )
        {

            ClimbRobe ( collision );

        }

        if ( collision.gameObject.CompareTag ( "Web" ) )
        {
            slowed = true;
            slowEffect = collision.gameObject.GetComponent<SpiderWebSpot> ( ).GetSlowPercent ( );
        }
    }
    private void OnTriggerExit2D ( Collider2D collision )
    {
        if ( collision.gameObject.layer == LayerMask.NameToLayer ( "Ladder" ) )
        {
            climbingLadder = false;
        }
        if ( collision.gameObject.layer == LayerMask.NameToLayer ( "Rope" ) )
        {
            climbingRobe = false;
        }
        if ( collision.gameObject.CompareTag ( "Web" ) )
        {
            slowed = false;
        }

    }
    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if ( collision.gameObject.layer == LayerMask.NameToLayer ( "Ladder" ) )
        {
            jump = false;
        }
    }
    #endregion

    #region Coroutines


    IEnumerator LeapDuration ( float duration )
    {
        yield return new WaitForSeconds ( duration );
        leaping = false;
    }
    IEnumerator StunDuration ( float time )
    {

        yield return new WaitForSeconds ( time );
        stunned = false;
    }
    IEnumerator BlockTurning ( float time )
    {
        yield return new WaitForSeconds ( time );
        canTurn = true;
    }
    IEnumerator WeaknessDuration ( float time )
    {
        yield return new WaitForSeconds ( time );
        weakness = false;
        weaknessAmount = 1;
    }
    IEnumerator RemoveCombat ( )
    {
        yield return new WaitForSeconds ( 8f );
        inCombat = false;
    }
    IEnumerator StopDuration ( float duration )
    {
        yield return new WaitForSeconds ( duration );
        stopped = false;
    }

    IEnumerator OverTimeDamage ( float dmg, float duration, float tickSpeed, float colorFlashSpeed, Color collor )
    {
        for ( int i = 0 ; i < duration ; i++ )
        {
            yield return new WaitForSeconds ( tickSpeed );

            stats.health.BaseValue -= dmg;

            if ( playerNotifyEvent != null )
            {
                playerNotifyEvent ( head.transform, dmg.ToString ( ), collor );
            }
            if ( playerFlashEvent != null )
            {
                StartCoroutine ( playerFlashEvent ( gameObject, colorFlashSpeed, collor, false ) );
            }
            if ( playerTakeDamageEvent != null )
            {
                playerTakeDamageEvent ( dmg );
            }

        }
    }

    IEnumerator Attack ( )
    {

        yield return new WaitForSeconds ( heroAnim.GetCurrentAnimatorStateInfo ( 0 ).length );
        //yield return new WaitForSeconds ( stats.baseAttackSpeed.Value );
        main.GetComponent<BoxCollider2D> ( ).enabled = false;

    }
    IEnumerator MainAttackCD ( float cd )
    {
        isMainAttackRdy = false;
        yield return new WaitForSeconds ( cd );
        isMainAttackRdy = true;
    }
    IEnumerator OffAttackCD ( )
    {

        yield return new WaitForSeconds ( PlayerClass.instance.baseAttackSpeed.Value );
        isOffAttackRdy = true;

    }

    IEnumerator DashTime ( )
    {
        yield return new WaitForSeconds ( dashTime );
        isDashing = false;
        //Debug.Log ( "Stopped Dashing" );
    }

    IEnumerator DashCoolDown ( )
    {
        yield return new WaitForSeconds ( dashCooldown );
        readyToDash = true;
    }

    IEnumerator SpellCooldown ( float cd )
    {
        yield return new WaitForSeconds ( cd );
        isSpellRdy = true;
    }

    IEnumerator DelayEvent ( float delayTime, bool delayRdy )
    {
        yield return new WaitForSeconds ( delayTime );
        delayRdy = true;

    }

    IEnumerator Invulnerable ( float time )
    {
        yield return new WaitForSeconds ( time );
        invulnerable = false;
    }

    IEnumerator RemoveSlow ( )
    {
        yield return new WaitForSeconds ( 5f );
        slowed = false;
    }

    IEnumerator WalkStepInterval ( )
    {

        yield return new WaitForSeconds ( newMoveSpeed * 0.1f );
        if ( PlayerSoundEvent != null )
        {
            PlayerSoundEvent ( audioSource, PlayerSoundType.PlayerFootSteps );
        }

    }

    #endregion

    #region COMMENTEDOUT FUNCTIONS

    /// <summary>
    /// Pelaajan liike X axelilla
    /// </summary>
    //void PlayerMovement ( )
    //{
    //    if ( !isWallJumping )
    //    {
    //        //Turning
    //        if ( moveX > 0.2f )
    //        {
    //            directionRight = true;
    //            //wallJumpDirection = -1;
    //            transform.eulerAngles = rightDir;
    //        }
    //        else if ( moveX < -0.2f )
    //        {
    //            directionRight = false;
    //            //wallJumpDirection = 1;
    //            transform.eulerAngles = leftDir;
    //        }

    //        //lisää air multiplierin speedscalen sijaan

    //        if ( isAir )
    //        {
    //            playerRB.velocity = new Vector2 ( ( moveX * stats.moveSpeed.Value ) * airSpeed * Time.fixedDeltaTime, playerRB.velocity.y );
    //        }
    //        else
    //        {
    //            playerRB.velocity = new Vector2 ( ( moveX * stats.moveSpeed.Value ) * speedScale * Time.fixedDeltaTime, playerRB.velocity.y );
    //        }
    //    }

    //}

    /// <summary>
    /// Pelaajan Hyppy, tuplahyppy ja seinähyppy
    /// </summary>
    //void PlayerJump ( )
    //{

    //    if ( isWallJumping )
    //    {
    //        playerRB.gravityScale = 0.5f;
    //        goingDown = false;
    //    }
    //    else
    //    {
    //        playerRB.gravityScale = 1f;
    //    }

    //    if ( Input.GetButtonDown ( "Jump" ) && !initialJump && isGrounded )
    //    {
    //        //Debug.Log ( "InitJump" );
    //        initialJump = true;
    //    }
    //    //MarioJump
    //    if ( Input.GetButton ( "Jump" ) && !isJumping && initialJump && !isDoubleJumping )
    //    {
    //        //Debug.Log ( "jumping" );
    //        //heroAnim.SetTrigger ( "Jump" );
    //        isJumping = true;
    //        isAir = true;
    //        playerRB.AddForce ( Vector2.up * stats.jumpForce.Value * jumpScale );

    //    }

    //    //Debug.Log ( playerRB.velocity.y );

    //    //DoubleJump         
    //    if ( Input.GetButtonDown ( "Jump" ) && jumpsCount > 0 && !canWallJump && isDoubleJumpLearned && isJumping && playerRB.velocity.y < 1.0f && !initialJump && !isGrounded )
    //    {
    //        //Debug.Log ( "DoubleJump" );
    //        isDoubleJumping = true;
    //        isWallJumping = false;
    //        jumpsCount--;
    //        playerRB.AddForce ( Vector2.up * stats.extraJumpForce.Value * extraJumpScale, ForceMode2D.Impulse );
    //    }
    //    else if ( Input.GetButtonDown ( "Jump" ) && jumpsCount > 0 && !canWallJump && isDoubleJumpLearned && isJumping && playerRB.velocity.y >= 1.0f && !initialJump && !isGrounded )
    //    {
    //        //Debug.Log ( "Limiting the jump" );
    //        isDoubleJumping = true;
    //        isWallJumping = false;
    //        jumpsCount--;
    //        playerRB.AddForce ( Vector2.up * stats.extraJumpForce.Value * extraJumpScale * 0.5f, ForceMode2D.Impulse );
    //    }

    //    //WallJump
    //    if ( Input.GetButtonDown ( "Jump" ) && canWallJump && isWallJumpLearned && isJumping && !initialJump && !isGrounded )
    //    {

    //        isWallJumping = true;

    //        //playerRB.velocity = Vector2.up * 0.2f * wallNormal * wallJumpPushBack * stats.extraWallJumpForce.Value * wallJumpScale * Time.deltaTime;
    //        if ( playerRB.velocity.x > 0 )
    //        {
    //            Dir ( true );
    //        }
    //        else
    //        {
    //            Dir ( false );
    //        }

    //        playerRB.AddForce ( ( Vector2.up * wallJumpVerticalVectorMultipier + wallNormal ) * wallJumpPushBack * stats.extraWallJumpForce.Value * wallJumpScale, ForceMode2D.Impulse );

    //    }

    //    if ( Input.GetButtonUp ( "Jump" ) )
    //    {
    //        canWallJump = false;

    //    }

    //    //Better Gravity in jump
    //    if ( playerRB.velocity.y < 0 )
    //    {
    //        playerRB.velocity += Vector2.up * Physics2D.gravity.y * ( fallMultiplier - 1 ) * Time.deltaTime;
    //    }
    //    else if ( !Input.GetButton ( "Jump" ) && playerRB.velocity.y > 0 )
    //    {
    //        //Debug.Log ( "Input false" );
    //        initialJump = false;
    //        //isJumping = false;
    //        playerRB.velocity += Vector2.up * Physics2D.gravity.y * ( lowJumpMultiplier - 1 ) * Time.deltaTime;
    //    }

    //}

    /// <summary>
    /// Alternate ground check
    /// </summary>
    //private void GroundCheck ( )
    //{
    //    //layermask tapa tsekkaa grounded testi
    //    isGrounded = Physics2D.OverlapCircle ( feet.position, groundCheckRadius, whatIsGround );

    //    if ( isGrounded )
    //    {
    //        fallTime = 0;
    //        isDoubleJumping = false;
    //        canWallJump = false;
    //        isJumping = false;
    //        isAir = false;
    //        isWallJumping = false;
    //        hitsGround = true;
    //        jumpsCount = extraJumps;
    //    }
    //    else
    //    {
    //        hitsGround = false;
    //        isAir = true;
    //    }
    //}

    ///// <summary>
    ///// Inputit ActionBaareille
    ///// </summary>
    //void CastSpell ( )
    //{
    //    //// Testailua varten
    //    if ( Input.GetButton ( "ActionBar1" ) && stats.mana.Value >= 0.1f )
    //    {
    //        //ChannelSpell
    //        //Tee taika            
    //        OnManaUse ( 0.1f );
    //    }
    //    else if ( Input.GetButton ( "ActionBar2" ) )
    //    {
    //        if ( isSpellRdy && stats.mana.Value >= 10f )
    //        {
    //            isSpellRdy = false;
    //            OnManaUse ( 10f );
    //            StartCoroutine ( SpellCooldown ( 1f ) );
    //        }
    //    }
    //    else if ( Input.GetButton ( "ActionBar3" ) )
    //    {
    //        if ( isSpellRdy && stats.mana.Value >= 15f )
    //        {
    //            isSpellRdy = false;
    //            OnManaUse ( 15f );
    //            StartCoroutine ( SpellCooldown ( 2f ) );
    //        }
    //    }
    //    else if ( Input.GetButton ( "ActionBar4" ) )
    //    {
    //        if ( isSpellRdy && stats.mana.Value >= 50f )
    //        {
    //            isSpellRdy = false;
    //            OnManaUse ( 50f );
    //            StartCoroutine ( SpellCooldown ( 5f ) );
    //        }
    //    }
    //    else if ( Input.GetButton ( "ActionBar5" ) )
    //    {
    //        if ( isSpellRdy && stats.mana.Value >= 100f )
    //        {
    //            isSpellRdy = false;
    //            OnManaUse ( 100f );
    //            StartCoroutine ( SpellCooldown ( 8f ) );
    //        }
    //    }
    //}
    #endregion
}
