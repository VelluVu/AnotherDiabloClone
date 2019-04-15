using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string _name = "New Skill";
   
    public Sprite _sprite;
    public AudioClip _sound;
    public float _cooldown=0;
    public float _manaUsage = 0;

    public abstract void Initialize(GameObject player);
    public abstract void TriggerAbility();
    
}
