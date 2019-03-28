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

        if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) >= controller.attackDistance )
        {
            if ( controller.transform.position.x > ( controller.chaseTarget.position.x + 0.4f ) )
            {
                controller.rb.velocity = ( controller.targetDir * controller.enemyStats.moveSpeed * controller.moveSpeedScale * -controller.enemyStats.chaseMultiplier * Time.deltaTime );
            }
            else if ( controller.transform.position.x < ( controller.chaseTarget.position.x - 0.4f ) )
            {
                controller.rb.velocity = ( controller.targetDir * controller.enemyStats.moveSpeed * controller.moveSpeedScale * controller.enemyStats.chaseMultiplier * Time.deltaTime );
            }
        }
    }
}
