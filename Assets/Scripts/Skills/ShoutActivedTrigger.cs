using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutActivedTrigger : MonoBehaviour
{
    //[HideInInspector] public GameObject prefab;
    [HideInInspector] public float range;
    [HideInInspector] public float duration;
    [HideInInspector] public float heal;
    [HideInInspector] public float damage;
    [HideInInspector] public float dotDamage;
    [HideInInspector] public float speedBuff;
    [HideInInspector] public float manaUsage;
    [HideInInspector] public bool fearEnemy;
    
    //[HideInInspector] public bool castToHolder;
    private ShoutScript shoutScript;


    public void Activate()
    {
        Debug.Log("SHOUT");
        if (this.gameObject.GetComponent<Player>() != null)
        {
            Debug.Log("mana used" + manaUsage);
            this.gameObject.GetComponent<Player>().OnManaUse(manaUsage);
        }

        shoutScript = this.gameObject.AddComponent<ShoutScript>();

        shoutScript.range = range;
        shoutScript.duration = duration;
        shoutScript.heal = heal;
        shoutScript.damage = damage;
        shoutScript.dotDamage = dotDamage;
        shoutScript.speedBuff = speedBuff;
        shoutScript.fearEnemy = fearEnemy;
        
        
    }
    
}
