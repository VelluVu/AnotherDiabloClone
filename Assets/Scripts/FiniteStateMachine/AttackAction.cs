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

    void Attack ( StateController controller )
    {

        RaycastHit2D hit;

        hit = Physics2D.Raycast ( controller.eyes.transform.position, controller.eyes.right );

        if ( hit.collider != false )
        {
            if ( hit.collider.gameObject.CompareTag ( "Player" ) && hit.distance <= controller.attackDistance && controller.attackRdy)
            {

                controller.Attack ( );

            }
        }
    }
}
