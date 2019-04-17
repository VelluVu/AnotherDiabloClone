using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Abilities/AbilityContainer")]
public class AbilityContainer : ScriptableObject
{
    public string _name = "Ability container";
    public Ability[] activeAbilities;
    public Ability[] allAbilities;
    


}
