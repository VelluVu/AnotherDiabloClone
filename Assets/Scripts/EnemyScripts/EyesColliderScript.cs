using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesColliderScript : MonoBehaviour
{

    bool hasDealtDmg;

    private void Update ( )
    {
        if(gameObject.GetComponent<BoxCollider2D>().isActiveAndEnabled)
        {
            hasDealtDmg = false;
        }
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if(collision.gameObject.CompareTag("Player") && !hasDealtDmg)
        {
            Debug.Log ( gameObject.GetComponentInParent<StateController>().enemyStats.name + " Hits you" );
            hasDealtDmg = true;
            gameObject.GetComponentInParent<StateController> ( ).DealDamage ( collision.GetComponentInParent<Player>() );
            StartCoroutine ( DmgCooldown ( ) );
        }
    }

    IEnumerator DmgCooldown()
    {
        yield return new WaitForSeconds ( gameObject.GetComponentInParent<StateController>().enemyStats.attackSpeed );
        hasDealtDmg = false;
    }



}
