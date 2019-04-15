using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityINFO : MonoBehaviour
{
    //tämän classin tarkoitus on vain antaa infot spawnatulle objectille 
    //jonka kautta voidaan antaa aseeseen esim damage jos siinä on erilinen skripti
    
    public float _damage=0;
    public float _duration = 0;
    
    public float _healingPlayer=0;
    public float _healingEnemy=0;


    public float _dotDamage=0;
    public float _dotTime=0;

    public string _desc = "default";

    public Vector2 _direction = Vector2.zero;
    public Vector2 _spawnPos = Vector2.zero;

    

    

    
}
