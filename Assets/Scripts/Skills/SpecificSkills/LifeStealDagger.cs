using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealDagger : MonoBehaviour
{
    public float damage;
    public DamageType damageType;
    public float lifeStealProsent;
    public float rotateSpeed = 150f;

    private bool hitRightSide=true;
    private int dirValue=1;
    private bool stopMoving = false;
    private Vector3 stopPos;

    //private Rigidbody2D rb;
    private AbilityINFO abilityInfo;
    private GameObject player;
    private PlayerClass playerC;
    //private Animator anim;

    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //anim = player.GetComponentInChildren<Animator>();
    }
    void Start()
    {
        setPlayerCanTurn();
        //rb = GetComponent<Rigidbody2D>();
        //anim.SetTrigger("skill_lifeStealDagger");
        abilityInfo = GetComponent<AbilityINFO>();
        playerC = player.GetComponent<PlayerClass>();

        setAbilityValues();
        checkDir();
        transform.position = player.transform.position + new Vector3(0.2f * dirValue, 0.2f, 0);
        Invoke("setPlayerCanTurn", 0.5f);
    }

    void setPlayerCanTurn()
    {
        if(player.GetComponent<Player>().canTurn == false)
        {
            player.GetComponent<Player>().canTurn = true;
        }
        else
        {
            player.GetComponent<Player>().canTurn = false;
        }
    }
    void checkDir()
    {
        hitRightSide = player.GetComponent<Player>().directionRight;
        if (!hitRightSide)
            dirValue = -1;
    }
    void setAbilityValues()
    {
        damage = abilityInfo._damage;
        damageType = abilityInfo._damageType;
        lifeStealProsent = abilityInfo._healingPlayer;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (stopMoving)
            return;
        GameObject go = other.gameObject.transform.root.gameObject;
        if (other.gameObject.CompareTag("Player"))
        {


        }
        else if (go.CompareTag("Enemy"))
        {
            if (go.GetComponent<StateController>() != null)
            {
                go.GetComponent<StateController>().TakeDamage(go, damage, false, damageType, true, 100);
                float maxHP = playerC.maxHealth.BaseValue;
                playerC.RestoreHealth(maxHP * (lifeStealProsent/100));
            }

            Destroy(this.gameObject);

        }
        else if (!go.CompareTag("LootHolder") || !go.CompareTag("Untagged"))
        {
            stopPos = transform.position;
            stopMoving = true;
            Destroy(this.gameObject, 3);
        }
    }

    void Update()
    {
       

        if (stopMoving)
        {
            transform.position = stopPos;
            return;
        }
            
        transform.eulerAngles += new Vector3(0, 0, rotateSpeed * Time.deltaTime) * -1 * dirValue;

    }
}
