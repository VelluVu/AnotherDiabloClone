using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Perus Hyökkäys action
/// </summary>
[CreateAssetMenu ( menuName = "PluggableAI/Actions/Attack" )]
public class AttackAction : Action
{

    public override void Act ( StateController controller )
    {
        Attack ( controller );
    }

    /// <summary>
    /// Käynnistää hyökkäyksen jos pelaaja on hyökkäys etäisyydellä
    /// </summary>
    /// <param name="controller"></param>
    void Attack ( StateController controller )
    {

        if ( controller.attackRdy)
        {
            controller.Attack ( );
        }
    }
}
