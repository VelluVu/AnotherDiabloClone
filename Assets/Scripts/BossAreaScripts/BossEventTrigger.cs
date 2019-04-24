using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventTrigger : MonoBehaviour
{

    public GameObject boss;
    bool bossActive;
    bool bossRdy;
    public float bossActivateTime;

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            boss.SetActive ( true );
            bossActive = true;
            StartCoroutine ( ActivateBoss ( ) );
        }
    }

    IEnumerator ActivateBoss()
    {
        yield return new WaitForSeconds ( bossActivateTime );
        boss.GetComponent<StateController> ( ).aiActive = true;
        gameObject.SetActive ( false );
    }

    
}
