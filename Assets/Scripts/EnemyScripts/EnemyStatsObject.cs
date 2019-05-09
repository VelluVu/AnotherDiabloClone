using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Enemies/EnemyStats/Enemy")]
public class EnemyStatsObject : ScriptableObject
{
    public string _name = "Monster";
    public float _moveSpeed;
    public float _chaseMultiplier;
    public float _jumpForce;

    public float _attackSpeed;
    public float _attackDamage;

    public float _health;
    public float _mana;

    public float _armor;

    public float _fireResistance;
    public float _frostResistance;
    public float _poisonResistance;
    public float _lightningResistance;
    public float _physicalResistance;

    public int _xpReward;
    public int _level;

    public EnemyTypeForSound _enemyTypeForSound;
    public EnemyType _enemyType;
    public GroundEnemyType _groundEnemyType;
    public FlyingEnemyType _flyingEnemyType;
    public BossEnemyType _bossEnemyType;
    public EnemyVariations[] _enemyVariation;

}


