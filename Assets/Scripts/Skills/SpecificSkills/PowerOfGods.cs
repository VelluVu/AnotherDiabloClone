using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOfGods : MonoBehaviour
{
    GameObject player;
    LightingSource2D ls;
    float dur = 0.3f;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        ls = GetComponent<LightingSource2D>();
        transform.parent = player.transform;
        Invoke("disLight",0.5f);
        Destroy(this.gameObject, 5); 
    }
    void disLight()
    {
        ls.enabled = false;
    }
    

}
