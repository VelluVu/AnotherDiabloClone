using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer head;
    public Color goTowardsColor;

    Color defaultHeadColor;
    float duratiion;
    float t = 0;
    bool switcher = true;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        loopChilds();
        
    }
    void Start()
    {
        duratiion = GetComponent<AbilityINFO>()._duration;
        
        defaultHeadColor = head.color;
        
        transform.parent = player.transform;
        //transform.position = player.transform.position + new Vector3(0, 0.3f, 0);
        Invoke("changeSwitch", duratiion / 2);
        Destroy(this.gameObject, duratiion);
    }

    void changeSwitch()
    {
        head.color = goTowardsColor;
        t = 0;
        switcher=false;
    }
    private void Update()
    {
        if(switcher)
        {
            head.color = Color.Lerp(defaultHeadColor, goTowardsColor, t);
        }
        else
        {
            head.color = Color.Lerp(goTowardsColor,defaultHeadColor, t);
        }
        
        if(t < 1)
        {
            t += Time.deltaTime / (duratiion/2);
        }

    }
    private void OnDisable()
    {
        head.color = defaultHeadColor;
    }
   
    void loopChilds()
    {
        SpriteRenderer[] r = player.GetComponentsInChildren<SpriteRenderer>();

        for(int i=0; i < r.Length; i++)
        {
            if(r[i].name == "Head")
            {
                head = r[i];
                return;
            }
        }
    }
}
