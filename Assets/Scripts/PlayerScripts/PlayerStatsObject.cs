﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Classes/PlayerStats/Class")]
public class PlayerStatsObject : ScriptableObject
{

    public string _name = "Jesse";
    public string _className = "Mercenary";
    public ClassType _classType;

    [Header ("Player Stats")]
    public float _moveSpeed;
    public float _jumpForce;
    public float _extraJumpForce;
    public float _extraWallJumpForce;

    public float _baseDamage;
    public float _baseAttackSpeed;
    public float _maxHealth;
    public float _health;
    public float _maxMana;
    public float _mana;
    public float _stamina; //rajoittaa liikkumista jotenkin?
    public float _armor;
    public float _cooldownReduction;
    public float _criticalHitChance;
    public float _criticalHitDamage;
    public float _fireResistance;
    public float _coldResistance;
    public float _poisonResistance;
    public float _lightningResistance;
    public float _physicalResistance;
    public float _allResistance;
    public float _goldFind;
    public float _magicFind;
    public float _pickupRadius;
    public float _healthRegeneration;
    public float _manaCostReduction;
    public float _meleeDamageReduction;
    public float _rangedDamageReduction;
    public float _healthOnKill;
    public float _block;
    public float _fallDamageReduction;
    public float _experienceOnKill;
    public float _experienceBonus;

    public int _strength;
    public int _dexterity;
    public int _endurance;
    public int _energy;

}
