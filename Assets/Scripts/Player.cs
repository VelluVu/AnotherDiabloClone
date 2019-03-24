using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pelaaja Prototyyppi
/// </summary>
public class Player : MonoBehaviour
{

    public PlayerStats stats; //pelaajan statit
    public Inventory invi; //pelaaja tietää reppunsa 

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
    #endregion

    #region publicVariables
    public float speedScale;
    #endregion

    #region FlagVariables
    int jumpCount = 0;
    int wallJumpDirection = -1;
    #endregion

    private void Start ( )
    {
        playerRB = gameObject.GetComponent<Rigidbody2D> ( );
        heroAnim = gameObject.GetComponent<Animator> ( );
    }

    private void Update ( )
    {
        OpenInventory ( );

        if(!isBlocking)
            PlayerMovement ( );

        PlayerJump ( );
        PlayerBasicAttack ( );
        PlayerBasicBlock ( );
        TurnMousePointerDir ( );
    }

    void TurnMousePointerDir()
    {

        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint ( mousePos );

        if ( playerRB.velocity.x < 0.1f && playerRB.velocity.x > -0.1f )
        {
            if ( mousePos.x > transform.position.x )
            {
                transform.eulerAngles = new Vector3 ( 0, 0, 0 );
            }
            if ( mousePos.x < transform.position.x )
            {
                transform.eulerAngles = new Vector3 ( 0, -180, 0 );
            }
        }
    }

    //Pelaajan liike eri suuntiin
    void PlayerMovement ( )
    {
        float moveX = Input.GetAxis ( "Horizontal" );

        Vector2 movement = new Vector2 ( moveX, 0 );
        
        playerRB.AddForce ( movement * stats.moveSpeed * stats.moveSpeed * speedScale );
        
        if ( playerRB.velocity.x >= 0.1f )
        {
            wallJumpDirection = -1;
            transform.eulerAngles = new Vector3 ( 0, 0, 0 );
            heroAnim.SetBool ( "Walk", true );
        }
        else if ( playerRB.velocity.x <= -0.1f )
        {
            wallJumpDirection = 1;
            transform.eulerAngles = new Vector3 ( 0, -180, 0 );
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

        if ( Input.GetButtonDown ( "Jump" ) && jumpCount < 2 && !isBlocking)
        {
            if ( isWallJump)
            {
                playerRB.AddForce ( new Vector2 ( wallJumpDirection * ( playerRB.velocity.x + 100 ), stats.jumpForce * 3f ), ForceMode2D.Impulse );
                heroAnim.SetTrigger ( "Jump" );
            }
            playerRB.AddForce ( new Vector2 ( 0, stats.jumpForce * stats.jumpForce ), ForceMode2D.Impulse );
            heroAnim.SetTrigger ( "Jump" );

            jumpCount++;

        }
    }

    /// <summary>
    /// Perus Isku hiiren vasemmasta
    /// </summary>
    void PlayerBasicAttack ( )
    {
        if ( Input.GetMouseButtonDown ( 0 ) && !isBlocking )
        {
            heroAnim.SetTrigger ( "Attack" );
            
            rightHand.GetComponent<BoxCollider2D> ( ).enabled = true;
            StartCoroutine ( Attack ( ) );
        }
       
    }

    //Colliderin ylläpito
    IEnumerator Attack()
    {
        yield return new WaitForSeconds ( 1.0f );
        rightHand.GetComponent<BoxCollider2D> ( ).enabled = false;
    }

    /// <summary>
    /// Blockkaa iskuja
    /// </summary>
    void PlayerBasicBlock ( )
    {
        if ( Input.GetMouseButton ( 1 ) && !isAir)
        {
            isBlocking = true;
            heroAnim.SetTrigger ( "Block" );
        }
        if ( Input.GetMouseButtonUp ( 1 ) )
        {
            isBlocking = false;
        }

        if(isBlocking)
        {
            leftHand.GetComponent<BoxCollider2D> ( ).enabled = true ;
        }
        else
        {
            leftHand.GetComponent<BoxCollider2D> ( ).enabled = false;
        }
    }

    //Avaa inventaarion
    public void OpenInventory ( )
    {
        if ( Input.GetKeyDown ( "b" ) )
        {
            if ( invi.gameObject.activeSelf == true )
            {
                invi.gameObject.SetActive ( false );
            }
            else
            {
                invi.gameObject.SetActive ( true );
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
        if ( (collision.gameObject.CompareTag ( "Door" ) || collision.gameObject.CompareTag("TreasureChest")) && Input.GetKey ( "e" ))
        {
            collision.gameObject.SendMessage ( "Aukene");
        }
    }

    //Katsoo osuuko loottiin
    private void OnTriggerEnter2D ( Collider2D other )
    {

        if ( other.gameObject.CompareTag ( "PickUp" ) && other.gameObject.GetComponent<Item> ( ).pickedUp == false )
        {
            other.gameObject.SetActive ( false );
            invi.AddItemToInventory ( other.gameObject.GetComponent<Item> ( ).GetItem ( ) );
        }
    }
}