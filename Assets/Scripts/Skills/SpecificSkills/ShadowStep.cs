using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStep : MonoBehaviour
{
    public GameObject player;
    public Color changeToColor;

    public float force;
    public float angle = 10;
    public float fearRange = 5;
    public float fearDuration;
    public GameObject bigSmoke;
    public Vector3 dir;
    public List<GameObject> enemies = new List<GameObject>();

    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer[] spritet;
    
    Player p;
    AbilityINFO abilityInfo;

    bool directionRight;
    float dirValue = 1;
    bool hit = false;
    Vector3 pPos;
    Vector3 lastPos;

    Animator anim;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        p = player.GetComponent<Player>();
        anim = player.GetComponentInChildren<Animator>();
        spritet = player.GetComponentsInChildren<SpriteRenderer>();
    }
    void Start()
    {
        p.leaping = true;
        p.canTurn = false;
        setDirValue();
        setAbilityValues();
        changeColor(true);
        transform.position = player.transform.position + new Vector3(0.5f * dirValue, 0, 0);
        pPos = player.transform.position;
        lastPos = transform.position;
        setSmoke();
        Jump();

        Invoke("startAnim", 0.3f);
        InvokeRepeating("checkIfMoved", 1, 0.1f);

        Destroy(this.gameObject, 10f);
    }

    void changeColor(bool switcher)
    {
        if(switcher)
        {
            for (int i = 0; i < spritet.Length; i++)
            {
                spritet[i].color = changeToColor;
            }
        }
        else
        {
            for (int i = 0; i < spritet.Length; i++)
            {
                spritet[i].color = Color.white;
            }
        }
       
    }

    void setAbilityValues()
    {
        abilityInfo = GetComponent<AbilityINFO>();
        force = abilityInfo._force;
        fearDuration = abilityInfo._duration;
    }
    void setDirValue()
    {
        directionRight = p.directionRight;
        if (directionRight)
            dirValue *= -1;
    }

    void startAnim()
    {
        anim.SetTrigger("skill_shadowStep");
    }

    void setSmoke()
    {
        getEnemiesAround();
        stunWeakFearEnemies(false, false, true);
        GameObject spawned = Instantiate(bigSmoke, pPos+new Vector3(0,-0.5f,0), Quaternion.identity);
        ParticleSystem part = spawned.GetComponentInChildren<ParticleSystem>();
        var main = part.main;
        main.duration = fearDuration;
        part.Play();
        Destroy(spawned, fearDuration+2);
    }

    void getEnemiesAround()
    {
        if (fearRange <= 0)
        {
            fearRange = 0;
        }

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(player.transform.position, new Vector2(fearRange, fearRange), 0);

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
                    state.SetStun(enemy, fearDuration);
                if (weak)
                    state.ApplyWeakness(enemy, 30, fearDuration);
                if (fear)
                    state.ApplyFear(enemy, fearDuration);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        GameObject g = c.gameObject;
        if (g.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), col);
        }

        if (g.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), c.collider);
            Physics2D.IgnoreCollision(g.GetComponent<Collider2D>(), col);
        }

        if (g.CompareTag("Ground") || g.CompareTag("Wall"))
        {
            endThings();
        }
    }

    void endThings()
    {
        col.enabled = false;
        hit = true;
        p.canTurn = true;
        p.leaping = false;

        Destroy(this.gameObject, 0.3f);
    }

    private void OnDisable()
    {
        p.leaping = false;
        changeColor(false);
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

    private void Jump()
    {
        dir = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3.right);

        if (directionRight)
            dir.x *= -1;

        rb.AddForce(dir * force);

    }


    private void Update()
    {
        pPos = player.transform.position;
        if (!hit)
        {
            player.transform.position = transform.position + new Vector3(-0.2f * dirValue, 0.5f, 0);
        }
    
    }

}
