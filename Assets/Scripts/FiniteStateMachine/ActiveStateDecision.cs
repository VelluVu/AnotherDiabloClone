using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/ActiveState" )]
public class ActiveStateDecision : Decision
{
    public override bool Decide ( StateController controller )
    {
        //bool chaseTargetIsActive = controller.chaseTarget.gameObject.activeSelf;
        bool chaseTargetIsActive = Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) <= controller.spotDistance;
        return chaseTargetIsActive;
    }
}
