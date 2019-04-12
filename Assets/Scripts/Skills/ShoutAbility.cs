using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ShoutAbility")]

public class ShoutAbility : Ability
{
    public float _range = 1f;
    public float _duration = 1f;
    public float _heal = 0f;
    public float _damage = 0f;
    public float _damageBuff = 0f;
    public float _speedBuff = 0f;

    public GameObject _prefab;

    private ShoutActivedTrigger shout;

    public override void Initialize(GameObject player)
    {
        shout = player.GetComponent<ShoutActivedTrigger>();
        shout.prefab = _prefab;
        shout.range = _range;
        shout.duration = _duration;
        shout.heal = _heal;
        shout.damage = _damage;
        shout.damageBuff = _damageBuff;
        shout.speedBuff = _speedBuff;
    }

    public override void TriggerAbility()
    {
        shout.Activate();
    }
}
