using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventTrigger : MonoBehaviour
{

    public GameObject boss;
    public GameObject bossHealthUI;
    public Transform bossEntrancePosition;
    public float bossActivateTime;
    public Vector3 areaSize;
    public LayerMask playerLayer;
    public bool bossStarted;

    public delegate void BossEventDelegate ( GameObject boss );
    public static event BossEventDelegate BossEvent;

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Player" ) )
        {
            bossStarted = true;
            boss.SetActive ( true );
            bossHealthUI.SetActive ( true );

            if ( BossEvent != null )
            {
                BossEvent ( boss );
            }
            StartCoroutine ( ActivateBoss ( ) );
        }
    }

    private void Update ( )
    {
        if ( boss == null )
        {
            bossHealthUI.SetActive ( false );
            gameObject.SetActive ( false );
        }
        if ( bossStarted )
        {
            IsPlayerInBossArea ( );
        }
    }

    void IsPlayerInBossArea ( )
    {
        if ( boss == null )
        {
            return;
        }

        if ( Physics2D.OverlapBox ( transform.position, areaSize, 0, playerLayer ) == null )
        {
            boss.transform.position = bossEntrancePosition.position;
            bossHealthUI.SetActive ( false );
            boss.SetActive ( false );
        }
        if ( Physics2D.OverlapBox ( transform.position, areaSize, 0, playerLayer ) )
        {
            bossHealthUI.SetActive ( true );
            boss.SetActive ( true );
        }
    }

    IEnumerator ActivateBoss ( )
    {
        yield return new WaitForSeconds ( bossActivateTime );
        boss.GetComponent<StateController> ( ).aiActive = true;
        gameObject.GetComponent<Collider2D> ( ).enabled = false;
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube ( transform.position, areaSize );
    }

}
