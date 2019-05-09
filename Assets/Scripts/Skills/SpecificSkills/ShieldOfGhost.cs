using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOfGhost : MonoBehaviour
{
    GameObject player;
    AbilityINFO abilityInfo;
    PlayerClass pc;
    float duration;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerClass>();
    }
    void Start()
    {
        transform.parent = player.transform;
       
        setAbilityValues();
        Destroy(this.gameObject, duration);
    }

    void setBoosts()
    {
        pc.BoostArmor(1000, duration);
    }
    void setAbilityValues()
    {
        abilityInfo = GetComponent<AbilityINFO>();
        duration = abilityInfo._duration;
    }

    
}
