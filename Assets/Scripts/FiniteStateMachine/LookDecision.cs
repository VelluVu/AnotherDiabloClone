﻿using System.Collections;
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
        RaycastHit2D hitDown;
    
        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius * 0.1f, controller.eyes.right, controller.spotDistance, controller.playerLayer );
            
            if ( hit.collider != false)
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

            hit = Physics2D.CircleCast ( controller.eyes.position, controller.radius, controller.eyes.right, controller.spotDistance, controller.playerLayer );
            hitDown = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius * 0.1f, -controller.eyes.up, controller.spotDistance, controller.playerLayer );

            if ( hit.collider != false  )
            {
                if ( hit.collider.gameObject.CompareTag ( "Player" ))
                {
                    controller.chaseTarget = hit.transform;
                    return true;
                }
            }

            if ( hitDown.collider != false )
            {
                if ( hitDown.collider.gameObject.CompareTag ( "Player" ) )
                {
                    controller.chaseTarget = hit.transform;
                    return true;
                }
            }      
        }

        if ( controller.alertedByEvent )
        {
            return true;
        }

        return false;
    }
}
