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
    public Rarity rarity; // item rarity
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
    int primaryCount;
    int secondaryCount;
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
        this.rarity = loot.rarity;
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
        RollAttributes(loot);
       
        return this;
    }
    public void RollAttributes(Loot loot)
    {
        List<Attribute> tempList = new List<Attribute>();
        List<Attribute> tempPrimary = new List<Attribute>();
        List<Attribute> tempSecondary = new List<Attribute>();
        primaryCount = PlayerInventory.instance.rarityLootCountPrimary[rarity];
        secondaryCount = PlayerInventory.instance.rarityLootCountSecondary[rarity];
        
        if (loot.randomAttributes)
        {
            
            Debug.Log(AttributePerSlot.instance);
            SlotAttribute SA = AttributePerSlot.instance.findSlot(armorSlot);
            foreach(Attribute att in SA.primaryAttributes){
                tempPrimary.Add(att);
            }
            foreach(Attribute att in SA.secondaryAttributes){
                tempSecondary.Add(att);
            }
            Debug.Log(SA.secondaryAttributes.Count);
            Debug.Log("tempsecondary size "+tempSecondary.Count);
            foreach(Attribute att in SA.constantAttributes)
            {
                tempList.Add(att);
            }
            
            for(int k = 0;k<primaryCount;k++)
            {
                int randomPrimary =Random.Range(0, (int)tempPrimary.Count);
                Debug.Log("RandomPrimary"+randomPrimary);
                tempList.Add(tempPrimary[randomPrimary]);
                tempPrimary.RemoveAt(randomPrimary);
            }
            for(int k = 0;k<secondaryCount;k++)
            {

                int randomSecondary =Random.Range(0, (int)tempSecondary.Count);
                Debug.Log("RandomSecondary"+randomSecondary);
                Debug.Log(tempSecondary.Count);
                tempList.Add(tempSecondary[randomSecondary]);
                tempSecondary.RemoveAt(randomSecondary);
                
            }
               
               
                
               
           
        }
        else
        {
            tempList = loot.attributes;
        }
        int i = 0;
        foreach (Attribute a in tempList)
        {
            RollAttribute RA = gameObject.AddComponent<RollAttribute>();
            Debug.Log(attributes);
            attributes.Add(RA);
            RA.rollAttribute(a);
            RA.rolledLoot = this;
            if (consumable)
            {
                RA.value = consumablePotency;
            }
            else if (RA.RollValueByItemLevels)
            {
                RA.value = Random.Range(RA.value, RA.valueMax) * (1 + itemLevel * 0.2f);
                RA.value2Min = Random.Range(RA.value2Min, RA.value2Max) * (1 + itemLevel * 0.2f);
                RA.value = (int)RA.value;
                RA.value2Min = (int)RA.value2Min;
            }
            else
            {
                RA.value = Random.Range(RA.value, RA.valueMax);
                RA.value2Min = Random.Range(RA.value2Min, RA.value2Max);
                RA.value = (int)RA.value;
                RA.value2Min = (int)RA.value2Min;
            }

            Debug.Log(attributes[i]);
            i++;
        }
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
        this.rarity = loot.rarity;
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
