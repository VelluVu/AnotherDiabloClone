using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAxeSkill : MonoBehaviour
{
    private AbilityINFO info;
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    private bool isGoingLeft = true;
    private float spinSpeed = 800;
    private PlayerClass playerClass;
    public DamageType damageType;

    private float damage;

    private bool stopMoving = false;
    private Vector3 stopPos;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        if (GetComponent<AbilityINFO>() == null)
        {
            Debug.Log("ThrowingAxe dont have AbilityINFO script.. it has been added");
            this.gameObject.AddComponent<AbilityINFO>();
        }

        info = GetComponent<AbilityINFO>();
        damage = info._damage;
        flip();
    }    

    bool isLeft(Vector2 a, Vector2 b)
    {
        return (-a.x * b.y + a.y * b.x) < 0;
    }
    void flip()
    {     
        if(player.GetComponent<Player>().directionRight)
        {
            spriteRenderer.flipX = true;
            isGoingLeft = false;
        }
   
    }
    void spin()
    {
        int dir = 1;
        if(!isGoingLeft)
            dir = -1;
        
        transform.Rotate(new Vector3(0, 0, dir * spinSpeed * Time.deltaTime));
    }
    
    void Update()
    {
        if(!stopMoving)
        {
            spin();
        }
        else
        {
            transform.position = stopPos;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (stopMoving)
            return;
        GameObject go = other.gameObject.transform.root.gameObject;
        if (other.gameObject.CompareTag("Player"))
        {
            
           
        }else if(go.CompareTag("Enemy"))
        {
            

            if (go.GetComponent<StateController>() != null)
            {
                go.GetComponent<StateController>().TakeDamage(go, damage, false, damageType, true, 100);
            }

            Destroy(this.gameObject);

        }
        else if(!go.CompareTag("LootHolder") || !go.CompareTag("Untagged"))
        {
            stopPos = transform.position;
            stopMoving = true;
            Destroy(this.gameObject,3);
        }

        

       

    }

}
