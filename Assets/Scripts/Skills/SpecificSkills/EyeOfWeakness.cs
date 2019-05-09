using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeOfWeakness : MonoBehaviour
{
    GameObject player;
    float duration;
    private void Awake()
    {
        player=GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        duration = GetComponent<ShoutScript>().duration;
        setPos();
        transform.parent = player.transform;
        Destroy(this.gameObject, duration);
    }

    void setPos()
    {
        transform.position = player.transform.position + new Vector3(0, 0.6f, 0);
    }
    
}
