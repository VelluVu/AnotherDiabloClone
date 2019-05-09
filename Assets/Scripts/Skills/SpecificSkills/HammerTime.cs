using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerTime : MonoBehaviour
{
    public GameObject player;
    public GameObject head;
    public float spinSpeed = -10f;
    public float hammerLifetime;
    public float dmg;
    public DamageType damageType;
    public float particleDamageProsent = 1f;

    public AbilityINFO abilityInfo;

    public ParticleSystem particle;
    public LightingSource2D ls;
    public DemoLightFlicker flicker;
    private ParticleDamage psDamageScript;
    private Animator anim;
   
    private float startAngle = 30f;
    private bool hitRightSide = true;
    private float rotated = 0;
    private float maxRotate = 130;
    private int x = 0;
    private float timer = 0;
    private int counter = 0;
    private float durationOfParticle;
    [HideInInspector]public List<Vector3> paths;
    [HideInInspector] public Vector3 headPos;

    private bool doNotRotate = false;
    private int angleY = 0;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponentInChildren<Animator>();
        ls = particle.GetComponent<LightingSource2D>();
        flicker = particle.GetComponent<DemoLightFlicker>();
        
    }
    void Start()
    {
        player.GetComponent<Player>().canTurn = false;
        psDamageScript = particle.GetComponent<ParticleDamage>();
        abilityInfo = GetComponent<AbilityINFO>();
        setAbilityValues();
        hitRightSide = player.GetComponent<Player>().directionRight;
        anim.SetTrigger("HammerTime");
        


        foreach (Transform child in player.GetComponentsInChildren<Transform>())
        {
            if (child.name == "Head")
            {
                head = child.gameObject;
                headPos = child.transform.position;
                break;
            }
        }
        
        if (!hitRightSide)
        {
            
            //startAngle *= -1;
            //spinSpeed *= -1;
            angleY = -180;
        }
            

        transform.parent = player.transform;
        transform.eulerAngles = new Vector3(0, angleY, startAngle);
        transform.position = new Vector3(0, 0.25f, 0) + headPos;

        setpaths(hitRightSide);
    }
    void setAbilityValues()
    {
        dmg = abilityInfo._damage;
        damageType = abilityInfo._damageType;
        hammerLifetime = abilityInfo._duration;

        //particle
        psDamageScript.damageType = damageType;
        psDamageScript.particleDamage = dmg * (particleDamageProsent / 100);

        var psMain = particle.main;
        durationOfParticle= hammerLifetime * 0.7f;
        psMain.duration = durationOfParticle;
    }

    void setpaths(bool isRight)
    {
        
        float mp = 1f;
        float x = 0.05f;
        float y = 0.1f;
        if (!isRight)
            mp *= -1;

        paths.Add(new Vector3((x + 0.2f)     * mp,(y + 0.3f), 0));
        paths.Add(new Vector3((x + 0.3f) * mp, (y + 0.3f), 0));
        paths.Add(new Vector3((x + 0.3f) * mp, (y + 0.3f), 0));
        paths.Add(new Vector3((x + 0.25f) * mp, (y + 0.2f), 0));
        paths.Add(new Vector3((x + 0.25f) * mp, (y + 0.1f), 0));
        paths.Add(new Vector3((x + 0.25f) * mp, (y + 0f), 0));
        paths.Add(new Vector3((x + 0.25f) * mp, (y + -0.1f),0));
        paths.Add(new Vector3((x + 0.20f) * mp, (y + -0.2f),0));
        paths.Add(new Vector3((x + 0.20f) * mp, (y + -0.3f),0));
        //paths.Add(new Vector3(0.2f,-0.5f,0) * mp);
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go= other.gameObject.transform.root.gameObject;
        if (other.gameObject.CompareTag("Enemy"))
        {           
            if (go.GetComponent<StateController>() != null)
            {
                go.GetComponent<StateController>().TakeDamage(go, dmg, false, damageType, true, 100);
            }
          
        }

    }


    void doVanishAnimation()
    {

        particle.Stop();
        SpriteRenderer r = GetComponentInChildren<SpriteRenderer>();
        
        Vector3 dScale = transform.localScale;
        
        transform.localScale = dScale * 0.80f;

        if(counter > 10)
        {
            Destroy(this.gameObject);
        }

        counter++;
    }
    void canTurnTrue()
    {
        player.GetComponent<Player>().canTurn = true;
    }
    void Update()
    {
        headPos = head.transform.position;
        timer += Time.deltaTime;
        if(timer >= 0.02f)
        {
            x++;
            if (x == paths.Count)
                x = paths.Count - 1;

            timer = 0;
        }
        
        if(doNotRotate)
        {
            
            return;
        }

        // ready to do things
        if (rotated > maxRotate)
        {
            if(!doNotRotate)
            {
                ls.enabled = true;
                flicker.enabled = true;
                InvokeRepeating("doVanishAnimation",durationOfParticle, 0.01f);
                //Destroy(this.gameObject, hammerLifetime);
                doNotRotate = true;
                particle.Play();
                Invoke("canTurnTrue", 0.5f);
                transform.parent = null;

            }
            else
            {
                transform.position = paths[x]+headPos;
                return;
            }
               
        }

        


        //Debug.Log("SPIN " + spinSpeed);

        transform.position = Vector3.Lerp(transform.position, paths[x] + headPos, 5 * Time.deltaTime);
        rotated += Mathf.Abs(spinSpeed * Time.deltaTime);
        //Debug.Log("ROTATED: " + rotated);

        transform.eulerAngles += new Vector3(0, 0, spinSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        spinSpeed *= Mathf.Pow(1.1f,1.0f);


        

    }
}
