using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAxeSkill : MonoBehaviour
{
    private AbilityINFO info;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private bool isGoingLeft = true;
    private float spinSpeed = 1500;
    private PlayerClass playerClass;
    public DamageType damageType;

    private float damage;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //playerClass = player.GetComponent<PlayerClass>();
        //playerClass.hea
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        Vector2 dirPos = (info._direction - (Vector2)player.transform.position);
       
        //float dotR = Vector2.Dot(dirPos, (Vector2)player.transform.position + new Vector2(0,1));
       
        if(Vector2.Dot(dirPos,player.transform.position)<0)
        {
            // vasenmalla puolella
            Debug.Log("vasen");
        }
        else
        {
            //oikella puolella
            
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
        spin();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go;
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player");
           
        }else if(other.gameObject.CompareTag("Enemy"))
        {
            go = other.gameObject;

            if (go.GetComponent<StateController>() != null)
            {
                go.GetComponent<StateController>().TakeDamage(go, damage, damageType);
            }

            Destroy(this.gameObject);

        }

       

    }

}
