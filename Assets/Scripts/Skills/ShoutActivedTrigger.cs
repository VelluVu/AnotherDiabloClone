using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutActivedTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject prefab;
    [HideInInspector] public float range;
    [HideInInspector] public float duration;
    [HideInInspector] public float heal;
    [HideInInspector] public float damage;
    [HideInInspector] public float damageBuff;
    [HideInInspector] public float speedBuff;
    

    public void Activate()
    {
        Debug.Log("SHOUT");
    }
    
}
