using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Actions/SpiderBossChangePositionAction" )]
public class SpiderBossChangePositionAction : Action
{
    public override void Act ( StateController controller )
    {
        ChangePosition ( controller );
    }

    void ChangePosition ( StateController controller )
    {

        float chosenDistance = 0;
        Vector2 chosenPosition = new Vector2(0,0);

        if ( !controller.changedPos )
        {
            for ( int i = 0 ; i < controller.positions.Count ; i++ )
            {
                if ( i == 0 )
                {                    
                    chosenDistance = Vector2.Distance ( controller.positions [ i ].position, controller.chaseTarget.position );
                    chosenPosition = controller.positions [ i ].position;
                }

                if ( i > 0 )
                {
                    if ( Vector2.Distance ( controller.positions [ i ].position, controller.chaseTarget.position ) > chosenDistance )
                    {
                        chosenDistance = Vector2.Distance ( controller.positions [ i ].position, controller.chaseTarget.position );
                        chosenPosition = controller.positions [ i ].position;
                    }
                    else
                    {
                        continue;
                    }
                }          
            }
            controller.transform.position = chosenPosition;
            controller.changedPos = true;
        }
    }
}
