using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowOfManaSprit : MonoBehaviour
{
    public float period = 1f;
    public Color goTowardsColor;
    private GameObject player;
    private float mana;
    private float duration;

    private AbilityINFO AbilityINFO;
    private Player p;

    SpriteRenderer[] spritet;
    float t = 0;
    bool switcher = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spritet = player.GetComponentsInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        p = player.GetComponent<Player>();
        setAbilityValues();
        transform.parent = player.transform;
        Destroy(this.gameObject, duration);
        InvokeRepeating("restoreMana", 0, period);
        Invoke("changeSwitch", duration / 2);
    }
    void setAbilityValues()
    {
        AbilityINFO = GetComponent<AbilityINFO>();
        mana = AbilityINFO._mana;
        duration = AbilityINFO._duration;
    }

    void restoreMana()
    {
        p.OnRestoreMana(mana, false);
    }

    void changeSwitch()
    {
        for (int i = 0; i < spritet.Length; i++)
        {
            spritet[i].color = goTowardsColor;
        }
        t = 0;
        switcher = false;
    }
    private void OnDisable()
    {
        for (int i = 0; i < spritet.Length; i++)
        {
            spritet[i].color = Color.white;
        }
    }
    private void Update()
    {
        if (switcher)
        {
            for(int i=0; i < spritet.Length; i++)
            {
                spritet[i].color= Color.Lerp(Color.white, goTowardsColor, t);
            }
            
        }
        else
        {
            for (int i = 0; i < spritet.Length; i++)
            {
                spritet[i].color = Color.Lerp(goTowardsColor, Color.white, t);
            }
        }

        if (t < 1)
        {
            t += Time.deltaTime / (duration / 2);
        }
    }
}
