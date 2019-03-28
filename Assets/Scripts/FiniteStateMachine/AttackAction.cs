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

        hit = Physics2D.Raycast ( controller.eyes.transform.position, controller.eyes.right , controller.enemyLayer );

        if ( hit.collider != false )
        {
            if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance && controller.attackRdy)
            {

                controller.Attack ( );

            }
        }
    }
}
