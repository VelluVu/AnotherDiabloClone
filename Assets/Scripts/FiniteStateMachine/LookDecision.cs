using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classi mahdollistaa Look decision scriptable objectin luonnin ja liittämisen state machineen
/// </summary>
[CreateAssetMenu ( menuName = "PluggableAI/Decisions/Look" )]
public class LookDecision : Decision
{
    /// <summary>
    /// Yliajaaa Decide rajapinnan function Decide ja palauttaa true staten mikäli pelaaja on etäisyyden päässä muuten false
    /// </summary>
    /// <param name="controller"></param>
    /// <returns></returns>
    public override bool Decide ( StateController controller )
    {
        bool targetVisible = Look ( controller );

        return targetVisible;
    }

    /// <summary>
    /// Katselee eteenpäin, jos näkee pelaajan spotDistance rajoissa
    /// </summary>
    /// <param name="controller">controlloi statejen muuttumista ja mahdollistaa vihollisen/npc parametrien ja functioiden käytön</param>
    /// <returns>palauttaa näkeekö pelaajan</returns>
    bool Look ( StateController controller )
    {

        RaycastHit2D hit;
        //RaycastHit2D hitDown;

        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.circleCastRadius, controller.eyes.right, controller.spotDistance, controller.targetLayer + controller.blockSightLayer );

            if ( hit.collider != false )
            {
                if ( hit.collider.gameObject.CompareTag ( "Player" ) )
                {

                    controller.chaseTarget = hit.transform;
                    return true;

                }
            }
        }
        else
        {

            Collider2D [ ] cols = Physics2D.OverlapCircleAll ( controller.eyes.position, controller.senseArea, controller.targetLayer );

            if ( cols != null )
            {
                foreach ( var col in cols )
                {
                    controller.chaseTarget = col.transform;

                    hit = Physics2D.CircleCast ( controller.eyes.position, controller.circleCastRadius, controller.chaseTarget.position - controller.eyes.position, controller.spotDistance, controller.targetLayer + controller.blockSightLayer );
                 
                    Debug.DrawRay ( controller.eyes.position, controller.chaseTarget.position - controller.eyes.position, Color.green, 1f );
                    if ( hit )
                    {
                        
                        if (hit.collider.gameObject.CompareTag("Wall"))
                        {
                            controller.chaseTarget = null;
                            return false;
                            
                        }
                        if(hit.collider.gameObject.CompareTag("Player"))
                        {
                            controller.chaseTarget = hit.transform;
                            return true;
                        }
                    }
                   

                }
            }
        }

        if ( controller.alertedByEvent )
        {
            controller.chaseTarget = ReferenceHolder.instance.player.gameObject.transform;
            return true;
        }

        controller.chaseTarget = null;
        return false;
    }
}
