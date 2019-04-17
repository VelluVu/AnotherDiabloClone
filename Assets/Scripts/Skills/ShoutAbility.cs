using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ShoutAbility")]

public class ShoutAbility : Ability
{
    //[Header("This adds script to the target")]
    public float _range = 0f;
    public float _duration = 1f;
    public float _heal = 0f;
    public float _damage = 0f;
    public float _increaseDamage = 0;
    public float _dotDamage = 0f;
    public float _speedBuff = 0f;
    public GameObject _spawnGameobject = null;

    public bool _fearEnemy = false;

    private ShoutActivedTrigger shout;

    public override void Initialize(GameObject player)
    {
        shout = player.GetComponent<ShoutActivedTrigger>();
        //shout.prefab = _prefab;
        //shout.castToHolder = _castToHolder;

        //shout.fearEnemy = _fearEnemy;
        //shout.range = _range;
        //shout.duration = _duration;
        //shout.heal = _heal;
        //shout.damage = _damage;
        //shout.dotDamage = _dotDamage;
        //shout.speedBuff = _speedBuff;
        //shout.manaUsage = _manaUsage;
    }

    public override void TriggerAbility(Ability skill)
    {       
        shout.Activate(skill);       
    }
}
