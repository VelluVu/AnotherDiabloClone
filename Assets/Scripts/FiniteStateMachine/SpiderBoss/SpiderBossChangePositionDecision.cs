using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/SpiderBoss/positionChange" )]
public class SpiderBossChangePositionDecision : Decision
{
    public override bool Decide ( StateController controller )
    {
        bool changePosition = ChangePos ( controller );
        return changePosition;
    }

    bool ChangePos ( StateController controller )
    {

        int healthPercent = Mathf.RoundToInt ( controller.enemyStats.GetHealthPercent ( ) );
        Debug.Log ( healthPercent );

        if ( healthPercent <= 80f && !controller.phase1 )
        {
            controller.phase1 = true;
            controller.changedPos = false;
            return true;
        }
        if ( healthPercent <= 60f && !controller.phase2 )
        {
            controller.phase2 = true;
            controller.changedPos = false;
            return true;
        }
        if ( healthPercent <= 40f && !controller.phase3 )
        {
            controller.phase3 = true;
            controller.changedPos = false;
            return true;
        }
        if ( healthPercent <= 20f && !controller.phase4 )
        {
            controller.phase4 = true;
            controller.changedPos = false;
            return true;
        }

        return false;
    }
}
