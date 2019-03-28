using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Etsii pelaajaa sen jälkeen kun hukkaa kohteen
/// </summary>
[CreateAssetMenu ( menuName = "PluggableAI/Decisions/Scan" )]
public class SearchDecision : Decision
{

    public override bool Decide ( StateController controller )
    {
        bool noEnemyOnSight = Search ( controller );
        return noEnemyOnSight;
    }

    /// <summary>
    /// Etsii pelaajaa olisi hyvä jos liikkuisi lyhyttä matkaa edestakaisin
    /// </summary>
    /// <param name="controller"></param>
    /// <returns></returns>
    private bool Search ( StateController controller )
    {

        RaycastHit2D hit = Physics2D.CircleCast ( controller.eyes.transform.position, controller.radius, controller.eyes.right, controller.enemyLayer );

        if ( hit.collider != false )
        {
            if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.spotDistance )
            {

                
                
            }

        }


        return controller.CheckIfCountDownElapsed ( controller.searchDuration );
    }
}
