using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/SpiderBoss/ContinueDecision" )]
public class SpiderBossContinueMainStateDecision : Decision
{
    
    public override bool Decide ( StateController controller )
    {
        bool changedPosition = ChangedPosition ( controller );
        return changedPosition;
    }

    bool ChangedPosition ( StateController controller )
    {
        
        if (controller.changedPos)
        {
            return true;
        }
        

        return false;
    }
}
