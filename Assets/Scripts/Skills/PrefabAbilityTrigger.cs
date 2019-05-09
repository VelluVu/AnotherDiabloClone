using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PrefabAbilityTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject prefab;
    [HideInInspector] public float damageProsent;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public float manaUsage;
    [HideInInspector] public float duration;
    [HideInInspector] public float force;
    [HideInInspector] public float velocity;
    [HideInInspector] public SkillSoundType SkillSoundType;


    [HideInInspector] public float waitBeforeLaunch;
    [HideInInspector] public string _animation;
    [HideInInspector] public bool playItBeforeWait;

    [HideInInspector] public bool stopPlayer;
    [HideInInspector] public float stopPlayerDuration;

    public AudioMixerGroup audioMixerGroup;
    private AbilityINFO abInfo;
    private Animator anim;
    private Player p;
    

    public delegate void AbilitySoundDelegate(AudioSource source, SkillSoundType skillSound);
    public static event AbilitySoundDelegate AbilitySoundEvent;

    private void Awake()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
        p = GetComponent<Player>();
    }

    public void setAbilityInfo(AbilityINFO abilityINFO)
    {
        abilityINFO._damage = damageProsent;
        abilityINFO._damageType = damageType;
        abilityINFO._force = force;
        abilityINFO._velocity = velocity;
        abilityINFO._duration = duration;
    }
    public void setValues(PrefabAbility prefabAbility)
    {
        prefab = prefabAbility.prefab;
        damageProsent = prefabAbility.damageProsent;
        damageType = prefabAbility.damageType;
        manaUsage = prefabAbility._manaUsage;
        force = prefabAbility.force;
        velocity = prefabAbility.velocity;
        duration = prefabAbility._duration;

        waitBeforeLaunch = prefabAbility._waitBeforeLaunch;
        _animation = prefabAbility._animation;
        playItBeforeWait = prefabAbility._playItBeforeWait;

        stopPlayer = prefabAbility._stopPlayerMoving;
        stopPlayerDuration = prefabAbility._stopDuration;
        SkillSoundType = prefabAbility.skillSoundType;
    }

    public void Active(Ability ability)
    {
        if(ability is PrefabAbility)
        {
            setValues(ability as PrefabAbility);
        }
        else
        {
            Debug.Log("Ability wasn't prefabability");
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
        if(_animation.Length > 0)
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

        GameObject spawnObj = Instantiate(prefab, transform.position, Quaternion.identity);
        abInfo = spawnObj.AddComponent<AbilityINFO>();
        AudioSource source=spawnObj.AddComponent<AudioSource>();
        source.volume = 0.1f;
        source.outputAudioMixerGroup = audioMixerGroup;

        if (AbilitySoundEvent != null)
        {
            AbilitySoundEvent(source, SkillSoundType);
        }

        setAbilityInfo(abInfo);

        yield return null;
    }
}
