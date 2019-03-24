using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/Scan" )]
public class SearchDecision : Decision
{
    public override bool Decide ( StateController controller )
    {
        bool noEnemyOnSight = Search(controller);
        return noEnemyOnSight;
    }

    private bool Search(StateController controller)
    {

        if ( !controller.dirRight )
        {
            controller.rb.AddForce ( Vector2.right * controller.enemyStats.moveSpeed * controller.enemyStats.moveSpeed, ForceMode2D.Impulse);
        }
        else
        {
            controller.rb.AddForce ( -Vector2.right * controller.enemyStats.moveSpeed * controller.enemyStats.moveSpeed , ForceMode2D.Impulse);
        }

        RaycastHit2D hit = Physics2D.Raycast ( controller.eyes.transform.position, controller.eyes.right );
        if(hit.collider != false)
        {
            if(hit.collider.gameObject.CompareTag("Player") && hit.distance <= controller.spotDistance)
            {
                return controller.CheckIfCountDownElapsed(controller.searchDuration);
            }
            else
            {
                return true;
            }
        }

        return true;
       
    }
}
