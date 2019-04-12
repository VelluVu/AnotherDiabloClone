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
public class Player : MonoBehaviour
{

    public PlayerClass stats; //pelaajan statit
    public LevelUp levelUp;

    #region Player Delegates
    public delegate void PlayerDeathDelegate ( Transform transform );
    public static event PlayerDeathDelegate playerDeathEvent;

    public delegate void PlayerTakeDamageDelegate ( float damage );
    public static event PlayerTakeDamageDelegate playerTakeDamageEvent;

    public delegate IEnumerator PlayerFlashDelegate ( GameObject source, float time, Color color, bool isFlashSpam );
    public static event PlayerFlashDelegate playerFlashEvent;

    public delegate void PlayerDealDamageDelegate ( GameObject enemy, float damage );
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
    [HideInInspector] public GameObject head;
    [HideInInspector] public GameObject rightHand;
    [HideInInspector] public GameObject leftHand;
    GameObject ToolTip;
    #endregion

    #region EssentialComponents
    [HideInInspector] Animator heroAnim;
    [HideInInspector] Rigidbody2D playerRB;
    public Transform feet;
    public LayerMask whatIsGround;
    public LayerMask groundRayLayer;
    [HideInInspector] public LayerMask enemyLayer;
    [HideInInspector] public WeaponPlaceHolder main;
    [HideInInspector] public WeaponPlaceHolder off;
    RaycastHit hit;
    Vector2 mousePos;
    Vector2 wallNormal;
    #endregion

    #region Booleans
    //Väliaikainen spellcd
    bool isSpellRdy = true;
    public bool canWallJump;
    public bool isDoubleJumping;
    bool isBlocking;
    public bool isAir;
    bool isAttackRdy = true;
    public bool isJumping;
    public bool initialJump;
    public bool isWallJumping;
    bool readyToDash = true;
    public bool isDashing;
    public bool isGrounded;
    public bool mouseOnUI;

    [Header ( "Ability Unlocks" )]
    public bool isDoubleJumpLearned;
    public bool isWallJumpLearned;
    public bool isDashLearned;

    #endregion

    #region publicVariables  
    [Header ( "Movement Variables" )]
    public float speedScale;
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed;
    public float wallJumpScale;
    public float extraJumpScale;
    public float jumpScale;
    public float airSpeed;
    public float wallJumpPushBack;
    public float jumpTime;
    public float groundCheckRadius;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float fallToDeathTime;
    public float lowFallDamageTime;
    public float fallDamageMultiplier;
    #endregion

    #region Extra Variables
    public int extraJumps;
    bool directionRight;
    int jumpsCount;
    float jumpTimeCounter;
    //public int wallJumpDirection = -1;
    float moveX;
    float gravity;
    float curYPos;
    Vector3 leftDir;
    Vector3 rightDir;
    Vector2 textPosition;
    private float fallStartYPos;
    private float distanceToFallHit;
    private bool goingDown;
    private bool takeFallDamage;
    private float fallDamage;
    private float fallTime;    
    #endregion

    private void Awake ( )
    {
        DontDestroyOnLoad ( this );

        ToolTip = FindObjectOfType<ToolTip> ( ).gameObject;
        main = rightHand.GetComponent<WeaponPlaceHolder> ( );
        off = leftHand.GetComponent<WeaponPlaceHolder> ( );
    }

    //Eventtien subscribausta varten
    private void OnEnable ( )
    {
        LevelUp.levelUpEvent += OnLevelUp;
        StateController.enemyDealDamageEvent += TakeDamage;
        Projectile.projectileHitEvent += TakeDamage;
    }

    private void OnDisable ( )
    {
        LevelUp.levelUpEvent -= OnLevelUp;
        StateController.enemyDealDamageEvent -= TakeDamage;
        Projectile.projectileHitEvent -= TakeDamage;
    }

    private void Start ( )
    {
        playerRB = gameObject.GetComponent<Rigidbody2D> ( );
        heroAnim = gameObject.GetComponent<Animator> ( );
        rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponSpeed = stats.baseAttackSpeed.Value;
        rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponDamage = stats.baseDamage.Value;
        leftDir = new Vector3 ( 0, -180, 0 );
        rightDir = new Vector3 ( 0, 0, 0 );
        gravity = playerRB.gravityScale;
    }

    public void SetMouseOnUI ( bool isMouseOnUI ) { mouseOnUI = isMouseOnUI; }

