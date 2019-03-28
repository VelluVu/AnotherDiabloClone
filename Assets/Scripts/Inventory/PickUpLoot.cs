using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLoot : MonoBehaviour
{
    public Loot loot;
    public RolledLoot rLoot;
    public bool pickedUp = false;
    public void Awake()
    {
        if(loot != null)
        {
            Setup();
        }
        
        
    }
    public void Setup()
    {
        GetComponent<SpriteRenderer>().sprite = loot.lootSprite;
        rLoot = gameObject.GetComponent<RolledLoot>();
        rLoot.rollLoot(loot);
    }
    public void DropSetup()
    {
        
        
    }
}
