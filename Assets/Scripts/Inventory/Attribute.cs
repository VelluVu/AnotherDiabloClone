using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;

[CreateAssetMenu(fileName = "Attribute", menuName = "Loot/Attribute", order = 2)]
public class Attribute: ScriptableObject
{
    [SerializeField]
    public enum Stat // tarkista että on samassa järjesyksessä mikä playerclass.addToList
    {
        MoveSpeed,JumpForce,ExtraJumpForce,BaseDamage,
        BaseAttackSpeed,Health,Mana,Stamina,Armor,
        fireResistance,coldResistance,poisonResistance,
        lightningResistance,Strength,Dexterity,
        Endurance,Energy,physicalResistance,maxHealth

    
    }
    public bool isBaseValue;
    public Stat stat;
    public bool primary;
    public float value; // make into value range
    public StatModType statModType;
    public StatModifier statModifier;
    public CharacterStat characterStat;
    public PlayerClass PC;
   
   
   
    
}
