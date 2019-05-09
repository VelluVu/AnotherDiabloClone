using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ShoutAbility")]

public class ShoutAbility : Ability
{
    //[Header("This adds script to the target")]
    public float _range = 0f;    
    public float _heal = 0f;
    public float _mana = 0f;
    public float _damage = 0f;
    public DamageType _damageType;
    public float _dotDamage = 0f;
    public float _speedBuff = 0f;
    public bool _playShoutAnimation = true;

    public float _increaseStrM = 0;
    public float _increaseArmorM = 0;
    public float _increaseAttactSpeedM = 0;
    public float _increaseHealthM = 0;
    

    public GameObject _spawnGameobject = null;

    public bool _fearEnemy = false;
    public bool _stunEnemy = false;
    public bool _weakEnemy = false;

    public bool _addShoutScript = true;
    public bool _addSStoPrefab = false;
    public bool _addAbilityInfoScript = false;
    public bool _addAIStoPrefab = false;

    public float _waitBeforeLaunch = 0f;
    public string _animation;
    public bool _playItBeforeWait = true;

    private ShoutActivedTrigger shout;

    public override void Initialize(GameObject player)
    {
        shout = player.GetComponent<ShoutActivedTrigger>();
        
    }

    public override void TriggerAbility(Ability skill)
    {       
        shout.Activate(skill);       
    }
}
