using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Patrollaus Action Class
/// </summary>
[CreateAssetMenu ( menuName = "PluggableAI/Actions/Patrol" )]
public class PatrolAction : Action
{
    public float pauseTime;
    public float timeToBreak;

    public override void Act ( StateController controller )
    {
        Patrol ( controller );
    }

    void Patrol ( StateController controller )
    {

        MoveForward ( controller );
        //SpotOpponent ( controller );
        //TurnAround ( controller );

    }

    /// <summary>
    /// Liikkuu ees taas...
    /// </summary>
    /// <param name="controller"></param>
    void MoveForward ( StateController controller )
    {

        if ( controller.dirRight )
        {

            controller.rb.velocity = new Vector2 (controller.targetDir.x * controller.enemyStats.moveSpeed * controller.moveSpeedScale * Time.deltaTime, controller.rb.velocity.y);
        }
        else
        {

            controller.rb.velocity = new Vector2 ( controller.targetDir.x * controller.enemyStats.moveSpeed  * -controller.moveSpeedScale * Time.deltaTime, controller.rb.velocity.y );
        }     
    }

    void SpotOpponent ( StateController controller )
    {


        controller.gaze = Physics2D.Raycast ( controller.eyes.transform.position, Vector2.right );



        if ( controller.gaze.collider != false )
        {
            if ( controller.gaze.collider.gameObject.CompareTag ( "Player" ) )
            {
                if ( controller.gaze.distance <= controller.spotDistance )
                {
                    Debug.Log ( "SPOTS PLAYER" );
                    //Spots Player
                    //Move To ChaseState

                }
            }
        }
    }
}
