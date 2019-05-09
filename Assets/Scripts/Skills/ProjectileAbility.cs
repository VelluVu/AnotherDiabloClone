using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ProjectileAbility")]

public class ProjectileAbility : Ability
{
    public float _protectileForce = 100f;
    public float _damage = 1f;
    public DamageType _damageType;
    public float _mass = 1f;
    public float _healingEnemy=0f;
    public float _healingPlayer = 0f;
   

    public Rigidbody2D _rb2D;

    public float _waitBeforeLaunch = 0f;
    public string _animation;
    public bool _playItBeforeWait = true;


    private ProjectileShootTrigger shootIt;

    public override void Initialize(GameObject player)
    {
        shootIt = player.GetComponent<ProjectileShootTrigger>();
    }

    public override void TriggerAbility(Ability ability)
    {
        if (!isEnabled)
            return;
        shootIt.Launch(ability);
    }

    

}