    private void Update ( )
    {

        moveX = Input.GetAxis ( "Horizontal" );

        FallToDeathCheck ( );

        GroundCheck ( );
        OpenInventory ( );

        if ( !mouseOnUI )
        {
            PlayerBasicAttack ( );
        }

        PlayerBasicBlock ( );
        //CastSpell ( );

        if ( !isBlocking )
        {
            PlayerJump ( );
        }

        if ( !isDashing )
        {
            TurnMousePointerDir ( );
            WalkAnimation ( );
        }

        if ( isDashLearned )
        {
            Dash ( );
        }

    }

    /// <summary>
    /// Alternate ground check
    /// </summary>
    private void GroundCheck ( )
    {
        //layermask tapa tsekkaa grounded testi
        isGrounded = Physics2D.OverlapCircle ( feet.position, groundCheckRadius, whatIsGround );

        if ( isGrounded )
        {          
            isDoubleJumping = false;
            canWallJump = false;
            isJumping = false;
            isAir = false;
            isWallJumping = false;
            goingDown = false;
            takeFallDamage = false;
            fallTime = 0;
            jumpsCount = extraJumps;
        }

    }

    private void FixedUpdate ( )
    {

        if ( !isBlocking )
            PlayerMovement ( );

    }

    /// <summary>
    /// Kääntää pelaajan hiiren suuntaan
    /// </summary>
    void TurnMousePointerDir ( )
    {

        MousePosition ( );

        if ( playerRB.velocity.x == 0 )
        {
            if ( mousePos.x > transform.position.x )
            {
                transform.eulerAngles = rightDir;
            }
            if ( mousePos.x < transform.position.x )
            {
                transform.eulerAngles = leftDir;
            }
        }
    }

