using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renew : MonoBehaviour
{
    public float period = 0.5f;
    private GameObject player;
    private float healing;
    private float duration;

    private AbilityINFO AbilityINFO;
    private Player p;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        p = player.GetComponent<Player>();
        setAbilityValues();
        transform.parent = player.transform;
        Destroy(this.gameObject, duration);
        InvokeRepeating("healPlayer", 0, period);
    }
    void setAbilityValues()
    {
        AbilityINFO = GetComponent<AbilityINFO>();
        healing = AbilityINFO._heal;
        duration = AbilityINFO._duration;
    }

    void healPlayer()
    {
        p.OnRestoreHealth(healing, false);
    }


}
