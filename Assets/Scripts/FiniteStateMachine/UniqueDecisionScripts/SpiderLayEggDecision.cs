using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/LayEggDecision" )]
public class SpiderLayEggDecision : Decision
{

    public float safeDistance;

    public override bool Decide ( StateController controller )
    {
        bool playerNearby = SensePlayerNear ( controller );
        return playerNearby;
    }

    bool SensePlayerNear(StateController controller)
    {

        Collider2D [ ] cols = Physics2D.OverlapCircleAll ( controller.transform.position, 3f, controller.playerLayer );

        foreach ( var col in cols )
        {
            if ( col.gameObject.CompareTag ( "Player" ) )
            {
                controller.chaseTarget = col.gameObject.transform;

                if ( Vector2.Distance ( controller.transform.position, controller.chaseTarget.position ) > safeDistance )
                {
                    return true;
                }

            }
        }

        return false;
    }
}
