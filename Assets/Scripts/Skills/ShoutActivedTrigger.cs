using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ShoutActivedTrigger : MonoBehaviour
{
    //[HideInInspector] public GameObject prefab;
    [HideInInspector] public float range;
    [HideInInspector] public float duration;
    [HideInInspector] public float heal;
    [HideInInspector] public float mana;
    [HideInInspector] public float damage;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public float dotDamage;
    [HideInInspector] public float speedBuff;
    [HideInInspector] public float manaUsage;
    [HideInInspector] public GameObject spawneGameobj;
    [HideInInspector] public bool fearEnemy;
    [HideInInspector] public bool stunEnemy;
    [HideInInspector] public bool weakEnemy;
    [HideInInspector] public bool playShoutAnimation;
    [HideInInspector] public SkillSoundType SkillSoundType;

    [HideInInspector] public float increaseStrM;
    [HideInInspector] public float increaseArmorM;
    [HideInInspector] public float increaseAttactSpeedM;
    [HideInInspector] public float increaseHealthM;

    [HideInInspector] public bool addShoutScript;
    [HideInInspector] public bool addAbilityInfoScript;
    [HideInInspector] public bool addSStoPrefab;
    [HideInInspector] public bool addAIStoPrefab;

    [HideInInspector] public float waitBeforeLaunch;
    [HideInInspector] public string _animation;
    [HideInInspector] public bool playItBeforeWait;
    [HideInInspector] public bool stopPlayer;
    [HideInInspector] public float stopPlayerDuration;

    //[HideInInspector] public bool castToHolder;
    public AudioMixerGroup audioMixerGroup;
    private ShoutScript sS;
    private AbilityINFO info;
    private Animator anim;
    private Player p;

    public delegate void AbilitySoundDelegate(AudioSource source, SkillSoundType skillSound);
    public static event AbilitySoundDelegate AbilitySoundEventB;

    private void Awake()
    {
        anim=transform.root.GetComponentInChildren<Animator>();
        p = GetComponent<Player>();
    }

    public void setAbilityInfo(AbilityINFO abilityINFO)
    {
        abilityINFO._damage = damage;
        abilityINFO._damageType = damageType;
        abilityINFO._duration = duration;
        abilityINFO._heal = heal;
        abilityINFO._mana = mana;

        abilityINFO._increaseArmor = increaseArmorM;
        abilityINFO._increaseStrengh = increaseStrM;
        abilityINFO._increaseAttackSpeed = increaseAttactSpeedM;
        abilityINFO._increaseHealth = increaseHealthM;
        
    }

    public void setValues(ShoutAbility skill)
    {
        
        range = skill._range;
        duration = skill._duration;
        heal = skill._heal;
        mana = skill._mana;
        damage = skill._damage;
        dotDamage = skill._dotDamage;
        speedBuff = skill._speedBuff;
        manaUsage = skill._manaUsage;
        fearEnemy = skill._fearEnemy;
        stunEnemy = skill._stunEnemy;
        weakEnemy = skill._weakEnemy;
        spawneGameobj = skill._spawnGameobject;
        damageType = skill._damageType;
        playShoutAnimation = skill._playShoutAnimation;

        increaseStrM = skill._increaseStrM;
        increaseArmorM = skill._increaseArmorM;
        increaseAttactSpeedM = skill._increaseAttactSpeedM;
        increaseHealthM = skill._increaseHealthM;

        addAbilityInfoScript = skill._addAbilityInfoScript;
        addShoutScript = skill._addShoutScript;
        addSStoPrefab = skill._addSStoPrefab;
        addAIStoPrefab = skill._addAIStoPrefab;

        waitBeforeLaunch = skill._waitBeforeLaunch;
        _animation = skill._animation;
        playItBeforeWait = skill._playItBeforeWait;

        stopPlayer = skill._stopPlayerMoving;
        stopPlayerDuration = skill._stopDuration;
        SkillSoundType = skill.skillSoundType;

    }

    public void Activate(Ability skill)
    {
        ShoutAbility shout;
        if(skill is ShoutAbility)
        {
            shout = skill as ShoutAbility;
            setValues(shout);
        }
        else
        {           
            return;
        }

        if (playItBeforeWait)
        {

            setAnimationAndThings();
            
        }

        StartCoroutine(go());
        
    }

    void setAnimationAndThings()
    {
        if (_animation.Length > 0)
        {
            anim.SetTrigger(_animation);
        }
        if (stopPlayer)
        {
            p.SetStun(stopPlayerDuration);
        }
    }
    IEnumerator go()
    {
        yield return new WaitForSecondsRealtime(waitBeforeLaunch);

        if (!playItBeforeWait)
        {
            setAnimationAndThings();
        }

        if (this.gameObject.GetComponent<Player>() != null && manaUsage > 0)
        {
            this.gameObject.GetComponent<Player>().OnManaUse(manaUsage);
        }

        if (playShoutAnimation)
            anim.SetTrigger("Shout");

        AudioSource source;
        if (spawneGameobj != null)
        {
            spawneGameobj = Instantiate(spawneGameobj, transform.position, Quaternion.identity);
            source = spawneGameobj.AddComponent<AudioSource>();
            source.volume = 0.1f;
        }
        else
        {
            source = GetComponent<AudioSource>();
        }

        source.outputAudioMixerGroup = audioMixerGroup;

        if (AbilitySoundEventB != null)
        {
            AbilitySoundEventB(source, SkillSoundType);
        }

        if (addShoutScript)
        {
            int count = 1;
            if (addSStoPrefab)
            {
                count = 2;
            }
            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    sS = this.gameObject.AddComponent<ShoutScript>();

                }
                else if (i == 1)
                {
                    sS = spawneGameobj.AddComponent<ShoutScript>();
                }

                sS.range = range;
                sS.duration = duration;
                sS.heal = heal;
                sS.mana = mana;
                sS.damage = damage;
                sS.damageType = damageType;
                sS.dotDamage = dotDamage;
                sS.speedBuff = speedBuff;
                sS.fearEnemy = fearEnemy;
                sS.stunEnemy = stunEnemy;
                sS.weakEnemy = weakEnemy;

                sS.increaseStrM = increaseStrM;
                sS.increaseArmorM = increaseArmorM;
                sS.increaseAttactSpeedM = increaseAttactSpeedM;
                sS.increaseHealthM = increaseHealthM;
            }
        }

        if (addAbilityInfoScript)
        {
            if (addAIStoPrefab)
            {
                info = spawneGameobj.AddComponent<AbilityINFO>();
            }
            else
            {
                info = this.gameObject.AddComponent<AbilityINFO>();
            }
            setAbilityInfo(info);
        }

        yield return null;
    }
    
}
