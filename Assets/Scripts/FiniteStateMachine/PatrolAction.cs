using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void MoveForward ( StateController controller )
    {

        if ( controller.dirRight )
        {

            controller.rb.AddForce ( controller.pos * controller.enemyStats.moveSpeed * controller.enemyStats.moveSpeed * controller.moveSpeedScale );
        }
        else
        {

            controller.rb.AddForce ( controller.pos * controller.enemyStats.moveSpeed * controller.enemyStats.moveSpeed * -controller.moveSpeedScale );
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
