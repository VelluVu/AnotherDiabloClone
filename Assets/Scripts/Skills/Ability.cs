using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string _name = "New Skill";
    [TextArea(3,20)]
    public string desc = "Give desc";
    public bool isEnabled = true;
    public bool checkLevel = true;
    public int levelToUnlock = 0;
    public Sprite _sprite;
    //public AudioClip _sound;
    //public AudioSource skillSoundSource;
    public SkillSoundType skillSoundType;
    public float _cooldown = 0;
    public float _manaUsage = 0;
    public float _duration = 0;
    public bool _showDurBar = true;
    public bool _allowDoThisInAir = true;
    public bool _stopPlayerMoving = false;
    public float _stopDuration = 0f;
    //public GameObject caster;

    public abstract void Initialize(GameObject player);
    public abstract void TriggerAbility(Ability skill);
    
}
