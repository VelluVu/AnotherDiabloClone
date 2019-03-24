using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Actions/Attack" )]
public class AttackAction : Action
{

    public override void Act ( StateController controller )
    {
        Attack ( controller );
    }

    void Attack(StateController controller)
    {

        RaycastHit2D hit;
       
        hit = Physics2D.Raycast ( controller.eyes.transform.position, controller.eyes.right );
       
        if ( hit.collider != false )
        {
            if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance )
            {
                if ( controller.CheckIfCountDownElapsed ( controller.attackCD ) )
                {

                    controller.Attack ( );

                }
                else
                {
                    float rng = Random.Range ( -2, 2 );
                    Vector2 pos = new Vector2 ( controller.transform.position.x, controller.transform.position.y );
                    Vector2 target = new Vector2 ( controller.transform.position.x, controller.transform.position.y * rng );

                    Vector2.Lerp ( pos, target, 1f );
                }
            }
        }
        else
        {
            float rng = Random.Range ( -2, 2 );
            Vector2 pos = new Vector2(controller.transform.position.x, controller.transform.position.y);
            Vector2 target = new Vector2 ( controller.transform.position.x, controller.transform.position.y * rng);
            
            Vector2.Lerp (pos, target , 1f );
        }
    }
}
