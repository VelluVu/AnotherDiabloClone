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

                    //controller.rb.velocity = new Vector2 ( ( controller.chaseTarget.position.x - controller.transform.position.x ) * controller.enemyStats.moveSpeed.Value * controller.enemyStats.chaseMultiplier.Value , 0 ) * Time.deltaTime;             
                    controller.transform.position = Vector2.MoveTowards ( controller.transform.position, new Vector2(controller.chaseTarget.position.x, controller.transform.position.y), controller.enemyStats.moveSpeed.Value * 0.5f * controller.enemyStats.chaseMultiplier.Value * Time.deltaTime );

                    if ( controller.dirRight )
                    {
                        controller.rb.velocity = Vector2.right;
                    }
                    else
                    {
                        controller.rb.velocity = Vector2.left;
                    }

                }
            }
            else
            {
                if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) >= 0 )
                {
                  
                    //controller.rb.velocity = ( controller.chaseTarget.position - controller.transform.position ) * controller.enemyStats.moveSpeed.Value * controller.enemyStats.chaseMultiplier.Value * Time.deltaTime;
                    controller.transform.position = Vector2.MoveTowards ( controller.transform.position, controller.chaseTarget.position, controller.enemyStats.moveSpeed.Value * 0.5f * controller.enemyStats.chaseMultiplier.Value * Time.deltaTime );

                    if ( controller.dirRight )
                    {
                        controller.rb.velocity = Vector2.right;
                    }
                    else
                    {
                        controller.rb.velocity = Vector2.left;
                    }

                }
            }
        }             
    }
}
