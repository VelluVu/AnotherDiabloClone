using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerRainDagger : MonoBehaviour
{


    // tämä scripti ei ole käytössä! kokeulun onneton tulos!
    public float damage;
    public DamageType damageType;
    public float trackDistance = 3;
    public GameObject target;

    public float rotateSpeed = 100f;
    public float velocity;

    public List<GameObject> enemies;
    public List<GameObject> closesEnemies;

    private bool stopMoving = false;
    //private Vector3 stopPos;
    private bool isReadyYet = false;
    
    
    void Start()
    {
        getClosestEnemies();

        if (target == null)
            return;

        transform.parent = null;
    }

    void getClosestEnemies()
    {        
        GameObject enemy;
        for(int i = 0; i < enemies.Count; i ++)
        {
            enemy = enemies[i];
            if(Vector2.Distance(enemy.transform.position,transform.position) < trackDistance)
            {
                closesEnemies.Add(enemy);
            }
        }
        if(closesEnemies.Count > 0)
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
        if (other.gameObject.CompareTag("Player"))
        {


        }
        else if (go.CompareTag("Enemy"))
        {
            if (go.GetComponent<StateController>() != null)
            {
                go.GetComponent<StateController>().TakeDamage(go, damage, false, damageType, true, 100);
                
            }

            Destroy(this.gameObject);

        }
        else if (!go.CompareTag("LootHolder") || !go.CompareTag("Untagged"))
        {
            Debug.Log("HIT::::: " + other.gameObject.transform.root.gameObject.tag);
            //stopPos = transform.position;
            stopMoving = true;
            Destroy(this.gameObject, 3);
        }
    }
    void Update()
    {
       if(target == null)
       {
            Debug.Log("No target");
            return;
       }

        //Debug.Log("target pos" + target.transform.position);

        Vector2 dir = target.transform.position-transform.position;
        //dir = target.transform.InverseTransformDirection(dir);
        //float angle = Vector2.Angle(target.transform.position, transform.position);
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        //Debug.Log("ANGLE "+ angle);
        Vector3 newD = Vector3.RotateTowards(transform.up, dir, 10 * Time.deltaTime, 0);
        Debug.DrawRay(transform.position, newD, Color.red);
        transform.rotation = Quaternion.LookRotation(newD);
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), 30 * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.1f * Time.deltaTime);
        //transform.position += transform.forward * Time.deltaTime * 2;

    }
}
