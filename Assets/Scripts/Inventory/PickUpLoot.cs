using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpLoot : MonoBehaviour
{
    public bool setItemLevelByPlayerLevel; // jos item leveli on asetettu leveli vai ei
    public Loot loot;
    public RolledLoot rLoot;
    public bool pickedUp = false;
    public int Count = 1; 
    TMPro.TMP_Text text;
    RawImage rawImage;
    bool setupDone = false;

    public void Awake()
    {
        if(loot != null && setupDone == false)
        {
            Setup(loot.itemLevel);
        }
        
        
    }
    public void Setup(int itemLevel)
    {
        GetComponent<SpriteRenderer>().sprite = loot.lootSprite;
        rLoot = gameObject.GetComponent<RolledLoot>();
        rLoot.rollLoot(loot);
        text = GetComponentInChildren<TMPro.TMP_Text>();
        text.text = rLoot.itemName;
        rawImage = GetComponentInChildren<RawImage>();
        rawImage.color = PlayerInventory.instance.rarity[rLoot.rarity];
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0.7f);
        setupDone = true;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.6f,0.6f), Random.Range(2f, 2.5f)), ForceMode2D.Impulse);

    }
    public void DropSetup()
    {
        text = GetComponentInChildren<TMPro.TMP_Text>();
        text.text = rLoot.itemName;
        rawImage = GetComponentInChildren<RawImage>();
        rawImage.color = PlayerInventory.instance.rarity[rLoot.rarity];
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0.7f);
        setupDone = true;
        
        
    }
    

}
