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

    private float damage;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //playerClass = player.GetComponent<PlayerClass>();
        //playerClass.hea
    }

    void Start()
    {

        if (GetComponent<AbilityINFO>() == null)
        {
            Debug.Log("ThrowingAxe dont have AbilityINFO script.. it has been added");
            this.gameObject.AddComponent<AbilityINFO>();
        }
     
        info = GetComponent<AbilityINFO>();
        damage = info._damage;
        flip();
    }
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }


    void flip()
    {

       
        float dotR = Vector2.Dot(info._direction, info._spawnPos);
       
        if(dotR > 0)
        {
           // vasenmalla puolella
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
                go.GetComponent<StateController>().TakeDamage(go, damage);
            }

            Destroy(this.gameObject);

        }

       

    }

}
