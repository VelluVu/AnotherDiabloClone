using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fear : MonoBehaviour
{
    public Color changeToColor;
    GameObject player;
    SpriteRenderer[] spritet;
    LightingSource2D ls;
    DemoLightFlicker df;
    float dur;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spritet = player.GetComponentsInChildren<SpriteRenderer>();
    }
    void Start()
    {
        transform.parent = player.transform;

        ls = GetComponent<LightingSource2D>();
        df = GetComponent<DemoLightFlicker>();
        dur = GetComponent<AbilityINFO>()._duration;

        changeColor(true);

        Destroy(this.gameObject, dur);
    }

    private void OnDisable()
    {
        ls.enabled = false;
        df.enabled = false;
        changeColor(false);
    }

    void changeColor(bool switcher)
    {
        if (switcher)
        {
            for (int i = 0; i < spritet.Length; i++)
            {
                spritet[i].color = changeToColor;
            }
        }
        else
        {
            for (int i = 0; i < spritet.Length; i++)
            {
                spritet[i].color = Color.white;
            }
        }
    }
}
