using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutScript : MonoBehaviour
{
    // Tämä scripti tulee playeriin kun shoutti laukaistaan.. tekee skilli toimen piteet ja lähtee pois

    [HideInInspector] public float range;
    [HideInInspector] public float duration;
    [HideInInspector] public float heal;
    [HideInInspector] public float damage;
    [HideInInspector] public float dotDamage;
    [HideInInspector] public float speedBuff;
    [HideInInspector] public bool fearEnemy;

    GameObject holder;

    
    Player playerC;

     
    bool speedBuffOn = false;

    private void Start()
    {
        
        holder = this.gameObject;
        if(holder.GetComponent<Player>() != null)
        {
            playerC = holder.GetComponent<Player>();
        }
        if(heal > 0)
        {
            playerC.OnRestoreHealth(heal, false);
        }

        if(damage > 0)
        {
            
            getEnemiesInRangeSetDotOrDamage(false);
        }
        if(dotDamage > 0 && range > 0)
        {
            
            getEnemiesInRangeSetDotOrDamage(true);
        }

        if(speedBuff > 0)
        {
            playerC.speedScale +=speedBuff;
            speedBuffOn = true;
            Destroy(this, duration);
        }
        else
        {
            Destroy(this);
        }
        
    }
    private void OnDisable()
    {
        if(speedBuffOn)
        {
            playerC.speedScale -= speedBuff;
        }
    }

    void getEnemiesInRangeSetDotOrDamage(bool isDotDamage)
    {
        Collider2D[] hitColliders;
        hitColliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D col in hitColliders)
        {
            if (col.tag == "Enemy")
            {
             if(col.gameObject.GetComponent<StateController>() != null)
                {
                    Debug.Log("ENEMy found " + col.gameObject);
                    //enemies.Add(col.gameObject);
                    if(isDotDamage)
                    {
                        col.gameObject.GetComponent<StateController>().setDotToTarget(col.gameObject, dotDamage, (int)duration, DamageType.Raw);
                    }
                    else
                    {
                        col.gameObject.GetComponent<StateController>().TakeDamage(col.gameObject, damage, DamageType.Raw);
                    }
                    


                }
            }
        }
    }

    //void makeDamageAllEnemiesInList(float dmg)
    //{
    //    if (enemies.Count == 0)
    //        return;

    //    foreach(GameObject enemy in enemies)
    //    {
    //        if (enemy.GetComponent<StateController>() == null)
    //            return;
    //        StateController state = enemy.GetComponent<StateController>();
    //        state.TakeDamage(enemy, dmg);

    //    }
    //}

    //void addDotAllEnemiesInList(float dmg, float dur)
    //{
    //    if (enemies.Count == 0)
    //        return;

    //    foreach (GameObject enemy in enemies)
    //    {
    //        dotScript=enemy.AddComponent<DotScript>();
    //        dotScript._duration = dur;
    //        dotScript._dotDamage = dmg;

    //    }
    //}
}
