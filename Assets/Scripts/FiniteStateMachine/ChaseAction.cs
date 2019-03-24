using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{

    public float chaseMultiplier;

    public override void Act ( StateController controller )
    {
        Chase ( controller );
    }

    void Chase( StateController controller)
    {

        if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) >= controller.attackDistance )
        {
            if(controller.transform.position.x > controller.chaseTarget.position.x + 0.6f)
            {
                controller.rb.AddForce ( Vector2.right * controller.enemyStats.moveSpeed * controller.enemyStats.moveSpeed * controller.moveSpeedScale * -chaseMultiplier );
            }
            else if ( controller.transform.position.x < controller.chaseTarget.position.x - 0.6f )
            {             
                controller.rb.AddForce ( Vector2.right * controller.enemyStats.moveSpeed * controller.enemyStats.moveSpeed * controller.moveSpeedScale * chaseMultiplier );
            }
        }
    }
}
