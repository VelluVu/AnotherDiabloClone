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
                if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) >= controller.attackDistance )
                {

                    if ( controller.transform.position.x > ( controller.chaseTarget.position.x + controller.attackDistance ) )
                    {
                        controller.rb.velocity = ( controller.targetDir * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * -controller.enemyStats.chaseMultiplier.Value * Time.deltaTime );
                    }
                    else if ( controller.transform.position.x < ( controller.chaseTarget.position.x - controller.attackDistance ) )
                    {
                        controller.rb.velocity = ( controller.targetDir * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * controller.enemyStats.chaseMultiplier.Value * Time.deltaTime );
                    }
                }
            }
            else
            {
                if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) >= controller.attackDistance )
                {
                    if ( !controller.dirRight )
                    {

                        controller.rb.velocity = ( controller.chaseTarget.position - controller.transform.position ) * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale* controller.enemyStats.chaseMultiplier.Value * Time.deltaTime;
                    }
                    else if ( controller.dirRight )
                    {

                        controller.rb.velocity = ( controller.chaseTarget.position - controller.transform.position ) * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * controller.enemyStats.chaseMultiplier.Value * Time.deltaTime;
                    }
                }
            }
        }
    }
}
