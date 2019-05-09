using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public float particleDamage;
    public DamageType damageType;
    public bool timerActive = false;
    public bool isReady = true;

    private ParticleSystem particle;
    private float nextAct = 0f;
    public float period = 0.1f;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        //if(timerActive)
        //{
        //    InvokeRepeating("resetReadyTime", 0, period);
        //}
    }
    void resetReadyTime()
    {
        isReady = true;
    }
    private void OnParticleCollision(GameObject other)
    {
        
        GameObject go = other.gameObject.transform.root.gameObject;
        if (!timerActive)
            isReady = true;
        if(go.CompareTag("Enemy"))
        {           
            if (go.GetComponent<StateController>() != null && isReady)
            {
                go.GetComponent<StateController>().TakeDamage(go, particleDamage, false, damageType, true, 100);
                isReady = false;
                nextAct = Time.time + period;
            }
        }
    }

    private void Update()
    {
        if(Time.time > nextAct)
        {
            isReady = true;
        }
    }





}
