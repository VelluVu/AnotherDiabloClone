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
        RaycastHit2D hitDown;

        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            
            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius * 0.2f, controller.eyes.right, controller.attackDistance, controller.playerLayer );
           
            if ( hit.collider != false )
            {
                if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance && controller.attackRdy )
                {
                    controller.rb.velocity = Vector2.zero;
                    controller.Attack ( );

                }
            }
        }
        else
        {       

            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius * 0.1f, controller.eyes.right, controller.attackDistance, controller.playerLayer );
            hitDown = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius * 0.1f, -controller.eyes.up, controller.attackDistance, controller.playerLayer );

            if ( hit.collider != false )
            {         
                if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance && controller.attackRdy )
                {
                    controller.rb.velocity = Vector2.zero;
                    controller.Attack ( );
                }
            }
            if ( hitDown.collider != false )
            {
                if ( hitDown.collider.gameObject.CompareTag ( "Player" ) && hitDown.distance <= controller.attackDistance && controller.attackRdy )
                {
                    controller.rb.velocity = Vector2.zero;
                    controller.Attack ( );
                }
            }
        }
    }
}
