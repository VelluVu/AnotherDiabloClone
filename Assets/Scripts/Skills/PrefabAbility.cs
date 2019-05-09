using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PrefabAbility")]
public class PrefabAbility : Ability
{
    public GameObject prefab;
    public float damageProsent;
    public DamageType damageType;
    public float force;
    public float velocity;

    public float _waitBeforeLaunch = 0f;
    public string _animation;
    public bool _playItBeforeWait = true;

    private PrefabAbilityTrigger prefabAbilityTrigger;

    public override void Initialize(GameObject player)
    {
        prefabAbilityTrigger = player.GetComponent<PrefabAbilityTrigger>();
    }

    public override void TriggerAbility(Ability skill)
    {
        prefabAbilityTrigger.Active(skill);
    }
}
