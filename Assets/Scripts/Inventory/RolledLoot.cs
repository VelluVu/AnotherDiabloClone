using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RolledLoot : MonoBehaviour
{
    
    #region itemInfo
    public string itemName; // name of the item
    [TextArea(3, 5)]
    public string description; // Description of the item
    public int itemID; // id of the item
    public Texture itemIcon; // itemin iconi
    public Sprite lootSprite;
    public List<Sprite> equipmentSprites;
    public Rarity Rarity; // item rarity
    public int itemLevel; // the level of the item
    #endregion

    public List<Tags> tags;
    #region attributes
    public List<RollAttribute> attributes = new List<RollAttribute>();
    #endregion

    #region bools
    public bool equippable;
    public bool consumable;
    public bool stackable;
    public bool mainHand;
    public bool offHand;
    #endregion

    

    #region armorData
    public ArmorSlot armorSlot; // mihin slottiin tämä sopii
    #endregion

    #region consumableData
    public float consumablePotency; // consumablen vahvuus
    #endregion

    #region value
    public float buyPrice;
    public float sellPrice;
    #endregion

    #region Saving
    public Loot originLoot;
    #endregion
    //rollaa rollattavat muuttujat lootissa ja asettaa loput muuttujat
    public RolledLoot rollLoot (Loot loot)
    {
        this.itemName = loot.itemName;
        this.description = loot.description;
        this.itemID = loot.itemID;
        this.itemIcon = loot.itemIcon;
        this.lootSprite = loot.lootSprite;
        this.equipmentSprites = loot.equipmentSprites;
        this.Rarity = loot.Rarity;
        this.itemLevel = loot.itemLevel;
        this.tags = loot.tags;
      
       
        // bools
        this.equippable = loot.equippable;
        this.consumable = loot.consumable;
        this.stackable = loot.stackable;
        this.mainHand = loot.mainHand;
        this.offHand = loot.offHand;

       
        this.armorSlot = loot.armorSlot;
        this.consumablePotency = loot.consumablePotency;
        this.buyPrice = loot.buyPrice;
        this.sellPrice = loot.sellPrice;
        this.originLoot = loot;
        int i = 0;
        foreach (Attribute a in loot.attributes)
        {
            RollAttribute RA = gameObject.AddComponent<RollAttribute>();
            Debug.Log(attributes);
            attributes.Add(RA);
            RA.rollAttribute(a);
            if (consumable)
            {
                RA.value = consumablePotency;
            }
            if (RA.RollValueByItemLevels)
            {
                RA.value = Random.Range(RA.value, RA.valueMax) * (1+itemLevel*0.2f);
                RA.value2Min = Random.Range(RA.value2Min, RA.value2Max) * (1 + itemLevel * 0.2f);
                RA.value = (int)RA.value;
                RA.value2Min = (int)RA.value2Min ;
            }
          
            Debug.Log(attributes[i]);
            i++;
        }
        return this;
    }
    // käytetään tiedonsiirton kun aseita equipataan tai siirettän eri inventoryslottiin
    public RolledLoot transferLoot(RolledLoot loot)
    {
        
        this.itemName = loot.itemName;
        this.description = loot.description;
        this.itemID = loot.itemID;
        this.itemIcon = loot.itemIcon;
        this.lootSprite = loot.lootSprite;
        this.equipmentSprites = loot.equipmentSprites;
        this.Rarity = loot.Rarity;
        this.itemLevel = loot.itemLevel;
        this.attributes = loot.attributes;
        this.tags = loot.tags;
        // bools
        this.equippable = loot.equippable;
        this.consumable = loot.consumable;
        this.stackable = loot.stackable;
        this.mainHand = loot.mainHand;
        this.offHand = loot.offHand;

        
        this.armorSlot = loot.armorSlot;
        this.consumablePotency = loot.consumablePotency;
        this.buyPrice = loot.buyPrice;
        this.sellPrice = loot.sellPrice;
        this.originLoot = loot.originLoot;
        return this;
      
        
    }
    public void Equip()
    {
        foreach(RollAttribute r in attributes)
        {
            r.Equip();
        }
    }
    public void UnEquip()
    {
        foreach (RollAttribute r in attributes)
        {
            r.Unequip();
        }
    }


}
