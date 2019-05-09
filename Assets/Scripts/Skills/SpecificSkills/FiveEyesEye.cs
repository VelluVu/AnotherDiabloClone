using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveEyesEye : MonoBehaviour
{
    public float damage;
    public DamageType damageType;
    public float trackDistance = 6;
    public GameObject target;
    public GameObject player;
    public SpriteRenderer spriteRenderer;

    public float rotateSpeed = 100f;
    public float velocity;
    public Vector3 ownPos;

    public List<GameObject> enemies;
    public List<GameObject> closesEnemies;

    private bool stopMoving = false;
    private Vector3 stopPos;
    private bool isReadyYet = false;
    private bool visited = false;

    private float period = 0.1f;

    private float scalingValue = 0.1f;
    private float scaled = 0;
    private float maxScale = 0.4f;

    void Start()
    {
        player = transform.root.gameObject;
        getClosestEnemies();
        scalingValue = Random.Range(0.01f, 0.1f);
        InvokeRepeating("increaseScale", 0.3f, period);
        //transform.parent = null;
        if (target == null)
            return;
        
        
    }

    void getClosestEnemies()
    {
        GameObject enemy;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemy = enemies[i];
            if (Vector2.Distance(enemy.transform.position, transform.position) < trackDistance)
            {
                closesEnemies.Add(enemy);
            }
        }
        if (closesEnemies.Count > 0)
        {
            int targetIndex = Random.Range(0, closesEnemies.Count);
            target = closesEnemies[targetIndex];
        }
        else
        {
            Destroy(this.gameObject, 3);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (stopMoving || !isReadyYet)
            return;
        GameObject go = other.gameObject.transform.root.gameObject;

        if (go.CompareTag("Enemy"))
        {
            if(go == target)
            {
                if (go.GetComponent<StateController>() != null)
                {
                    go.GetComponent<StateController>().TakeDamage(go, damage, false, damageType, true, 100);
                }
                Destroy(this.gameObject);
            }
        }
        
    }
    void increaseScale()
    {
        if (isReadyYet)
            return;
        Vector3 value = new Vector3(transform.localScale.y+scalingValue, transform.localScale.y + scalingValue, 0);
        spriteRenderer.color = Color.Lerp(Color.red, Color.white, 1);

        transform.localScale = value;
        scaled += scalingValue;
        if (scaled > maxScale)
            setLaunchValues();
    }
    void setLaunchValues()
    {
        isReadyYet = true;
        transform.parent = null;

        if(target==null)
        {
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate()
    {
       
    }
    void Update()
    {
        if (isReadyYet && target != null)
        {
            visited = true;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, velocity * Time.deltaTime);
            velocity *= 1.1f;
        }

        if(visited && target == null)
        {
            Destroy(this.gameObject);
        }
    }
}

