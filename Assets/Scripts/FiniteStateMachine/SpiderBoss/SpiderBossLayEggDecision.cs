using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/SpiderBoss/EggPhase" )]
public class SpiderBossLayEggDecision : Decision
{

    public override bool Decide ( StateController controller )
    {
        bool awayFromPlayer = CheckDistanceToPlayer ( controller );
        return awayFromPlayer;
    }

    bool CheckDistanceToPlayer( StateController controller )
    {

        Collider2D [ ] cols = Physics2D.OverlapCircleAll ( controller.transform.position, controller.radius, controller.targetLayer );

        foreach ( var col in cols )
        {
            if ( col.gameObject.CompareTag ( "Player" ) )
            {
                controller.chaseTarget = col.gameObject.transform;
              
                if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) < controller.senseArea )
                {
                    return false;
                }         
            }
        }

        return true;
    }
}
