using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidShield : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem particle;
    public float particleDamageProsent = 10;
    public float travelDistance = 4f;
    public float comingBackSpeedMult = 3f;
    public float rotateSpeed = 200f;
    

    float damage;
    DamageType damageType;
    float velocity;
    bool hitRightSide = true;    
    bool maxDistanceAchived = false;
    int dirValue = 1;

    Vector3 startPos;

    AbilityINFO abilityInfo;
    ParticleDamage psDamageScript;
    Animator anim;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        anim.SetTrigger("skill_voidShield");
        psDamageScript = particle.GetComponent<ParticleDamage>();
        abilityInfo = GetComponent<AbilityINFO>();
        setAbilityValues();
        hitRightSide = player.GetComponent<Player>().directionRight;
        if (!hitRightSide)
            dirValue = -1;
        startPos = transform.position;
    }
    void setAbilityValues()
    {
        damage = abilityInfo._damage;
        damageType = abilityInfo._damageType;
        velocity = abilityInfo._velocity;

        //particle
        psDamageScript.damageType = damageType;
        psDamageScript.particleDamage = damage * (particleDamageProsent / 100);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject.transform.root.gameObject;
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (go.GetComponent<StateController>() != null)
            {
                go.GetComponent<StateController>().TakeDamage(go, damage, false, damageType, true, 100);
            }
        }

        if(go.CompareTag("Wall") || go.CompareTag("Ground"))
        {
            maxDistanceAchived = true;
        }
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, startPos) > travelDistance && !maxDistanceAchived)
        {
            maxDistanceAchived = true;
        }

        if (!maxDistanceAchived)
        {
            transform.position += new Vector3(velocity * Time.deltaTime, 0, 0) * dirValue;
            transform.eulerAngles += new Vector3(0, 0, rotateSpeed * Time.deltaTime) * -1 * dirValue;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, velocity * Time.deltaTime * comingBackSpeedMult);
            transform.eulerAngles += new Vector3(0, 0, rotateSpeed * Time.deltaTime) * dirValue;
            velocity *= 1.001f;
            if (transform.position == player.transform.position)
                Destroy(this.gameObject);
        }
       
    }
}
