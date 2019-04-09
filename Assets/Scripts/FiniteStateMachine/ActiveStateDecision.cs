using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/ActiveState" )]
public class ActiveStateDecision : Decision
{
    public override bool Decide ( StateController controller )
    { 
        bool chaseTargetIsActive = Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) <= controller.spotDistance;

        if ( !chaseTargetIsActive )
        {
            controller.chaseTarget = null;
        }
        
        return chaseTargetIsActive;
    }
}
