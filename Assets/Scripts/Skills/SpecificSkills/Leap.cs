using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leap : MonoBehaviour
{

    public GameObject player;

    public float force;
    public float angle = 35;
    public float stunRange = 5;
    public float stunDuration;
    public GameObject smallSmoke;
    public Vector3 dir;
    public List<GameObject> enemies = new List<GameObject>();

    Rigidbody2D rb;
    Collider2D col;

    Player p;
    AbilityINFO abilityInfo;

    bool directionRight;
    float dirValue = 1;
    bool hit=false;
    Vector3 pPos;

    Animator anim;

    Vector3 lastPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        p = player.GetComponent<Player>();
        anim = player.GetComponentInChildren<Animator>();
    }
    void Start()
    {
       
        p.leaping = true;
        p.canTurn = false;

        setDirValue();
        setAbilityValues();
        
        transform.position = player.transform.position + new Vector3(0.5f*dirValue, 0, 0);
        pPos = player.transform.position;
        lastPos = transform.position;
        
        Jump();

        Invoke("startAnim", 0.3f);
        InvokeRepeating("checkIfMoved", 1, 0.1f);
        Destroy(this.gameObject, 10f);
    }

    void setAbilityValues()
    {
        abilityInfo = GetComponent<AbilityINFO>();
        force = abilityInfo._force;
        stunDuration = abilityInfo._duration;
    }
    void setDirValue()
    {
        directionRight = p.directionRight;
        if (!directionRight)
            dirValue *= -1;

    }

    void startAnim()
    {
        anim.SetTrigger("skill_tompJump");
    }

    void getEnemiesAround()
    {
        if (stunRange <= 0)
        {
            stunRange = 0;
        }

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(player.transform.position, new Vector2(stunRange, stunRange), 0);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject obj = hitColliders[i].gameObject;

            if (obj.CompareTag("Enemy"))
            {
                if (!enemies.Contains(obj.transform.root.gameObject))
                {
                    enemies.Add(obj.transform.root.gameObject);
                }
            }
        }
    }

    void stunWeakFearEnemies(bool stun, bool weak, bool fear)
    {
        if (enemies.Count == 0)
        {
            return;
        }

        StateController state;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<StateController>() != null)
            {
                state = enemy.GetComponent<StateController>();
                if (stun)
                    state.SetStun(enemy, stunDuration);
                if (weak)
                    state.ApplyWeakness(enemy, 30, stunDuration);
                if (fear)
                    state.ApplyFear(enemy, stunDuration);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        GameObject g = c.gameObject;
        if (g.CompareTag("Player") )
        {           
            Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), col);

        }

        if(g.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), c.collider);
            Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), col);
        }

        if(g.CompareTag("Ground") || g.CompareTag("Wall"))
        {
            endThings();
        }
    }

    void endThings()
    {
        col.enabled = false;
        hit = true;
        //player.transform.position = transform.position;
        GameObject spawned = Instantiate(smallSmoke, pPos + new Vector3((0.2f) * dirValue, -0.7f, 0), Quaternion.identity);
        Destroy(spawned, 0.5f);
        getEnemiesAround();
        stunWeakFearEnemies(true, false, false);
        p.canTurn = true;
        p.leaping = false;
        Destroy(this.gameObject, 0.3f);
    }

    private void OnDisable()
    {
        p.leaping = false;
    }
    private void Jump()
    {
        dir = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3.right);
        
        if (!directionRight)
            dir.x *= -1;
        
        rb.AddForce(dir * force);
        
    }
    void checkIfMoved()
    {
        
        if (lastPos == transform.position)
        {
            if (hit)
            {
                return;
            }
            endThings();
        }

        lastPos = transform.position;

    }

    private void Update()
    {
        pPos = player.transform.position;
        if(!hit)
        {
            player.transform.position = transform.position + new Vector3(-0.2f * dirValue, 0.5f, 0);
        }

        
       
    }
   

    

}
