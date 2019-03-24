using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : MonoBehaviour
{

    Rigidbody2D spiderRB;
    BoxCollider2D spiderCol;
    Animator spiderAnim;
    Vector2 pos;
    public GameObject ledgeChecker;

    public float moveSpeed;
    public float moveSpeedScale;
    float distance = 1f;

    public bool dirRight = false;
    bool spotsPlayer = false;
    bool attackReady = true;

    private void Start ( )
    {
        spiderRB = gameObject.GetComponent<Rigidbody2D> ( );
        spiderCol = gameObject.GetComponent<BoxCollider2D> ( );
        spiderAnim = gameObject.GetComponent<Animator> ( );
    }

    private void Update ( )
    {
        if ( !spotsPlayer )
        {
            SpiderMovement ( );
        }

        SpiderGaze ( );
    }

    void SpiderGaze ( )
    {

        RaycastHit2D gaze;

        if ( dirRight )
        {
            gaze = Physics2D.Raycast ( ledgeChecker.transform.position, Vector2.right );

        }
        else
        {
            gaze = Physics2D.Raycast ( ledgeChecker.transform.position, Vector2.left );

        }

        Debug.Log ( gaze.distance );

        if ( gaze.collider != false )
        {
            if ( gaze.collider.gameObject.CompareTag ( "Player" ) )
            {
                if ( gaze.distance <= 0.8f && gaze.distance >= 0.55f )
                {

                    spiderRB.AddForce ( new Vector2 ( spiderRB.velocity.x, spiderRB.velocity.y + 8.0f ), ForceMode2D.Impulse );

                }

                if ( gaze.distance <= 2.5f )

                {
                    spiderRB.AddForce ( new Vector2 ( spiderRB.velocity.x + 0.1f, spiderRB.velocity.y ), ForceMode2D.Impulse );

                    if ( gaze.distance <= 0.5f && attackReady )
                    {
                        spiderAnim.SetTrigger ( "Attack" );
                        attackReady = false;
                        StartCoroutine ( SpiderAttack ( ) );
                    }

                }
            }
        }
    }

    IEnumerator SpiderAttack ( )
    {

        yield return new WaitForSeconds ( 1f );
        attackReady = true;

    }

    void SpiderMovement ( )
    {

        pos = new Vector2 ( 1, 0 );

        if ( dirRight )
        {
            spiderRB.AddForce ( pos * moveSpeed * moveSpeed * moveSpeedScale );
        }
        else
        {
            spiderRB.AddForce ( pos * moveSpeed * moveSpeed * -moveSpeedScale );
        }

        RaycastHit2D hit = Physics2D.Raycast ( ledgeChecker.transform.position, -Vector2.up, distance );

        if ( hit.collider == false )
        {
            if ( dirRight )
            {
                transform.eulerAngles = new Vector3 ( 0, -180, 0 );
                dirRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3 ( 0, 0, 0 );
                dirRight = true;
            }
        }

        if ( spiderRB.velocity.x >= 0.1f )
        {
            transform.eulerAngles = new Vector3 ( 0, 0, 0 );
            spiderAnim.SetBool ( "Walk", true );
        }
        else if ( spiderRB.velocity.x <= -0.1f )
        {
            transform.eulerAngles = new Vector3 ( 0, -180, 0 );
            spiderAnim.SetBool ( "Walk", true );
        }
        else
        {
            spiderAnim.SetBool ( "Walk", false );
            StartCoroutine ( HoldBreath ( pos ) );
        }
    }

    IEnumerator HoldBreath ( Vector2 position )
    {
        yield return new WaitForSeconds ( 1f );

        spiderRB.AddForce ( pos * moveSpeed * moveSpeedScale );

    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Door" ) || collision.gameObject.CompareTag ( "Wall" ) )
        {
            if ( dirRight )
            {
                transform.eulerAngles = new Vector3 ( 0, -180, 0 );
                dirRight = false;
            }
            else if ( !dirRight )
            {
                transform.eulerAngles = new Vector3 ( 0, 0, 0 );
                dirRight = true;
            }
        }
    }

}
