using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorHammer : MonoBehaviour
{
    public GameObject player;     
    public float travelDistance = 8f;
    private float distanceTime;
    private float decreaseVeloctyM = 0.85f;
    private float incresingVeloctyM = 1.2f;
    public float rotateSpeed = 150f;


    float damage;
    DamageType damageType;
    float velocity;
    float defaultVelocity;
    bool hitRightSide = true;
    bool maxDistanceAchived = false;
    bool visited = false;
    int dirValue = 1;

    Vector3 startPos;
    Vector3 handPos;

    AbilityINFO abilityInfo;
    Animator anim;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        anim.SetTrigger("ThorHammerThrow");
        abilityInfo = GetComponent<AbilityINFO>();
        setAbilityValues();
        
        hitRightSide = player.GetComponent<Player>().directionRight;
        if (!hitRightSide)
            dirValue = -1;

        handPos = new Vector3(0.1f * dirValue, -0.2f, 0);
        transform.position = player.transform.position +  handPos;
        startPos = transform.position;

        InvokeRepeating("decreaseVelocity", 0, 0.1f);
    }
    void setAbilityValues()
    {
        damage = abilityInfo._damage;
        damageType = abilityInfo._damageType;
        velocity = abilityInfo._velocity;
        defaultVelocity = velocity;

        //velocity *= ThrowM;
        distanceTime = velocity / travelDistance;
        Debug.Log("Time: " + distanceTime);
    }
    void playCatchAnimation()
    {
        if (!visited)
            anim.SetTrigger("ThorHammerCatch");

        visited = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject.transform.root.gameObject;
        
        if(other.gameObject.CompareTag("Player"))
        {
            
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (go.GetComponent<StateController>() != null)
            {
                go.GetComponent<StateController>().TakeDamage(go, damage, false, damageType, true, 100);
            }
        }
        else if (!go.CompareTag("LootHolder") || !go.CompareTag("Untagged"))
        {
            Debug.Log("TaG: " + go.tag);
            maxDistanceAchived = true;
        }

        //if (go.CompareTag("Wall") || go.CompareTag("Ground"))
        //{
        //    maxDistanceAchived = true;
        //}
    }
    void decreaseVelocity()
    {
        if (maxDistanceAchived)
        {
            //velocity *= (1 - decreaseVeloctyM) + 1;
            velocity *= incresingVeloctyM;
            
        }
        else
        {
            velocity *= decreaseVeloctyM;
        }

        if (velocity < defaultVelocity * 0.15)
            velocity = defaultVelocity * 0.15f;

    }
    void Update()
    {
        

        if (Vector3.Distance(transform.position, startPos) > travelDistance && !maxDistanceAchived)
        {
            
            maxDistanceAchived = true;
        }

        Quaternion targetRotatio;

        if (!maxDistanceAchived)
        {
            targetRotatio= Quaternion.Euler(new Vector3(0, 0, -90) * dirValue);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotatio, rotateSpeed * Time.deltaTime);
            transform.position += new Vector3(velocity * Time.deltaTime, 0, 0) * dirValue;
            //velocity *= 0.98f;
            //if (velocity <= defaultVelocity * 0.1)
            //    velocity = defaultVelocity * 0.1f;
        }
        else
        {
            if(Vector3.Distance(transform.position,player.transform.position) < travelDistance*0.4f)
            {
                targetRotatio = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotatio, 2*rotateSpeed * Time.deltaTime);
                playCatchAnimation();
            }
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position+handPos, velocity * Time.deltaTime);
            //velocity *= 1.02f;

            if (transform.position == player.transform.position+handPos)
                Destroy(this.gameObject);
        }
    }
}
