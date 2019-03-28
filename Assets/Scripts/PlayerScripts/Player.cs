using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pelaaja Prototyyppi
/// </summary>
public class Player : MonoBehaviour
{

    public PlayerClass stats; //pelaajan statit
    public LevelUp levelUp;
    Vector3 leftDir;
    Vector3 rightDir;

    #region GameObjects
    public GameObject head;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject dmgTextPrefab;
    GameObject ToolTip;
    #endregion

    #region EssentialComponents
    Animator heroAnim;
    Rigidbody2D playerRB;
    CameraControl camShaking;
    public Image healthPool;
    public Image manaPool;
    public Transform feet;
    public LayerMask whatIsGround;
    public LayerMask enemyLayer;    
    public WeaponPlaceHolder main;
    public WeaponPlaceHolder off;   
    #endregion

    #region Booleans
    bool isWallJump = false;
    bool isBlocking = false;
    bool isAir = false;
    bool isAttackRdy = true;
    bool isJumping = false;
    bool readyToDash = true;
    public bool isDashing;
    public bool isDoubleJumpEnabled;
    public bool isWallJumpEnabled;
    public bool isDashEnabled;
    public bool isGrounded;    
    #endregion

    #region publicVariables  
    public float speedScale;
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed;
    public float wallJumpScale;
    public float airSpeed;
    public float wallJumpPushBack;
    public float jumpTime;
    public float groundCheckRadius;
    float gravity;
    #endregion

    #region FlagVariables
    public int extraJumps;
    public bool directionRight;
    int jumpsCount = 0;
    float jumpTimeCounter;
    int wallJumpDirection = -1;
    float moveX;
    #endregion

    private void Awake ( )
    {
        ToolTip = FindObjectOfType<ToolTip> ( ).gameObject;
        DontDestroyOnLoad ( this );
    }

    private void Start ( )
    {
        camShaking = Camera.main.GetComponent<CameraControl> ( );
        playerRB = gameObject.GetComponent<Rigidbody2D> ( );
        heroAnim = gameObject.GetComponent<Animator> ( );
        rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponSpeed = stats.baseAttackSpeed.Value;
        rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponDamage = stats.baseDamage.Value;
        leftDir = new Vector3 ( 0, -180, 0 );
        rightDir = new Vector3 ( 0, 0, 0 );
        gravity = playerRB.gravityScale;
    }

    private void Update ( )
    {
        moveX = Input.GetAxis ( "Horizontal" );
        GroundCheck ( );
        OpenInventory ( );
        PlayerBasicAttack ( );
        PlayerBasicBlock ( );
        PlayerJump ( );

        if ( !isDashing )
        {
            TurnMousePointerDir ( );       
            WalkAnimation ( );
        }

        if ( isDashEnabled )
        {
            Dash ( );
        }

        HealthTrack ( stats.health.Value );
        Debug.Log ( stats.health.Value * 0.01f );
        ManaTrack ( stats.mana.Value );
    }

