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
        GetComponent<SpriteRenderer>().sprite = loot.lootSprite;
        rLoot = gameObject.AddComponent<RolledLoot>();
        rLoot.rollLoot(loot);
        
    }
}
