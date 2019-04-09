using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Jahtaus Action Class
/// </summary>
[CreateAssetMenu ( menuName = "PluggableAI/Actions/Chase" )]
public class ChaseAction : Action
{

    public override void Act ( StateController controller )
    {
        Chase ( controller );
    }

    /// <summary>
    /// Jahtaa pelaajaa kunnes on hyökkäys etäisyydellä
    /// </summary>
    /// <param name="controller"></param>
    void Chase ( StateController controller )
    {
        if ( controller.chaseTarget != null )
        {
            if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
            {
                if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) >= 0 )
                {
                    if(controller.chaseTarget.position.x < controller.transform.position.x)
                    {
                        controller.dirRight = false;
                    }
                    else
                    {
                        controller.dirRight = true;
                    }

                    controller.rb.velocity = new Vector2 ( ( controller.chaseTarget.position.x - controller.transform.position.x ) * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * controller.enemyStats.chaseMultiplier.Value * Time.deltaTime, controller.rb.velocity.y );
                 
                }
            }
            else
            {
                if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) >= 0 )
                {
                    if ( controller.chaseTarget.position.x < controller.transform.position.x )
                    {
                        controller.dirRight = false;
                    }
                    else
                    {
                        controller.dirRight = true;
                    }

                    controller.rb.velocity = ( controller.chaseTarget.position - controller.transform.position ) * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * controller.enemyStats.chaseMultiplier.Value * Time.deltaTime;
                    
                }
            }
        }
    }
}
