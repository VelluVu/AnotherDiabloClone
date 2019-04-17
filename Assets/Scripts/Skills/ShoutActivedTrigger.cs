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
    [HideInInspector] public float increaseDamage;
    [HideInInspector] public float dotDamage;
    [HideInInspector] public float speedBuff;
    [HideInInspector] public float manaUsage;
    [HideInInspector] public GameObject spawneGameobj;
    [HideInInspector] public bool fearEnemy;
    
    //[HideInInspector] public bool castToHolder;
    private ShoutScript shoutScript;

    public void setValues(ShoutAbility skill)
    {
        
        range = skill._range;
        duration = skill._duration;
        heal = skill._heal;
        damage = skill._damage;
        dotDamage = skill._dotDamage;
        speedBuff = skill._speedBuff;
        manaUsage = skill._manaUsage;
        fearEnemy = skill._fearEnemy;
        spawneGameobj = skill._spawnGameobject;
        increaseDamage = skill._increaseDamage;
    }

    public void Activate(Ability skill)
    {
        ShoutAbility shout;
        if(skill is ShoutAbility)
        {
            shout = skill as ShoutAbility;
            setValues(shout);
        }
        else
        {
           
            return;
        }
               
        if (this.gameObject.GetComponent<Player>() != null)
        {
            Debug.Log("mana used " + manaUsage);
            this.gameObject.GetComponent<Player>().OnManaUse(manaUsage);
        }
        //Debug.Log("range :" + range);
        //Debug.Log("duration :" + duration);
        //Debug.Log("heal :" + heal);
        //Debug.Log("damage :" + damage);
        //Debug.Log("dotDamage :" + dotDamage);
        //Debug.Log("speedbuff :" + speedBuff);
        //Debug.Log("manaUsage :" + manaUsage);
        //GameObject spawnedOBJ;
        if (spawneGameobj != null)
            spawneGameobj = Instantiate(spawneGameobj, transform.position, Quaternion.identity);

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