    void MousePosition ( )
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint ( mousePos );
    }

    void Dir ( bool dir )
    {
        if ( dir )
        {
            transform.eulerAngles = rightDir;
        }
        else
        {
            transform.eulerAngles = leftDir;
        }
    }

    /// <summary>
    /// Pelaajan liike X axelilla
    /// </summary>
    void PlayerMovement ( )
    {
        if ( !isWallJumping )
        {
            //Turning
            if ( moveX > 0.2f )
            {
                directionRight = true;
                //wallJumpDirection = -1;
                transform.eulerAngles = rightDir;
            }
            else if ( moveX < -0.2f )
            {
                directionRight = false;
                //wallJumpDirection = 1;
                transform.eulerAngles = leftDir;
            }

            //lisää air multiplierin speedscalen sijaan

            if ( isAir )
            {
                playerRB.velocity = new Vector2 ( ( moveX * stats.moveSpeed.Value ) * airSpeed * Time.fixedDeltaTime, playerRB.velocity.y );
            }
            else
            {
                playerRB.velocity = new Vector2 ( ( moveX * stats.moveSpeed.Value ) * speedScale * Time.fixedDeltaTime, playerRB.velocity.y );
            }
        }

    }

    /// <summary>
    /// Animates player movement
    /// </summary>
    void WalkAnimation ( )
    {

        if ( playerRB.velocity.x >= 0.05f )
        {
            heroAnim.SetBool ( "Walk", true );
        }
        else if ( playerRB.velocity.x <= -0.05f )
        {
            heroAnim.SetBool ( "Walk", true );
        }
        else
        {
            heroAnim.SetBool ( "Walk", false );
        }
    }

    /// <summary>
    /// Pelaajan Hyppy, tuplahyppy ja seinähyppy
    /// </summary>
    void PlayerJump ( )
    {

        if ( isWallJumping )
        {
            playerRB.gravityScale = 0.5f;

        }
        else
        {
            playerRB.gravityScale = 1f;
        }

        if ( Input.GetButtonDown ( "Jump" ) && !initialJump && isGrounded )
        {
            //Debug.Log ( "InitJump" );
            initialJump = true;
        }
        //MarioJump
        if ( Input.GetButton ( "Jump" ) && !isJumping && initialJump && !isDoubleJumping )
        {
            //Debug.Log ( "jumping" );
            heroAnim.SetTrigger ( "Jump" );
            isJumping = true;
            isAir = true;
            playerRB.AddForce ( Vector2.up * stats.jumpForce.Value * jumpScale );

        }

        //Debug.Log ( playerRB.velocity.y );

        //DoubleJump         
        if ( Input.GetButtonDown ( "Jump" ) && jumpsCount > 0 && !canWallJump && isDoubleJumpLearned && isJumping && playerRB.velocity.y < 1.0f && !initialJump && !isGrounded )
        {
            //Debug.Log ( "DoubleJump" );
            isDoubleJumping = true;
            isWallJumping = false;
            jumpsCount--;
            playerRB.AddForce ( Vector2.up * stats.extraJumpForce.Value * extraJumpScale, ForceMode2D.Impulse );
        }
        else if ( Input.GetButtonDown ( "Jump" ) && jumpsCount > 0 && !canWallJump && isDoubleJumpLearned && isJumping && playerRB.velocity.y >= 1.0f && !initialJump && !isGrounded )
        {
            //Debug.Log ( "Limiting the jump" );
            isDoubleJumping = true;
            isWallJumping = false;
            jumpsCount--;
            playerRB.AddForce ( Vector2.up * stats.extraJumpForce.Value * extraJumpScale * 0.5f, ForceMode2D.Impulse );
        }

        //WallJump
        if ( Input.GetButtonDown ( "Jump" ) && canWallJump && isWallJumpLearned && isJumping && !initialJump && !isGrounded )
        {

            isWallJumping = true;

            //playerRB.velocity = Vector2.up * 0.2f * wallNormal * wallJumpPushBack * stats.extraWallJumpForce.Value * wallJumpScale * Time.deltaTime;
            if ( playerRB.velocity.x > 0 )
            {
                Dir ( true );
            }
            else
            {
                Dir ( false );
            }

            playerRB.AddForce ( ( Vector2.up * 0.8f + wallNormal ) * wallJumpPushBack * stats.extraWallJumpForce.Value * wallJumpScale, ForceMode2D.Impulse );

        }

        if ( Input.GetButtonUp ( "Jump" ) )
        {
            canWallJump = false;

        }


        //Better Gravity in jump
        if ( playerRB.velocity.y < 0 )
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity.y * ( fallMultiplier - 1 ) * Time.deltaTime;
        }
        else if ( !Input.GetButton ( "Jump" ) && playerRB.velocity.y > 0 )
        {
            Debug.Log ( "Input false" );
            initialJump = false;
            //isJumping = false;
            playerRB.velocity += Vector2.up * Physics2D.gravity.y * ( lowJumpMultiplier - 1 ) * Time.deltaTime;
        }

    }

    /// <summary>
    /// Pelaajan Dash move
    /// </summary>
    public void Dash ( )
    {
        if ( isDashing )
        {
            //Debug.Log ( "IS DASHING" );
            heroAnim.SetBool ( "Walk", false );
            //dash animaatio
        }

        if ( Input.GetButtonDown ( "Dash" ) && !isDashing && readyToDash )
        {
            readyToDash = false;
            //Debug.Log ( "Started Dashing" );
            playerRB.gravityScale = 0;
            isDashing = true;
            StartCoroutine ( DashTime ( ) );
            StartCoroutine ( DashCoolDown ( ) );
        }

        if ( Input.GetButton ( "Dash" ) && isDashing )
        {
            //Debug.Log ( "Holding Dash Key" );

            if ( directionRight && isDashing )
            {
                playerRB.velocity = new Vector2 ( playerRB.velocity.x, 0 );
                transform.Translate ( new Vector2 ( playerRB.velocity.x * dashSpeed * Time.deltaTime, 0 ) );

            }
            else if ( !directionRight && isDashing )
            {
                playerRB.velocity = new Vector2 ( playerRB.velocity.x, 0 );
                transform.Translate ( new Vector2 ( playerRB.velocity.x * -dashSpeed * Time.deltaTime, 0 ) );

            }
            else
            {
                //Debug.Log ( "Stopped Dashing" );
                isDashing = false;
                playerRB.gravityScale = gravity;
            }
        }

        if ( Input.GetButtonUp ( "Dash" ) )
        {
            //Debug.Log ( "Stopped Dashing" );
            isDashing = false;
            playerRB.gravityScale = gravity;
        }
    }

    /// <summary>
    /// Perus Isku hiiren vasemmasta
    /// </summary>
    void PlayerBasicAttack ( )
    {

        if ( Input.GetMouseButtonDown ( 0 ) && !isBlocking && isAttackRdy )
        {
            if ( main.weaponType == WeaponType.MeleeWeapon || main.weaponType == WeaponType.TwoHandedMeleeWeapon )
            {
                isAttackRdy = false;
                heroAnim.SetTrigger ( "Attack" );
                rightHand.GetComponent<BoxCollider2D> ( ).enabled = true;
                StartCoroutine ( Attack ( ) );
                StartCoroutine ( AttackCD ( ) );

            }
            if ( main.weaponType == WeaponType.RangedWeapon || main.weaponType == WeaponType.TwoHandedRangedWeapon )
            {
                // Ampuminen hiiren suuntaan
            }
        }

    }

    /// <summary>
    /// Inputit ActionBaareille
    /// </summary>
    void CastSpell ( )
    {
        //// Testailua varten
        if ( Input.GetButton ( "ActionBar1" ) && stats.mana.Value >= 0.1f )
        {
            //ChannelSpell
            //Tee taika            
            OnManaUse ( 0.1f );
        }
        else if ( Input.GetButton ( "ActionBar2" ) )
        {
            if ( isSpellRdy && stats.mana.Value >= 10f )
            {
                isSpellRdy = false;
                OnManaUse ( 10f );
                StartCoroutine ( SpellCooldown ( 1f ) );
            }
        }
        else if ( Input.GetButton ( "ActionBar3" ) )
        {
            if ( isSpellRdy && stats.mana.Value >= 15f )
            {
                isSpellRdy = false;
                OnManaUse ( 15f );
                StartCoroutine ( SpellCooldown ( 2f ) );
            }
        }
        else if ( Input.GetButton ( "ActionBar4" ) )
        {
            if ( isSpellRdy && stats.mana.Value >= 50f )
            {
                isSpellRdy = false;
                OnManaUse ( 50f );
                StartCoroutine ( SpellCooldown ( 5f ) );
            }
        }
        else if ( Input.GetButton ( "ActionBar5" ) )
        {
            if ( isSpellRdy && stats.mana.Value >= 100f )
            {
                isSpellRdy = false;
                OnManaUse ( 100f );
                StartCoroutine ( SpellCooldown ( 8f ) );
            }
        }
    }

    /// <summary>
    /// Osuessa viholliseen suorittaa tarvittavat calculaatiot
    /// </summary>
    /// <param name="target"></param>
    /// <param name="weaponPower"></param>
    public void DealDamage ( GameObject target, float weaponPower )
    {
        if ( target != null )
        {
            float calculatedDamage = Random.Range ( ( int ) stats.baseDamage.Value, ( int ) stats.baseDamageMax.Value );





            if ( playerDealDamageEvent != null )
            {
                playerDealDamageEvent ( target, calculatedDamage );
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
    public void TakeDamage ( GameObject target, float dmg )
    {
        float calculatedDamage;
        Color color;
        Debug.Log ( dmg );
        if ( isBlocking )
        {

            //calculatedDamage = dmg - stats.armor.Value /*Tähän pitäis saada vielä shield defense*/;
            calculatedDamage = dmg * ( 1 - ( stats.armor.Value / ( 50 * 100/*vihollisen level*/+ stats.armor.Value ) ) );
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

            color = Color.red;
            calculatedDamage = dmg * ( 1 - ( stats.armor.Value / ( 50 * 1/*vihollisen level*/+ stats.armor.Value ) ) );
            Debug.Log ( ( stats.armor.Value / ( 50 * 1/*vihollisen level*/+ stats.armor.Value ) ) );
            Debug.Log ( calculatedDamage );
            calculatedDamage = Mathf.Round ( calculatedDamage );
            stats.health.BaseValue -= calculatedDamage;


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

    public void FallToDeathCheck ( )
    {
       
        if(playerRB.velocity.y > 0)
        {
            goingDown = false;
        }

        if ( !goingDown && playerRB.velocity.y < -0.05f )
        {
            Debug.Log ( "IS GOING DOWN !!!" );          
            goingDown = true;
            fallStartYPos = transform.position.y;
            
        }

        if(goingDown)
        {
            fallTime += Time.deltaTime;
        }

        Debug.Log ( fallTime );
        if(fallTime > lowFallDamageTime && !takeFallDamage)
        {
            takeFallDamage = true;
            fallDamage = Mathf.Sqrt ( fallDamageMultiplier * Mathf.Abs ( Physics.gravity.y ) * fallStartYPos - transform.position.y );
            TakeDamage ( gameObject, fallDamage );
        }

        if (fallTime > fallToDeathTime)
        {
            fallTime = 0;
            Debug.Log ( "FELL TO DEATH" );
            Die ( );     
        }
    }

    public void Alive()
    {

        Time.timeScale = 1;

        if (playerAliveEvent != null)
        {
            playerAliveEvent ( );
        }
    }
    /// <summary>
    /// Kuolllessa tapahtuvat jutut
    /// </summary>
    public void Die ( )
    {     
        if ( playerDeathEvent != null )
        {
            playerDeathEvent ( transform );
        }

        //AVAA UI DEATH CLASS , kun semmonen löytyy.

        //Vois tehdä GetAreaName Jonnekki missä ilmoitetaan areanimestä ui
         
        Debug.Log ( "I'm Dead" );
    }

    /// <summary>
    /// Blockkaa iskuja oikeasta hiiren näppäimestä
    /// </summary>
    void PlayerBasicBlock ( )
    {
        if ( Input.GetMouseButton ( 1 ) && !isAir )
        {
            isBlocking = true;
            heroAnim.SetTrigger ( "Block" );
        }
        if ( Input.GetMouseButtonUp ( 1 ) )
        {
            isBlocking = false;
        }

        //Ei välttämättä tarvi collideria ellei halua tehdä jotain knockback efectiä
        if ( isBlocking )
        {
            playerRB.velocity = playerRB.velocity.x * Vector2.zero;
            leftHand.GetComponent<BoxCollider2D> ( ).enabled = true;
        }
        else
        {
            leftHand.GetComponent<BoxCollider2D> ( ).enabled = false;
        }
    }

    /// <summary>
    /// Avaa inventaarion i näppäimestä
    /// </summary>
    public void OpenInventory ( )
    {
        if ( Input.GetButtonDown ( "OpenInventory" ) )
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
    }

    /// <summary>
    /// resettaa booleanit ja laskurit hyppyjä varten,
    /// myöhemmin myös osumien tarkistus
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D ( Collision2D collision )
    {

        //Seinästä seinähyppy hetkellisesti mahdolliseksi
        if ( collision.gameObject.CompareTag ( "Wall" ) )
        {

            canWallJump = true;
            wallNormal = collision.contacts [ 0 ].normal;
            Debug.DrawRay ( collision.contacts [ 0 ].point, wallNormal );
            //StartCoroutine ( WallJumpRanOut ( ) );

        }

        //Tämä voisi olla myös collision stayssa ehkä, jos on buginen niin siirtää sinne <antaa dashaa vihujen lävitse>
        if ( isDashing && collision.gameObject.CompareTag ( "Enemy" ) )
        {
            Physics2D.IgnoreCollision ( gameObject.GetComponent<BoxCollider2D> ( ), collision.collider, true );
        }

    }

    /// <summary>
    /// Kun irtoaa maasta ollaan ilmassa ja kun irrotaan enemyistä niin on taas mahdollista törmätä dashilla
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Ground" ) || collision.gameObject.CompareTag ( "InteractableObject" ) || collision.gameObject.CompareTag ( "Enemy" ) )
        {
            isAir = true;
            isGrounded = false;
        }
        if ( !isDashing && collision.gameObject.CompareTag ( "Enemy" ) )
        {
            Physics2D.IgnoreCollision ( gameObject.GetComponent<BoxCollider2D> ( ), collision.collider, false );
        }
    }

    #region Coroutines
    IEnumerator Attack ( )
    {
        yield return new WaitForSeconds ( heroAnim.GetCurrentAnimatorStateInfo ( 0 ).length );
        rightHand.GetComponent<BoxCollider2D> ( ).enabled = false;

    }

    IEnumerator AttackCD ( )
    {
        yield return new WaitForSeconds ( stats.baseAttackSpeed.Value * rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponSpeed );
        isAttackRdy = true;

    }

    IEnumerator WallJumpRanOut ( )
    {
        yield return new WaitForSeconds ( 0.2f );
        Debug.Log ( "Wall Jump Ran Out" );
        canWallJump = false;
    }

    IEnumerator DashTime ( )
    {
        yield return new WaitForSeconds ( dashTime );
        isDashing = false;
        playerRB.gravityScale = gravity;
        Debug.Log ( "Stopped Dashing" );
    }

    IEnumerator DashCoolDown ( )
    {
        yield return new WaitForSeconds ( dashCooldown );
        readyToDash = true;
        //ilmoittaa että dash on valmis
        //textPosition = new Vector2 ( transform.position.x, head.transform.position.y + floatingTextPosition );
        //floatingText.SpawnText ( "Ready To Dash", textPosition, Color.cyan );
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
    #endregion

}
