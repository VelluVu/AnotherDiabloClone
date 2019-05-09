using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutScript : MonoBehaviour
{
    // Tämä scripti tulee playeriin kun shoutti laukaistaan.. tekee skilli toimen piteet ja lähtee pois

    [HideInInspector] public float range;
    [HideInInspector] public float duration;
    [HideInInspector] public float heal;
    [HideInInspector] public float mana;
    [HideInInspector] public float damage;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public float dotDamage;
    [HideInInspector] public float speedBuff;
    [HideInInspector] public bool fearEnemy;
    [HideInInspector] public bool stunEnemy;
    [HideInInspector] public bool weakEnemy;

    [HideInInspector] public float increaseStrM;
    [HideInInspector] public float increaseArmorM;
    [HideInInspector] public float increaseAttactSpeedM;
    [HideInInspector] public float increaseHealthM;

    GameObject holder;
  
    Player playerC;
    PlayerClass pc;
    public List<GameObject> enemies = new List<GameObject>();
     
    bool speedBuffOn = false;

    private void Start()
    {
        playerC = GetComponent<Player>();


        pc = GetComponent<PlayerClass>();
       
            
        getEnemiesAround();
        if (heal > 0)
        {
            Debug.Log("HEAALL");
            
            playerC.OnRestoreHealth(heal, false);
        }

        if(mana > 0 )
        {
            Debug.Log("MANAAA");
            playerC.OnRestoreMana(mana, false);
        }

        if(damage > 0)
        {
            
            setEnemiesInRangeDamageAndDot(false);
        }
        
        if(fearEnemy)
        {
            stunWeakFearEnemies(false, false, true);
        }
        if(stunEnemy)
        {
            stunWeakFearEnemies(true,false,false);
        }
        if(dotDamage > 0 && range > 0)
        {
            setEnemiesInRangeDamageAndDot(true);
        }

        if(increaseHealthM != 0)
        {
            pc.BoostHealth((increaseHealthM - 1f)*100, duration);
        }

        if(increaseStrM != 0)
        {
            pc.BoostStrength((increaseStrM - 1f) * 100, duration);
        }

        if(increaseArmorM != 0)
        {
            pc.BoostArmor((increaseArmorM - 1f) * 100, duration);
        }

        if(increaseAttactSpeedM != 0)
        {
            pc.BoostAttackSpeed((increaseAttactSpeedM - 1f) * 100, duration);
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

    void stunWeakFearEnemies(bool stun,bool weak, bool fear)
    {
        if(enemies.Count == 0)
        {
            return;
        }

        StateController state;
        foreach(GameObject enemy in enemies)
        {
            if (enemy.GetComponent<StateController>() != null)
            {
                state = enemy.GetComponent<StateController>();
                if(stun)
                    state.SetStun(enemy, duration);
                if (weak)
                    state.ApplyWeakness(enemy, 30, duration);
                if (fear)
                    state.ApplyFear(enemy, duration);
            }
        }
    }

    public void setEnemiesInRangeDamageAndDot(bool isDotDamage)
    {
        Debug.Log("COUnt: " + enemies.Count);
        if (enemies.Count <= 0)
        {
            return;
        }
            
        foreach (GameObject enemy in enemies)
        {
            if(enemy.gameObject.GetComponent<StateController>() != null)
            {
                if(isDotDamage)
                {
                    enemy.GetComponent<StateController>().setDotToTarget(enemy, dotDamage, (int)duration, damageType);
                }
                else
                {
                    enemy.GetComponent<StateController>().TakeDamage(enemy, damage, false, damageType, true, 100);
                }
            }
            
        }
    }
    void getEnemiesAround()
    {
        if (range <= 0)
        {
            range = 0;
        }
            
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.root.position, new Vector2(range, range), 0);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject obj = hitColliders[i].gameObject;
            //Debug.Log("OBJ: " + obj);
            if (obj.CompareTag("Enemy"))
            {
                if (!enemies.Contains(obj.transform.root.gameObject))
                {
                    enemies.Add(obj.transform.root.gameObject);
                }
            }
        }
    }
}
