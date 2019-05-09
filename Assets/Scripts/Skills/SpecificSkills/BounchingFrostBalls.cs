using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounchingFrostBalls : MonoBehaviour
{
    public ParticleSystem particle;
    //private GameObject player;
    private ParticleDamage pS;
    private AbilityINFO abilityInfo;

    private float damage;
    private DamageType damageType;
    private float duration;
    
    //private void Awake()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //}
    private void Start()
    {
        particle.Stop();

        setAbilityValues();
        setParticleValues();
        
        Destroy(this.gameObject, (particle.main.startLifetime.constantMax+particle.main.duration)*1.20f);
    }
    void setAbilityValues()
    {
        abilityInfo = GetComponent<AbilityINFO>();
        damage = abilityInfo._damage;
        damageType = abilityInfo._damageType;
        duration = abilityInfo._duration;
    }
    void setParticleValues()
    {
        
        var main = particle.main;
        main.duration = duration;
        particle.Play();

        pS = particle.GetComponent<ParticleDamage>();
        pS.particleDamage = damage;
        pS.damageType = damageType;
        
    }
   
}
