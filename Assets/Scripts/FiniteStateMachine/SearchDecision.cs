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

        if ( controller.enemyStats.enemyType != EnemyType.FlyingEnemy )
        {
            if ( !controller.hasTurn )
            {
                controller.hasTurn = true;

                if ( controller.dirRight )
                {                  
                    controller.rb.velocity = Vector2.left * 4 * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * Time.deltaTime;
                }
                else if ( !controller.dirRight )
                {                
                    controller.rb.velocity = Vector2.right * 4 * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * Time.deltaTime;
                }
                controller.StartCoroutine ( controller.WaitTime ( 1f ) );
            }
        }
        else
        {

            if ( controller.dirRight )
            {

                controller.rb.velocity = new Vector2 ( 0, controller.amplitude * ( Mathf.Sin ( 1 - controller.frequency + Time.time ) * Time.deltaTime ) );            

            }
            else if ( !controller.dirRight )
            {
                controller.rb.velocity = new Vector2 ( 0, controller.amplitude * ( Mathf.Sin ( 1 - controller.frequency + Time.time ) * Time.deltaTime ) );    

            }

        }

        return controller.CheckIfCountDownElapsed ( controller.searchDuration );
    }
}
