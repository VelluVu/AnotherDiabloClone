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
    public string Rarity; // item rarity
    public int itemLevel; // the level of the item
    #endregion


    #region attributes
    public List<Attribute> attributes;
    #endregion

    #region bools
    public bool equippable;
    public bool consumable;
    public bool stackable;
    public bool mainHand;
    public bool offHand;
    #endregion

    #region weaponData
    public float damageMin; // minimi damage mitä ase tekee
    public float damageMax; // maksimi damage mitä ase tekee
    #endregion

    #region armorData
    public float armor; // armorin määrä
    public string armorSlot; // mihin slottiin tämä sopii
    #endregion

    #region consumableData
    public float potency; // consumablen vahvuus
    #endregion

    #region value
    public float buyPrice;
    public float sellPrice;
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
        this.attributes = loot.attributes;
        // bools
        this.equippable = loot.equippable;
        this.consumable = loot.consumable;
        this.stackable = loot.stackable;
        this.mainHand = loot.mainHand;
        this.offHand = loot.offHand;

        this.damageMin = loot.damageMin;
        this.damageMax = loot.damageMax;
        this.armor = loot.armor;
        this.armorSlot = loot.armorSlot;
        this.potency = loot.potency;
        this.buyPrice = loot.buyPrice;
        this.sellPrice = loot.sellPrice;
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
        // bools
        this.equippable = loot.equippable;
        this.consumable = loot.consumable;
        this.stackable = loot.stackable;
        this.mainHand = loot.mainHand;
        this.offHand = loot.offHand;

        this.damageMin = loot.damageMin;
        this.damageMax = loot.damageMax;
        this.armor = loot.armor;
        this.armorSlot = loot.armorSlot;
        this.potency = loot.potency;
        this.buyPrice = loot.buyPrice;
        this.sellPrice = loot.sellPrice;
        return this;
      
        
    }



}
