using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Classes/PlayerStats/Class")]
public class PlayerStatsObject : ScriptableObject
{

    public string _name = "Jesse";
    public string _className = "JumalanPoeka";
    public ClassType _classType;
    public float _moveSpeed;
    public float _jumpForce;
    public float _extraJumpForce;
    public float _baseDamage;
    public float _baseAttackSpeed;
    public float _maxHealth;
    public float _health;
    public float _maxMana;
    public float _mana;
    public float _stamina; //rajoittaa liikkumista jotenkin?
    public float _armor;
    public float _criticalHitChance;
    public float _criticalHitDamage;
    public float _fireResistance;
    public float _coldResistance;
    public float _poisonResistance;
    public float _lightningResistance;
    public float _physicalResistance;

    public int _strength;
    public int _dexterity;
    public int _endurance;
    public int _energy;

}
