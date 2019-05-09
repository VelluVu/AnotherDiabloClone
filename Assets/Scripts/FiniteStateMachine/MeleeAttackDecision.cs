using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/MeleeAttack" )]
public class MeleeAttackDecision : Decision
{
    public override bool Decide ( StateController controller )
    {
        bool enemyInAttackRange = AttackRangeCheck ( controller );
        return enemyInAttackRange;
    }

    public bool AttackRangeCheck(StateController controller )
    {
        RaycastHit2D hit;
        RaycastHit2D hitDown;

        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            
            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.circleCastRadius, controller.eyes.right, controller.attackDistance, controller.targetLayer );

            //Debug.DrawRay ( controller.eyes.transform.position, controller.eyes.right, Color.red, 1f );

            if ( hit.collider != false )
            {
                
                if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance )
                {
                    controller.chaseTarget = hit.transform;
                    
                    return true;

                }
            }
        }
        else
        {

            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.circleCastRadius, controller.eyes.right, controller.attackDistance, controller.targetLayer );
            hitDown = Physics2D.CircleCast ( controller.eyes.transform.position, controller.circleCastRadius, -controller.eyes.up, controller.attackDistance, controller.targetLayer );

            //Debug.DrawRay ( controller.eyes.transform.position, controller.eyes.right, Color.red, 1f );
            //Debug.DrawRay ( controller.eyes.transform.position, -controller.eyes.up, Color.red, 1f );

            if ( hit.collider != false )
            {
                if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance )
                {
                    controller.chaseTarget = hit.transform;
                    
                    return true;
                }
            }
            if ( hitDown.collider != false )
            {
                if ( hitDown.collider.gameObject.CompareTag ( "Player" ) && hitDown.distance <= controller.attackDistance )
                {
                    controller.chaseTarget = hit.transform;
                   
                    return true;
                }
            }
        }
        return false;
    }
}
