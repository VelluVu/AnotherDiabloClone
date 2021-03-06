﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;

public class RollAttribute : MonoBehaviour
{

    public RolledLoot rolledLoot;
    public bool RollValueByItemLevels;
    public bool isBaseValue;
    public string attributeName;
    public int stat;
    public bool primary;
    public float value; // make into value range
    public float valueMax = 0;
    public float value2Min = 0;
    public float value2Max = 0;
    public Stat theStat;
    public StatModType statModType;
    public StatModifier statModifier;
    public CharacterStat characterStat;
    public PlayerClass PC;
    public Attribute originAttribute;
    private float tempBaseValue;
    private float tempBaseValueMax;


    public void CreateStatModifier()
    {
        
        if (originAttribute.stat == Stat.Armor)
        {
            value *= PlayerInventory.instance.armorSlotArmorModifier[rolledLoot.armorSlot];
           
        }
        Debug.Log(statModType);
        statModifier = new StatModifier(value, statModType, this);
        characterStat = PC.listCharacterStats[stat];
        string text = createText();
        Debug.Log(text);

    }
    public void Equip()
    {
        
        if (isBaseValue)
        {
           
            tempBaseValue = characterStat.BaseValue;
            
            characterStat.BaseValue = value;
            if (theStat == Stat.BaseDamage)
            {
                   tempBaseValueMax = PC.baseDamageMax.BaseValue;
                   PC.baseDamageMax.BaseValue = value2Min;
                
            }

        }
        else
        {
          
            CreateStatModifier();
            characterStat.AddModifier(new StatModifier(value, statModType, this));
           
        }
        PC.checkForChanges();


    }
    public void Unequip()
    {

        if (isBaseValue)
        {
            characterStat.BaseValue = tempBaseValue;
            if (theStat == Stat.BaseDamage)
            {
                PC.baseDamageMax.BaseValue = tempBaseValueMax ;
                

            }
        }
        else
        {
            characterStat.RemoveAllModifiersFromSource(this);
        }
      
        PC.checkForChanges();
        
    }
    public void Consume()
    {
        if (rolledLoot.tags.Contains(Tags.HealingPotion))
        {
            //FindObjectOfType<Player>().OnRestoreHealth(0, true);
            FindObjectOfType<Player>().OnRestoreHealth(value, false);
        }
        else if (rolledLoot.tags.Contains(Tags.ManaPotion))
        {
            FindObjectOfType<Player>().OnRestoreMana(value, false);
        }
       

        //characterStat.AddModifier(statModifier);
        //PC.checkForChanges();
    }
    public void EndConsume()
    {
        characterStat.RemoveAllModifiersFromSource(this);
    }
    public RollAttribute rollAttribute (Attribute attribute)
    {
        value = attribute.value;
        stat = (int)attribute.stat;
        theStat = attribute.stat;
        primary = attribute.primary;
        statModType = attribute.statModType;
        statModifier = attribute.statModifier;
        characterStat = attribute.characterStat;
        attributeName = attribute.attributeName;
        isBaseValue = attribute.isBaseValue;
        valueMax = attribute.valueMax;
        value2Min = attribute.value2Min;
        value2Max = attribute.value2Max;
        RollValueByItemLevels = attribute.rollValueByItemLevels;
        originAttribute = attribute;
        PC = PlayerClass.instance;
        CreateStatModifier();
        return this;
    }
    public string createText()
    {
        string returnText = "";
        string format = "F0";
        if(theStat == Stat.BaseAttackSpeed)
        {
            format = "F1";
            float displayValue = -value * 100;
            Mathf.Round(displayValue);
            
            returnText = "+" + "<color=yellow>" + displayValue.ToString(format) + "%</color> " + attributeName;
            return returnText;
        }
        if(theStat == Stat.criticalHitChance || theStat == Stat.criticalHitDamage || theStat == Stat.cooldownReduction || theStat == Stat.experienceBonus)
        {
            returnText = "+" + "<color=yellow>" + value.ToString(format) + "%</color> " + attributeName;
            
        }
        else if(statModType == StatModType.Flat)
        {
        
            if (theStat == Stat.BaseDamage)
            {
                returnText = "+" + "<color=yellow>" + value.ToString(format) + "-" +value2Min.ToString(format) +"</color> " + attributeName;
            }
            else
            {
                returnText = "+" + "<color=yellow>" + value.ToString(format) + "</color> " + attributeName;
            }
            
        }
        else if(statModType == StatModType.PercentAdd)
        {
            returnText = "+" + "<color=yellow>" + value.ToString(format) + "%</color> " + attributeName;
        }


        return returnText;
    }

}
