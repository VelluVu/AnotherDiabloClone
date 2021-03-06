﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotScript : MonoBehaviour
{
    public float _dotDamage=0;
    public float _duration=0;
    private StateController enemyState;
    //private Player playerC;
    private float nextAct=0f;
    private int counter = 0;
    public DamageType _damageType;

    private void Start()
    {
        if(GetComponent<StateController>() != null)
        {            
            enemyState = GetComponent<StateController>();
        }
        //if(GetComponent<Player>() != null)
        //{
        //    playerC = GetComponent<Player>();
        //}


        Destroy(this, _duration);
    }

    private void Update()
    {
        if(Time.time > nextAct)
        {
            counter++; 
            nextAct += 1;
            enemyState.TakeDamage(this.gameObject, _dotDamage, false, _damageType, true, 100);
            
        }
    }

}
