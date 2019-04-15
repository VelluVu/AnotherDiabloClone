using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Käyttää tätä hyökkäys colliderina tällä hetkellä, mutta tietyille vihollisille voisi laittaa weaponplaceholder.
/// </summary>
public class EnemyAttackCollider : MonoBehaviour
{

    bool hasDealtDmg;
    public GameObject bloodSplash;
    public DamageType damageType;

    private void Awake ( )
    {
        gameObject.GetComponent<Collider2D> ( ).enabled = false;
    }
    /// <summary>
    /// Osuuko collider pelaajaan ja onko tehnyt vahinkoa vielä, jos ei niin ilmoittaa statemachinelle käyttää dealdamage functiota
    /// </summary>
    /// <param name="collision"></param>
    //private void OnCollisionEnter2D ( Collision2D collision )
    //{
    //    if ( collision.gameObject.CompareTag ( "Player" ) )
    //    {
    //        if ( !hasDealtDmg )
    //        {
    //            Debug.Log ( gameObject.GetComponentInParent<StateController> ( ).enemyStats.name + " Hits you" );
    //            hasDealtDmg = true;
    //            Destroy ( Instantiate ( bloodSplash, collision.contacts [ 0 ].point, Quaternion.identity ), 2f );
    //            gameObject.GetComponentInParent<StateController> ( ).DealDamage ( collision.gameObject.GetComponentInParent<Player> ( ) );
    //            gameObject.GetComponent<Collider2D> ( ).enabled = false;
    //            StartCoroutine ( DmgCooldown ( ) );
    //        }
    //    }
    //}

    ///Jos hirviöllä ei ole asetta
    private void OnTriggerStay2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Player" ) && !hasDealtDmg )
        {

            //Debug.Log ( gameObject.GetComponentInParent<StateController> ( ).enemyStats.name + " Hits you" );
            hasDealtDmg = true;
            Destroy ( Instantiate ( bloodSplash, collision.gameObject.GetComponent<Collider2D> ( ).bounds.ClosestPoint ( transform.position ), Quaternion.identity ), 2f );
            gameObject.GetComponentInParent<StateController> ( ).DealDamage ( collision.gameObject, 0, damageType );
            StartCoroutine ( DmgCooldown ( ) );

        }
    }

    /// <summary>
    /// Mahdollistaa uuden vahingon teon resettaamalla booleanin
    /// </summary>
    /// <returns></returns>
    IEnumerator DmgCooldown ( )
    {
        yield return new WaitForSeconds ( gameObject.GetComponentInParent<StateController> ( ).enemyStats.attackSpeed.Value );
        hasDealtDmg = false;
    }



}
