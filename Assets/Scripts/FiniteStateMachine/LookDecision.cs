using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Decisions/Look" )]
public class LookDecision : Decision
{
    public override bool Decide ( StateController controller )
    {
        bool targetVisible = Look ( controller );

        return targetVisible;
    }

    bool Look ( StateController controller )
    {

        RaycastHit2D hit;
    
        hit = Physics2D.Raycast ( controller.eyes.position, controller.eyes.right );
      
        if ( hit.collider != false )
        {
            if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.spotDistance )
            {

                controller.chaseTarget = hit.transform;
                return true;

            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
