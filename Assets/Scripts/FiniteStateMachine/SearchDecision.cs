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
           
      
        RaycastHit2D hit = Physics2D.Raycast ( controller.eyes.transform.position, controller.eyes.right );
        if(hit.collider != false)
        {
            if(hit.collider.gameObject.CompareTag("Player") && hit.distance <= controller.spotDistance)
            {              
                return controller.CheckIfCountDownElapsed(controller.searchDuration);
            }             
        }     

        return controller.CheckIfCountDownElapsed ( controller.searchDuration );
    } 
}
