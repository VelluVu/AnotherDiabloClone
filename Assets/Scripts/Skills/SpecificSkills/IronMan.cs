using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronMan : MonoBehaviour
{
    public Color changeToColor;
    GameObject player;
    SpriteRenderer[] spritet;
    float dur;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spritet = player.GetComponentsInChildren<SpriteRenderer>();
    }
    void Start()
    {
        dur = GetComponent<AbilityINFO>()._duration;
        
        changeColor(true);

        Destroy(this.gameObject, dur);
    }

    private void OnDisable()
    {
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
