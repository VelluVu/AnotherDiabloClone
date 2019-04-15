using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;

[CreateAssetMenu(fileName = "Attribute", menuName = "Loot/Attribute", order = 2)]
public class Attribute: ScriptableObject
{
    public bool rollValueByItemLevels = false;
    public string attributeName;
    public bool isBaseValue;
    public Stat stat;
    public bool primary;
    public float value; 
    public float valueMax = 0;
    public float value2Min = 0;
    public float value2Max = 0;
    public StatModType statModType;
    public StatModifier statModifier;
    [HideInInspector] public CharacterStat characterStat;
    
    public int id = 0;
   
   
    
}

