using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ProjectileAbility")]

public class ProjectileAbility : Ability
{
    public float _protectileForce = 100f;
    public float _damange = 1f;
    public float _mass = 1f;
    public float _healingEnemy=0f;
    public float _healingPlayer = 0f;
    public float manaUsage = 0f;

    public Rigidbody2D _rb2D;

    private ProjectileShootTrigger shootIt;

    public override void Initialize(GameObject player)
    {
        shootIt = player.GetComponent<ProjectileShootTrigger>();
        //shootIt.projectileForce = _protectileForce;
        //shootIt.rb2D = _rb2D;
        //shootIt.damage = _damange;
        //shootIt.mass = _mass;
        //shootIt.healingEnemy = _healingEnemy;
        //shootIt.healingPlayer = _healingPlayer;
        //shootIt.manaUsage = manaUsage;
    }

    public override void TriggerAbility(Ability ability)
    {
        shootIt.Launch(ability);
    }

}
