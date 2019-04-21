using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Patrollaus Action Class
/// </summary>
[CreateAssetMenu ( menuName = "PluggableAI/Actions/Patrol" )]
public class PatrolAction : Action
{

    public override void Act ( StateController controller )
    {
        Patrol ( controller );
    }

    void Patrol ( StateController controller )
    {

        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            if ( controller.dirRight )
            {

                controller.rb.velocity = new Vector2 ( controller.targetDir.x * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * Time.deltaTime, controller.rb.velocity.y );
            }
            else
            {

                controller.rb.velocity = new Vector2 ( controller.targetDir.x * controller.enemyStats.moveSpeed.Value * -controller.moveSpeedScale * Time.deltaTime, controller.rb.velocity.y );
            }
        }
        else
        {

            if ( controller.dirRight )
            {

                controller.rb.velocity = new Vector2 ( controller.targetDir.x * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * Time.deltaTime, controller.amplitude * ( Mathf.Sin ( 1 - controller.frequency + Time.time ) * Time.deltaTime ) );
                controller.StartCoroutine ( controller.TurnAfterTime ( controller.flyingPatrolDirectionTime, false ) );

            }
            else if ( !controller.dirRight )
            {

                controller.rb.velocity = new Vector2 ( controller.targetDir.x * controller.enemyStats.moveSpeed.Value * -controller.moveSpeedScale * Time.deltaTime, controller.amplitude * ( Mathf.Sin ( 1 - controller.frequency + Time.time ) * Time.deltaTime ) );
                controller.StartCoroutine ( controller.TurnAfterTime ( controller.flyingPatrolDirectionTime, true ) );

            }
        }
    }
}
