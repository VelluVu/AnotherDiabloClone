using System.Collections;
using System.Collections.Generic;
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
    #endregion

    #region EssentialComponents
    Animator heroAnim;
    Rigidbody2D playerRB;
    #endregion

    #region Booleans
    bool isWallJump = false;
    bool isBlocking = false;
    bool isAir = false;
    bool isAttackRdy = true;
    #endregion

    #region publicVariables
    public WeaponPlaceHolder main;
    public WeaponPlaceHolder off;
    public float speedScale;
    public float airSpeed;
    public float wallJumpPushBack;
    #endregion

    #region FlagVariables
    int jumpCount = 0;
    int wallJumpDirection = -1;
    float positiveVelocity = 0.1f;
    float negativeVelocity = -0.1f;
    #endregion

    private void Awake ( )
    {
        DontDestroyOnLoad ( this );
    }

    private void Start ( )
    {
        playerRB = gameObject.GetComponent<Rigidbody2D> ( );
        heroAnim = gameObject.GetComponent<Animator> ( );
        rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponSpeed = stats.baseAttackSpeed.Value;
        rightHand.GetComponent<WeaponPlaceHolder> ( )._weaponDamage = stats.baseDamage.Value;
        leftDir = new Vector3 ( 0, -180, 0 );
        rightDir = new Vector3 ( 0, 0, 0 );
    }

    private void Update ( )
    {
        OpenInventory ( );

        if ( !isBlocking )
            PlayerMovement ( );

        PlayerJump ( );
        PlayerBasicAttack ( );
        PlayerBasicBlock ( );
        TurnMousePointerDir ( );
    }

    void TurnMousePointerDir ( )
    {

        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint ( mousePos );

        if ( playerRB.velocity.x < positiveVelocity && playerRB.velocity.x > negativeVelocity )
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

    //Pelaajan liike eri suuntiin
    void PlayerMovement ( )
    {
        float moveX = Input.GetAxis ( "Horizontal" );

        Vector2 movement = new Vector2 ( moveX, 0 );

        if ( !isAir )
        {
            playerRB.AddForce ( movement * stats.moveSpeed.Value * stats.moveSpeed.Value * speedScale );
        }
        else
        {
            playerRB.AddForce ( movement * stats.moveSpeed.Value * stats.moveSpeed.Value * airSpeed * speedScale );
        }

        if ( playerRB.velocity.x >= 0.1f )
        {
            wallJumpDirection = -1;
            transform.eulerAngles = rightDir;
            heroAnim.SetBool ( "Walk", true );
        }
        else if ( playerRB.velocity.x <= -0.1f )
        {
            wallJumpDirection = 1;
            transform.eulerAngles = leftDir;
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

        if ( Input.GetButtonDown ( "Jump" ) && jumpCount < 2 && !isBlocking )
        {
            isAir = true;
            if ( isWallJump )
            {
                playerRB.AddForce ( new Vector2 ( wallJumpDirection * ( playerRB.velocity.x + wallJumpPushBack ), stats.jumpForce.Value * 3f ), ForceMode2D.Impulse );
                heroAnim.SetTrigger ( "Jump" );
            }
            playerRB.AddForce ( new Vector2 ( 0, stats.jumpForce.Value * stats.jumpForce.Value ), ForceMode2D.Impulse );
            heroAnim.SetTrigger ( "Jump" );

            jumpCount++;

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

    public void DealDamage ( StateController enemy, float weaponPower )
    {

        if ( enemy != null )
        {
            enemy.TakeDamage ( weaponPower + stats.baseDamage.Value );
            if (enemy.enemyStats.health <= 0)
            {
                levelUp.GainXP ( enemy.enemyStats.xpReward, stats );
            }
        }
    }

    public void TakeDamage ( float dmg )
    {
        stats.health.BaseValue -= ( dmg - stats.armor.Value );

        if ( stats.health.Value <= 0 )
        {
            Die ( );
        }
    }

    public void Die ( )
    {
        Debug.Log ( "I'm Dead" );
    }

    //Colliderin ylläpito
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

    /// <summary>
    /// Blockkaa iskuja
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

    //Avaa inventaarion
    public void OpenInventory ( )
    {
        if ( Input.GetKeyDown ( "i" ) )
        {
            if ( PlayerInventory.instance.gameObject.activeSelf == true )
            {
                PlayerInventory.instance.gameObject.SetActive ( false );
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
            jumpCount = 0;
        }

        if ( collision.gameObject.CompareTag ( "Wall" ) )
        {
            isWallJump = true;
            jumpCount--;
        }

        if ( collision.gameObject.CompareTag ( "Enemy" ) )
        {
            playerRB.AddForce ( new Vector2 ( wallJumpDirection * ( playerRB.velocity.x + wallJumpPushBack ), stats.jumpForce.Value * 3f ), ForceMode2D.Impulse );
            heroAnim.SetTrigger ( "Jump" );
        }
    }

    private void OnCollisionExit2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Ground" ) )
        {
            isAir = true;
        }
    }

    private void OnCollisionStay2D ( Collision2D collision )
    {
        if ( ( collision.gameObject.CompareTag ( "Door" ) || collision.gameObject.CompareTag ( "TreasureChest" ) ) && Input.GetKey ( "e" ) )
        {
            collision.gameObject.SendMessage ( "Aukene" );
        }
    }

    //Katsoo osuuko loottiin
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
}
