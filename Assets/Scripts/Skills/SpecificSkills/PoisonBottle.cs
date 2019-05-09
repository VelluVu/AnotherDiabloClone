using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBottle : MonoBehaviour
{
    public GameObject poisonCloud;
    public float damage;
    public DamageType damageType;
    public float duration;
    
    AbilityINFO abilityInfo;

    bool used = false;
    bool directionRight;
    float spinSpeed = 500;
    GameObject player;
    Animator anim;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        anim.SetTrigger("Throw3");
        directionRight = player.GetComponent<Player>().directionRight;
        setSpawnPoint();
        setAbilityValues();
        Invoke("releaseTheSmoke", 10);
    }

    void setAbilityValues()
    {
        abilityInfo = GetComponent<AbilityINFO>();
        damage = abilityInfo._damage;
        damageType = abilityInfo._damageType;
        duration = abilityInfo._duration;
    }

    void setSpawnPoint()
    {
        Vector3 pos = new Vector3(0.2f, 0.2f);
        if (!directionRight)
            pos.x *= -1;

        transform.position = pos + player.transform.position;
    }
    void spin()
    {
        int dir = 1;
        if (directionRight)
            dir = -1;

        transform.Rotate(new Vector3(0, 0, dir * spinSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D c)
    {        
        if(c.gameObject.CompareTag("Ground") && !used)
        {            
            used = true;
            releaseTheSmoke();
        }
    }

    void releaseTheSmoke()
    {
        GameObject spawned = Instantiate(poisonCloud, transform.position, Quaternion.identity);
        SmokeCloud sc = spawned.GetComponent<SmokeCloud>();
        sc.damage = damage;
        sc.damageType = damageType;
        sc.durantion = duration;

        Destroy(this.gameObject);
    }
    private void Update()
    {
        spin();
    }
}
