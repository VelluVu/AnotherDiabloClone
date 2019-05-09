using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomp : MonoBehaviour
{
    float duration;
    int dirValue=1;
    GameObject player;
    Player p;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        p = player.GetComponent<Player>();
    }
    void Start()
    {
        transform.parent = player.transform;
        p.CanTurn(0.8f);
        setDirValue();

        duration = GetComponent<AbilityINFO>()._duration;

        transform.position = player.transform.position + new Vector3(0.26f*dirValue, -0.5f, 0);
        Destroy(this.gameObject, duration);
    }

    void setDirValue()
    {
        if(!p.directionRight)
        {
            dirValue *= -1;
        }
    }

    
}
