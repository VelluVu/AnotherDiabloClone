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
        if ( controller == null )
        {
            return;
        }
        Patrol ( controller );
    }

    void Patrol ( StateController controller )
    {
        
        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {

            controller.rb.velocity = new Vector2 ( controller.GetTargetPosFromVector ( controller.targetWP.position ).normalized.x * controller.enemyStats.moveSpeed.Value * 15 *
                controller.moveSpeedScale * Time.deltaTime, 0 );
            WPChange ( controller );
        }
        else
        {

            controller.rb.velocity = new Vector2 ( controller.GetTargetPosFromVector ( controller.targetWP.position ).normalized.x *
            controller.enemyStats.moveSpeed.Value * 15 * controller.moveSpeedScale,
            controller.amplitude * ( Mathf.Sin ( 1 - controller.frequency + Time.time ) ) ) * Time.deltaTime;

            WPChange ( controller );

        }
    }
    public void WPChange ( StateController controller )
    {
        if ( ExtensionMethods.FlatDistanceTo ( controller.transform.position, controller.targetWP.position ) < 0.2f )
        {

            if ( controller.wPindex + 1 < controller.positions.Count )
            {
                controller.wPindex++;
            }
            else
            {
                controller.wPindex = 0;
            }

            controller.targetWP = controller.positions [ controller.wPindex ];
        }

    }
}