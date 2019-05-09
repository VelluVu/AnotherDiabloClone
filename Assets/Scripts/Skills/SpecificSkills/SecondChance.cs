using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChance : MonoBehaviour
{
    GameObject player;
    AbilityINFO abilityINFO;
    Player p;
    PlayerClass pc;

    float duration;
    float strM;
    float healthM;
    float belowHealt = 0.15f;

    bool visited = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        p = player.GetComponent<Player>();
        pc = player.GetComponent<PlayerClass>();
    }
    private void Start()
    {
        getShoutScriptValues();
        Destroy(this.gameObject, duration);
    }

    void getShoutScriptValues()
    {
        abilityINFO = GetComponent<AbilityINFO>();
        duration = abilityINFO._duration;
        strM = abilityINFO._increaseStrengh;
        healthM = abilityINFO._increaseHealth;
    }

    void active()
    {
        p.OnRestoreHealth(pc.maxHealth.BaseValue * (healthM - 1), false);
        pc.BoostStrength((strM - 1), duration * 0.8f);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if(pc.health.BaseValue < pc.maxHealth.BaseValue*belowHealt && !visited)
        {
            active();
            visited = true;
        }
    }
}
