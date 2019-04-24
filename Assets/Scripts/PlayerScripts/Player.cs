using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Kryz.CharacterStats;
using UnityEngine.EventSystems;

/// <summary>
/// Pelaaja Prototyyppi
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

    public delegate void PlayerDealDamageDelegate ( GameObject enemy, float damage, DamageType damageType );
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

    #endregion

    #region GameObjects
    public GameObject head;
    public GameObject rightHand;
    public GameObject leftHand;
    GameObject ToolTip;
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
    RaycastHit hit;
    Vector2 mousePos;
    Vector2 wallNormal;
    #endregion

    #region Booleans
    [Header ( "Action Booleans" )]
    public bool isSpellRdy = true;
    public bool isMainAttackRdy = true;
    public bool isOffAttackRdy = true;
    public bool readyToDash = true;
    public bool canWallJump;
    public bool isBlocking;
    public bool isAir;
    public bool isDashing;
    public bool mouseOnUI;
    public bool hitsGround;
    public bool takeFallDamage;
    public bool goingDown;
    bool invulnerable;
    public bool slowed;
    public bool isDead = false;
    public bool isAlive = true;
    public bool climbingLadder;
    public bool climbingRobe;
    public bool jump;

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
    [Range ( 0.1f, 5f )] public float dashSpeed;

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
    bool directionRight;
    int jumpsCount;
    private float fallStartYPos;
    private float fallDamage;
    float fallTime;
    float fallDamageMultiplierBonus;
    #endregion

    #region Unity Monobehavior Functions
    private void Awake ( )
    {
        movement = gameObject.GetComponent<PlayerMovement> ( );
        stats = gameObject.GetComponent<PlayerClass> ( );
        ToolTip = FindObjectOfType<ToolTip> ( ).gameObject;
        main = rightHand.GetComponent<WeaponPlaceHolder> ( );
        off = leftHand.GetComponent<WeaponPlaceHolder> ( );
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
        //rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponSpeed = stats.baseAttackSpeed.Value;
        //rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponDamage = stats.baseDamage.Value;
        slowEffect = 1;
        newGravity = -( 2 * jumpHeightMax ) / ( timeToJumpApex * timeToJumpApex );
        jumpVelocityMax = Mathf.Abs ( newGravity ) * timeToJumpApex;
        jumpVelocityMin = Mathf.Sqrt ( 2 * Mathf.Abs ( newGravity ) * jumpHeightMin );
        OpenInventory ( );
    }
    private void Update ( )
    {

        CalculateVelocity ( );
        HandleWallSliding ( );
        HandleLadderClimbing ( );
        HandleRobeClimb ( );
        HandleSlows ( );
        UpdateDirection ( );

        if ( !isDead && !isBlocking )
            movement.Move ( new Vector2 ( newVelocity.x * speedScale * slowEffect, newVelocity.y * jumpScale * slowEffect ) * Time.deltaTime );

        if ( newVelocity.x <= 0.1f && newVelocity.x >= -0.1f )
            TurnMousePointerDir ( );

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
    }
    #endregion

    #region Input Functions
    public void SetDirectionalInput ( Vector2 input )
    { directionalInput = input; }
    public void OnJumpInputDown ( )
    {
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
            }
            if ( climbingLadder || climbingRobe )
            {
                newVelocity.y = jumpVelocityMax;
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
        if ( off.weaponType == WeaponType.Shield )
        {
            if ( !isAir )
            {
                isBlocking = true;
                //heroAnim.SetTrigger ( "Block" );
            }

            if ( isBlocking )
            {
                off.UseWeapon ( );
            }
            else
            {
                off.HaltWeapon ( );
            }
        }
        else
        {

            if ( off.weaponType == WeaponType.MeleeWeapon )
            {
        
                off.UseWeapon ( );

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
        off.HaltWeapon ( );
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
        if ( !isBlocking && !mouseOnUI )
        {
            if ( main.weaponType == WeaponType.MeleeWeapon || main.weaponType == WeaponType.TwoHandedMeleeWeapon )
            {
             
                main.UseWeapon ( );

            }
            if ( main.weaponType == WeaponType.RangedWeapon || main.weaponType == WeaponType.TwoHandedRangedWeapon )
            {

            }
        }
    }
    public void GetFirstMouseButtonUp ( )
    {
        main.HaltWeapon ( );
    }
    public void DashInputDown ( )
    {
        if ( !isDashing && readyToDash )
        {
            readyToDash = false;
            //Debug.Log ( "Started Dashing" );
            playerRB.gravityScale = 0;
            isDashing = true;
            movement.dashing = true;
            StartCoroutine ( DashTime ( ) );
            StartCoroutine ( DashCoolDown ( ) );
        }
    }
    public void DashInputHold ( )
    {
        if ( isDashing )
        {

            //Debug.Log ( "Holding Dash Key" );
            //Debug.Log ( "IS DASHING" );
            //heroAnim.SetBool ( "Walk", false );
            //dash animaatio
            if ( directionRight && isDashing )
            {
                newVelocity = new Vector2 ( newVelocity.x, 0 );
                transform.Translate ( new Vector2 ( dashSpeed * Time.deltaTime, 0 ) );

            }
            else if ( !directionRight && isDashing )
            {
                newVelocity = new Vector2 ( newVelocity.x, 0 );
                transform.Translate ( new Vector2 ( -dashSpeed * Time.deltaTime, 0 ) );

            }
        }
    }
    public void DashInputUp ( )
    {
        //Debug.Log ( "Stopped Dashing" );
        isDashing = false;
        movement.dashing = false;
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
            fallDamageMultiplierBonus = 0;
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
            if ( directionalInput.y > 0 )
            {
                jump = false;
                newVelocity.y = ladderClimbSpeed * speedScale;
            }
            if ( directionalInput.y < 0 )
            {
                jump = false;
                newVelocity.y = -ladderClimbSpeed * speedScale;
            }

            if ( directionalInput.y == 0 && !jump )
            {
                newVelocity.y = 0;
            }
        }
    }
    void HandleRobeClimb ( )
    {
        if ( climbingRobe )
        {
            if ( !jump && directionalInput.y == 0 && newVelocity.x != 0 )
            {
                newVelocity.y = 0;
            }
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
                jump = false;
                newVelocity.y = robeClimbSpeed * speedScale;
                newVelocity.x = collision.attachedRigidbody.velocity.x;

            }
            if ( directionalInput.y < 0 )
            {
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
    #endregion

    #region Animation Functions
    /// <summary>
    /// Animates player walk
    /// </summary>
    void WalkAnimation ( )
    {
        if ( newVelocity.x >= 0.05f )
        {
            //heroAnim.SetBool ( "Walk", true );
        }
        else if ( newVelocity.x <= -0.05f )
        {
            //heroAnim.SetBool ( "Walk", true );
        }
        else
        {
            //heroAnim.SetBool ( "Walk", false );
        }
    }
    #endregion

    #region Event Functions
    /// <summary>
    /// Osuessa viholliseen suorittaa tarvittavat calculaatiot
    /// </summary>
    /// <param name="target"></param>
    /// <param name="weaponPower"></param>
    public void DealDamage ( GameObject target, float weaponPower, DamageType damageType )
    {
        if ( target != null )
        {
            float calculatedDamage = Random.Range ( ( int ) stats.baseDamage.Value, ( int ) stats.baseDamageMax.Value );

            if ( playerDealDamageEvent != null )
            {
                playerDealDamageEvent ( target, calculatedDamage, damageType );
            }
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
            playerNotifyEvent ( head.transform, amount.ToString ( ), Color.blue );
        }

        if ( restoreAll )
        {
            stats.FullyRestoreMana ( );
        }
        else
        {
            stats.RestoreMana ( amount );
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
        Debug.Log ( dmg );
        Debug.Log ( damageType );

        if ( float.IsNaN ( dmg ) || invulnerable )
        {
            return;
        }
        float fullResistance = stats.allResistance.Value;
        fullResistance += PlayerClass.instance.damageTypes [ damageType ].Value;

        if ( isBlocking )
        {


            calculatedDamage = dmg * ( 1 - ( fullResistance / ( 20 * level + fullResistance ) ) );
            calculatedDamage = dmg * ( 1 - ( stats.block.Value / ( 10 * level + stats.block.Value ) ) );
            calculatedDamage = dmg * ( 1 - ( stats.armor.Value / ( 50 * level + stats.armor.Value ) ) );
            calculatedDamage = Mathf.Round ( calculatedDamage );

            //Ei saa mennä negatiiviseksi
            if ( calculatedDamage < 0 )
            {
                calculatedDamage = 0;
            }

            color = Color.yellow;
            stats.health.BaseValue -= calculatedDamage;

        }
        else
        {
            /*
            switch ( damageType )
            {   
                case DamageType.Physical:
                    break;
                case DamageType.Fire:
                    break;
                case DamageType.Frost:
                    break;
                case DamageType.Poison:
                    break;
                case DamageType.Raw:
                    break;
                default:
                    break;
            }*/

            if ( damageType == DamageType.Raw )
            {
                color = Color.red;
                calculatedDamage = Mathf.Round ( dmg );
                stats.health.BaseValue -= calculatedDamage;
            }
            else
            {
                color = Color.red;
                calculatedDamage = dmg * ( 1 - ( fullResistance / ( 20 * level + fullResistance ) ) );
                calculatedDamage = dmg * ( 1 - ( stats.armor.Value / ( 50 * level + stats.armor.Value ) ) );
                Debug.Log ( ( stats.armor.Value / ( 50 * level + stats.armor.Value ) ) );
                Debug.Log ( calculatedDamage );
                calculatedDamage = Mathf.Round ( calculatedDamage );

                if ( calculatedDamage < 1 && calculatedDamage > 0 )
                {
                    calculatedDamage = 1;
                }

                stats.health.BaseValue -= calculatedDamage;
            }
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

        //Kuolema
        if ( stats.health.Value <= 0 )
        {
            Die ( );
        }
    }

    /// <summary>
    /// Levelup hetkellä tapahtuvat jutut
    /// </summary>
    public void OnLevelUp ( )
    {

        //esim partikkeli efekti
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

    }

    /// <summary>
    /// Laskee falldamagen ja kuolettavan tippumisen ajan
    /// </summary>
    public void FallToDeathCheck ( )
    {
        if ( newVelocity.y > 0 || movement.collisions.below || movement.collisions.left || movement.collisions.right )
        {
            goingDown = false;
            fallTime = 0;
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
            Debug.Log ( "IS GOING DOWN !!!" );
            goingDown = true;
            fallStartYPos = transform.position.y;
        }

        if ( goingDown )
        {
            fallTime += Time.deltaTime;
            fallDamageMultiplierBonus += fallTime;
        }

        if ( isDashing || wallSliding || isDead || climbingLadder || climbingRobe )
        {
            fallTime = 0;
            fallDamageMultiplierBonus = 0;
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

        if ( takeFallDamage && hitsGround && movement.collisions.below )
        {
            Debug.Log ( fallStartYPos );
            fallDamage = Mathf.Sqrt ( playerRB.mass * 2 * Mathf.Abs ( Physics2D.gravity.y ) * fallStartYPos - transform.position.y ) * ( fallDamageMultiplier + fallDamageMultiplierBonus );
            TakeDamage ( gameObject, fallDamage, DamageType.Raw, 1 );
            takeFallDamage = false;
            hitsGround = false;
        }

        if ( fallTime > fallToDeathTime )
        {
            Debug.Log ( "FELL TO DEATH" );
            fallTime = 0;
            Die ( );
        }
    }

    /// <summary>
    /// Respawnatessa tapahtuvat jutut
    /// </summary>
    /// <param name="playerPos"></param>
    public void Alive ( Transform playerPos )
    {

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

        isAlive = false;
        isDead = true;
        OnRestoreHealth ( -stats.maxHealth.Value, false );
        OnRestoreMana ( -stats.maxMana.Value, false );

        if ( playerDeathEvent != null )
        {
            playerDeathEvent ( transform );
        }

        Debug.Log ( "I'm Dead" );
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
    IEnumerator Attack ( )
    {

        //yield return new WaitForSeconds ( heroAnim.GetCurrentAnimatorStateInfo ( 0 ).length );
        yield return new WaitForSeconds ( stats.baseAttackSpeed.Value );
        rightHand.GetComponent<BoxCollider2D> ( ).enabled = false;

    }

    IEnumerator MainAttackCD ( )
    {

        yield return new WaitForSeconds ( stats.baseAttackSpeed.Value );
        isMainAttackRdy = true;

    }

    IEnumerator OffAttackCD ( )
    {

        yield return new WaitForSeconds ( stats.baseAttackSpeed.Value );
        isOffAttackRdy = true;

    }

    IEnumerator DashTime ( )
    {
        yield return new WaitForSeconds ( dashTime );
        isDashing = false;
        Debug.Log ( "Stopped Dashing" );
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
        yield return new WaitForSeconds ( 3 );
        slowed = false;
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
