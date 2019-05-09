using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProjectileShootTrigger : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public float projectileForce;
    [HideInInspector] public float damage;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public float duration;
    [HideInInspector] public float mass;
    [HideInInspector] public float healingEnemy;
    [HideInInspector] public float healingPlayer;
    [HideInInspector] public float manaUsage;
    [HideInInspector] public SkillSoundType SkillSoundType;

    [HideInInspector] public float waitBeforeLaunch;
    [HideInInspector] public string _animation;
    [HideInInspector] public bool playItBeforeWait;

    [HideInInspector] public bool stopPlayer;
    [HideInInspector] public float stopPlayerDuration;

    public AudioMixerGroup audioMixerGroup;
    private Transform spawn;
    private Vector2 dir;
    private Animator anim;
    private Player p;

    public delegate void AbilitySoundDelegate(AudioSource source, SkillSoundType skillSound);
    public static event AbilitySoundDelegate AbilitySoundEventA;

    private void Awake()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
        p = GetComponent<Player>();
    }

    public void setValues(ProjectileAbility skill)
    {
        rb2D = skill._rb2D;
        projectileForce = skill._protectileForce;
        damage = skill._damage;
        mass = skill._mass;
        healingPlayer = skill._healingPlayer;
        healingEnemy = skill._healingEnemy;
        manaUsage = skill._manaUsage;
        damageType = skill._damageType;
        duration = skill._duration;
        waitBeforeLaunch = skill._waitBeforeLaunch;
        _animation = skill._animation;
        playItBeforeWait = skill._playItBeforeWait;
        stopPlayer = skill._stopPlayerMoving;
        stopPlayerDuration = skill._stopDuration;
        SkillSoundType = skill.skillSoundType;
        

    }

    public void Launch(Ability skill)
    {
        if(skill is ProjectileAbility)
        {
            setValues(skill as ProjectileAbility);
        }
        else
        {
            Debug.Log("ABILITY wasn't protectile ability");
            return;
        }
        spawn = transform;
        // tästä saadaan hiiren osoittama suunta ja laukstaan se siihe suuntaan
        dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - spawn.position;
        if(playItBeforeWait )
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

        Rigidbody2D clonedProjectile = Instantiate(rb2D, spawn.position, Quaternion.identity) as Rigidbody2D;

        AbilityINFO info = clonedProjectile.gameObject.AddComponent<AbilityINFO>();
        clonedProjectile.mass = mass;
        info._damage = damage;
        info._direction = dir;
        info._spawnPos = spawn.position;
        info._healingEnemy = healingEnemy;
        info._healingPlayer = healingPlayer;
        info._duration = duration;
        info._damageType = damageType;

        AudioSource source = clonedProjectile.gameObject.AddComponent<AudioSource>();
        source.volume = 0.1f;
        source.outputAudioMixerGroup = audioMixerGroup;

        if (AbilitySoundEventA != null)
        {
            AbilitySoundEventA(source, SkillSoundType);
        }


        clonedProjectile.AddForce(dir.normalized * projectileForce);

        yield return null;
    }
}
