using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;

public class RollAttribute : MonoBehaviour
{

    public bool isBaseValue;
    public string attributeName;
    public int stat;
    public bool primary;
    public float value; // make into value range
    public StatModType statModType;
    public StatModifier statModifier;
    public CharacterStat characterStat;
    public PlayerClass PC;
    private float tempBaseValue;


    public void CreateStatModifier()
    {
        Debug.Log(stat);
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
        }
        else
        {
            characterStat.RemoveAllModifiersFromSource(this);
        }
      
        PC.checkForChanges();
        
    }
    public void Consume()
    {
        characterStat.AddModifier(statModifier);
        PC.checkForChanges();
    }
    public void EndConsume()
    {
        characterStat.RemoveAllModifiersFromSource(this);
    }
    public RollAttribute rollAttribute (Attribute attribute)
    {
        value = attribute.value;
        stat = (int)attribute.stat;
        primary = attribute.primary;
        statModType = attribute.statModType;
        statModifier = attribute.statModifier;
        characterStat = attribute.characterStat;
        attributeName = attribute.name;
        isBaseValue = attribute.isBaseValue;
        PC = FindObjectOfType<PlayerClass>();
        CreateStatModifier();
        return this;
    }
    public string createText()
    {
        string returnText = attributeName + ": " + "<color=yellow>"+value.ToString()+"</color>";

        return returnText;
    }

}
