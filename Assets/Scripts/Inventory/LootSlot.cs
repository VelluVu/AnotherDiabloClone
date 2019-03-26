using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LootSlot : MonoBehaviour
{
   
    
    public bool isFilled;
    public bool isEmpty;
    public int stackSize;
    public RolledLoot item;
    public RawImage iconImage;
    public RawImage rarityImage;
    
    public List<WeaponPlaceHolder> hand = new List<WeaponPlaceHolder>();

    private void Awake()
    {
        
        item = GetComponent<RolledLoot>();
        emptySlot();
        item.itemID = -1;
       
    }
    
    public void emptySlot()
    {
        iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 0); // set alpha to 0;
        rarityImage.color = new Color(rarityImage.color.r, rarityImage.color.g, rarityImage.color.b, 0); // set alpha to 0;
        isEmpty = true;
        isFilled = false;
        Debug.Log("Emptied Slot");
    }

    public bool addToSlot(RolledLoot loot,int count)
    {
        Debug.Log(loot);
        if (isFilled)
        {
            return false;
        }
        else if(isEmpty)
        {

            item.transferLoot(loot);
            iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 1); // set alpha to 1;
            iconImage.texture = loot.itemIcon;
            rarityImage.color = PlayerInventory.instance.rarity[loot.Rarity];

            isEmpty = false;
            if (!item.stackable)
            {
                isFilled = true;
            }
            return true;
            
        }
        else if(loot.stackable  && stackSize+count< PlayerInventory.maxStackSize)
        {
            stackSize += count;
            return true;
        }
        return false;
    }

    public void EquipItem()
    {
        
        if (!isEmpty && item!=null)
        {
            
            if (item.equippable)
            {
                
                if (item.mainHand)// laitetaan ase main handiin
                {
                    hand[0].EquipWeapon(item);

                }
                else if (item.offHand) // laitetaan ase off handiin
                {
                    hand[1].EquipWeapon(item);

                }
                foreach (var v in PlayerInventory.instance.equipmentConnect) // check which equipment position this can be placed in
                {
                    if (v.key == item.armorSlot)
                    {
                        
                        RolledLoot tempLoot = v.ES.EquipItem(item);
                        item.transferLoot(tempLoot);
                        Debug.Log(tempLoot.itemID);
                        if (tempLoot.itemID == -1)
                        {
                            
                            emptySlot();
                        }
                        else
                        {
                            
                            UnequipItem();
                        }
                       
                        
                        break;
                    }
                }





            }
        }     
    }

    public void UnequipItem()
    {
        iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 1); // set alpha to 1;
        iconImage.texture = item.itemIcon;
        rarityImage.color = PlayerInventory.instance.rarity[item.Rarity];
        isEmpty = false;
        Debug.Log("Unequipped Item");
    }
   
}