    /// <summary>
    /// Alternate ground check
    /// </summary>
    private void GroundCheck ( )
    {
        //layermask tapa tsekkaa grounded testi
        isGrounded = Physics2D.OverlapCircle ( feet.position, groundCheckRadius, whatIsGround );
       
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

        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint ( mousePos );

        if ( playerRB.velocity.x == 0)
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

    /// <summary>
    /// Pelaajan liike X axelilla
    /// </summary>
    void PlayerMovement ( )
    {
        
        //Turning
        if ( moveX > 0 )
        {
            directionRight = true;
            wallJumpDirection = -1;
            transform.eulerAngles = rightDir;
        }
        else
        {
            directionRight = false;
            wallJumpDirection = 1;
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

        if ( isGrounded && Input.GetButtonDown ( "Jump" ) )
        {

            jumpsCount = extraJumps;
            isJumping = true;
            heroAnim.SetTrigger ( "Jump" );
            jumpTimeCounter = jumpTime;

        }

        //Double jump
        if ( Input.GetButtonDown ( "Jump" ) && jumpsCount > 0 && !isJumping && !isWallJump && isDoubleJumpEnabled)
        {

            jumpsCount--;
            Debug.Log ( "DOUBLE JUMP" );
            heroAnim.SetTrigger ( "Jump" );
            playerRB.AddForce ( new Vector2 ( playerRB.velocity.x,  stats.jumpForce.Value * stats.extraJumpForce.Value ), ForceMode2D.Impulse );

        }

        //Wall jump
        if ( isWallJump && Input.GetButtonDown ( "Jump" ) && isWallJumpEnabled )
        {

            isWallJump = false;
            Debug.Log ( "WALL JUMP" );
            heroAnim.SetTrigger ( "Jump" );
            playerRB.AddForce ( new Vector2 ( playerRB.velocity.x * wallJumpDirection * wallJumpPushBack,  stats.extraJumpForce.Value * stats.jumpForce.Value * wallJumpScale ), ForceMode2D.Impulse );

        }

        if ( Input.GetButton ( "Jump" ) && isJumping )
        {

            if ( jumpTimeCounter > 0 )
            {

                //playerRB.AddForce(Vector2.up * stats.jumpForce.Value, ForceMode2D.Impulse);
                playerRB.velocity = new Vector2 ( playerRB.velocity.x,  stats.jumpForce.Value);
                jumpTimeCounter -= Time.deltaTime;

            }
            else
            {

                isJumping = false;

            }
        }

        if ( Input.GetButtonUp ( "Jump" ) )
        {
            isJumping = false;

        }
    }

    /// <summary>
    /// Pelaajan Dash move
    /// </summary>
    public void Dash ( )
    {
        if(isDashing)
        {         
            Debug.Log ( "IS DASHING" );
            heroAnim.SetBool ( "Walk", false );
            //dash animaatio
        }
      
        if( Input.GetButtonDown( "Dash" ) && !isDashing && readyToDash)
        {
            readyToDash = false;
            Debug.Log ( "Started Dashing" );
            playerRB.gravityScale = 0;
            isDashing = true;
            StartCoroutine ( DashTime ( ) );
            StartCoroutine ( DashCoolDown ( ) );
        }

        if ( Input.GetButton ( "Dash" ) && isDashing)
        {
            Debug.Log ( "Holding Dash Key" );
            
            if ( directionRight && isDashing )
            {
                playerRB.velocity = new Vector2 ( playerRB.velocity.x, 0);
                transform.Translate (new Vector2 ( playerRB.velocity.x * dashSpeed * Time.deltaTime, 0 ));

            }
            else if( !directionRight && isDashing )
            {
                playerRB.velocity = new Vector2 ( playerRB.velocity.x, 0 );
                transform.Translate ( new Vector2 ( playerRB.velocity.x * -dashSpeed *  Time.deltaTime, 0 ) );

            }
            else
            {
                Debug.Log ( "Stopped Dashing" );
                isDashing = false;
                playerRB.gravityScale = gravity;
            }
        }   

        if (Input.GetButtonUp ( "Dash" ) )
        {
            Debug.Log ( "Stopped Dashing" );
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
            isAttackRdy = false;
            heroAnim.SetTrigger ( "Attack" );
            rightHand.GetComponent<BoxCollider2D> ( ).enabled = true;
            StartCoroutine ( Attack ( ) );
            StartCoroutine ( AttackCD ( ) );
        }
    }

    /// <summary>
    /// Damage Done calculated and passed to enemy
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="weaponPower"></param>
    public void DealDamage ( StateController enemy, float weaponPower )
    {

        if ( enemy != null )
        {
            enemy.TakeDamage ( weaponPower + stats.baseDamage.Value );
            if ( enemy.enemyStats.health <= 0 )
            {
                levelUp.GainXP ( enemy.enemyStats.xpReward, stats );
            }
        }
    }

    /// <summary>
    /// Player calculates defenses and how much damage is taken
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage ( float dmg )
    {
        float absoluteDmg;
        if ( isBlocking )
        {
            //Varmaan joku defense muuttuja kilpeen ois hyvä
            StartCoroutine ( camShaking.Shake ( 0.05f, 0.05f ) );
            absoluteDmg = dmg - stats.armor.Value * 1.5f;
            if(absoluteDmg < 0)
            {
                absoluteDmg = 0;
                dmgTextPrefab.GetComponent<ScrollingCombatText> ( ).SpawnText ( absoluteDmg, new Vector2(transform.position.x, head.transform.position.y) );
            }
            stats.health.BaseValue -= absoluteDmg;
            Debug.Log ( dmg - stats.armor.Value * 1.5f + " Damage taken " );
            Debug.Log ( stats.armor.Value * 1.5f + " Damage blocked " );     
            
        }
        else
        {

            StartCoroutine ( camShaking.Shake ( 0.1f, 0.1f ) );
            absoluteDmg = dmg - stats.armor.Value;
            stats.health.BaseValue -= absoluteDmg;
            Debug.Log ( dmg - stats.armor.Value + " Damage Taken " );
            dmgTextPrefab.GetComponent<ScrollingCombatText> ( ).SpawnText ( absoluteDmg, new Vector2 ( transform.position.x, head.transform.position.y ) );
                  
        }
        
        if ( stats.health.Value <= 0 )
        {
            Die ( );
        }
    }

    public void HealthTrack(float value)
    {
        healthPool.fillAmount = value * 0.01f;       
    }

    public void ManaTrack(float value)
    {
        manaPool.fillAmount = value * 0.01f;
    }

    /// <summary>
    /// Actions Taken on player death
    /// </summary>
    public void Die ( )
    {
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

        if ( isBlocking )
        {
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

        if ( collision.gameObject.CompareTag ( "Ground" ) )
        {
            isAir = false;
            isWallJump = false;
        }

        if(collision.gameObject.CompareTag("TreasureChest"))
        {
            isAir = false;
            isGrounded = true;
        }

        if ( collision.gameObject.CompareTag ( "Wall" ) )
        {

            isWallJump = true;      
            StartCoroutine ( WallJumpRanOut ( ) );
                     
        }

        if ( collision.gameObject.CompareTag ( "Enemy" ) )
        {

            isAir = false;
            isGrounded = true;
        }

        if ( isDashing && collision.gameObject.CompareTag ( "Enemy" ) )
        {
            Physics2D.IgnoreCollision ( gameObject.GetComponent<BoxCollider2D>() , collision.collider , true);
        }

    }

    /// <summary>
    /// Kun irtoaa maasta ollaan ilmassa
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Ground" ) )
        {
            isAir = true;
        }
        if ( !isDashing && collision.gameObject.CompareTag ( "Enemy" ) )
        {
            Physics2D.IgnoreCollision ( gameObject.GetComponent<BoxCollider2D> ( ), collision.collider, false );
        }
    }

    /// <summary>
    /// Aukaisee oven tai arkun e näppäimestä, tähän voi lisätä muitakin
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D ( Collision2D collision )
    {
        
        if ( ( collision.gameObject.CompareTag ( "Door" ) || collision.gameObject.CompareTag ( "TreasureChest" ) ) && Input.GetButton ( "Interaction" ) )
        {
            collision.gameObject.SendMessage ( "Aukene" );
        }
    }

    /// <summary>
    /// Katsoo törmääkö loottiin
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D ( Collider2D other )
    {

        /*if ( other.gameObject.CompareTag ( "PickUp" ) && other.gameObject.GetComponent<Item> ( ).pickedUp == false )
        {
            other.gameObject.SetActive ( false );
            invi.AddItemToInventory ( other.gameObject.GetComponent<Item> ( ).GetItem ( ) );
        }*/
        if ( other.gameObject.CompareTag ( "PickUp" ) && !other.gameObject.GetComponent<PickUpLoot> ( ).pickedUp )
        {
            Debug.Log ( "Pickup" );
            other.gameObject.GetComponent<PickUpLoot> ( ).pickedUp = true;
            PlayerInventory.instance.AddItem ( other.GetComponent<PickUpLoot> ( ).rLoot, 1 );
            other.gameObject.SetActive ( false );

        }

    }

    #region Coroutines
    IEnumerator Attack ( )
    {
        yield return new WaitForSeconds ( 1.0f );
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
        isWallJump = false;
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
    }
    #endregion

}
