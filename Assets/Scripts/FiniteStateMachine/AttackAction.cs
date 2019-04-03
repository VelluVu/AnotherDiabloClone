using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Perus Hyökkäys action
/// </summary>
[CreateAssetMenu ( menuName = "PluggableAI/Actions/Attack" )]
public class AttackAction : Action
{

    public override void Act ( StateController controller )
    {
        Attack ( controller );
    }

    /// <summary>
    /// Käynnistää hyökkäyksen jos pelaaja on hyökkäys etäisyydellä
    /// </summary>
    /// <param name="controller"></param>
    void Attack ( StateController controller )
    {

        RaycastHit2D hit;

        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius * 0.2f, controller.eyes.right, controller.attackDistance, controller.playerLayer );

            if ( hit.collider != false )
            {
                if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance && controller.attackRdy )
                {

                    controller.Attack ( );

                }
            }
        }
        else
        {
            //Hyökkäys vähän lähempänä
            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius * 0.1f, controller.eyes.right, controller.attackDistance, controller.playerLayer );
            Debug.DrawRay ( controller.eyes.position, controller.eyes.right * controller.spotDistance, Color.red );
            Debug.Log ( "Attack Ray HIT : " + hit.transform );

            if ( hit.collider != false )
            {
                //hit.distance on laaja niin tää on ehkä hölmö tapa testaa attackdistancee
                if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance && controller.attackRdy )
                {

                    controller.Attack ( );

                }
            }
        }
    }
}
