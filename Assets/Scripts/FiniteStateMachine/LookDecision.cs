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

        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius * 0.1f, controller.eyes.right, controller.spotDistance, controller.playerLayer );

            if ( hit.collider != false )
            {
                if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.spotDistance )
                {

                    controller.chaseTarget = hit.transform;
                    return true;

                }
            }

            return false;
        }
        else
        {

            hit = Physics2D.CircleCast ( controller.eyes.position, controller.radius, controller.eyes.right, controller.spotDistance, controller.playerLayer );
            Debug.DrawRay ( controller.eyes.position, controller.eyes.right * controller.spotDistance, Color.red );
            Debug.Log ( "Look Decision HIT : " + hit.transform );

            if ( hit.collider != false )
            {
                if ( hit.collider.gameObject.CompareTag ( "Player" ) )
                {
                    controller.chaseTarget = hit.transform;
                    return true;
                }
            }


            return false;

        }
    }
}
