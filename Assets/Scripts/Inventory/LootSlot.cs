using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;


public class LootSlot : MonoBehaviour
{


    public bool isFilled;
    public bool isEmpty;
    public int stackSize;
    public RolledLoot item;
    public RawImage iconImage;
    public RawImage rarityImage;
    public TMP_Text stackSizeText;

    public List<WeaponPlaceHolder> hand = new List<WeaponPlaceHolder>();

    private void Awake()
    {
        stackSizeText = GetComponentInChildren<TMP_Text>();
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
        stackSizeTextEnable(false);
        stackSize = 0;
        item.itemName = "";
        Debug.Log("Emptied Slot");
    }
    public bool checkIfAddToStack(RolledLoot loot,int count)
    {
        if (loot.stackable && stackSize + count < PlayerInventory.maxStackSize && loot.itemName == item.itemName)
        {
            stackSize += count;
            stackSizeTextEnable(true);
            return true;
        }
        return false;
    }

    public bool addToSlot(RolledLoot loot, int count,bool autoEquip)
    {
        if (loot.stackable && stackSize + count < PlayerInventory.maxStackSize && loot.itemName == item.itemName)
        {
            stackSize += count;
            stackSizeTextEnable(true);
            return true;
        }
        if (isFilled)
        {
            return false;
        }
        else if (isEmpty)
        {
            
            item.transferLoot(loot);
            iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 1); // set alpha to 1;
            iconImage.texture = loot.itemIcon;
            rarityImage.color = PlayerInventory.instance.rarity[loot.rarity];
            

            isEmpty = false;
            if (!item.stackable)
            {
                stackSize = 1;
                isFilled = true;
            }
            else if(item.stackable && count >1)
            {
                stackSize += count;
                stackSizeTextEnable(true);
            }
            if (Settings.instance.autoEquip && autoEquip) // if AutoEquip is on
            {
                CheckIfEquipSlotEmpty();
            }
            
            return true;

        }
       
        return false;
    }
    public void stackSizeTextEnable(bool enable)
    {
        if (enable)
        {
            stackSizeText.enabled = true;
            stackSizeText.text = stackSize.ToString();
        }
        else
        {
            stackSizeText.enabled = false;
        }
    }
    public void CheckIfEquipSlotEmpty()
    {
        if (item.equippable)
        {
            for(int i = 0; i < PlayerInventory.instance.equipmentConnect.Count; i++)
            { 
                if (PlayerInventory.instance.equipmentConnect[i].key == item.armorSlot)
                {
                    
                    
                    if (PlayerInventory.instance.equipmentConnect[i].ES.isEmpty)
                    {
                        
                        EquipItem(true);
                        return;
                    }
                    else if (PlayerInventory.instance.equipmentConnect[i + 1].ES.isEmpty && PlayerInventory.instance.equipmentConnect[i].key == ArmorSlot.Ring)
                    {
                        
                        EquipItem(true);
                        return;
                    }
                   
                    
                }
            }
        }
    }

    public void EquipItem(bool onlyEmptySlot)
    {

        
        if (!isEmpty && item != null)
        {
          
            if (item.equippable)
            {
                
                for (int i = 0; i < PlayerInventory.instance.equipmentConnect.Count; i++)
                {
                    if (PlayerInventory.instance.equipmentConnect[i].key == item.armorSlot) // check armorslot
                    {
                        if (onlyEmptySlot)
                        {
                            if (PlayerInventory.instance.equipmentConnect[i].ES.isEmpty)
                            {
                               
                                WeaponEquip();
                                int j = 0;
                                foreach (SpriteRenderer SR in PlayerInventory.instance.equipmentConnect[i].ES.graphicsSpriteRenderers)
                                {
                                    SR.sprite = item.equipmentSprites[j];
                                    j++;
                                }
                                RolledLoot tempLoot = PlayerInventory.instance.equipmentConnect[i].ES.EquipItem(item);
                                item.transferLoot(tempLoot);
                                emptySlot();
                                return;
                            }
                            else if(PlayerInventory.instance.equipmentConnect[i+1].ES.isEmpty &&  item.armorSlot == ArmorSlot.Ring && PlayerInventory.instance.equipmentConnect[i + 1].ES.armorSlot == ArmorSlot.Ring)
                            {
                                RolledLoot tempLoot = PlayerInventory.instance.equipmentConnect[i+1].ES.EquipItem(item);
                                item.transferLoot(tempLoot);
                                emptySlot();
                                return;
                            }
                        }
                        else
                        {
                            if(PlayerInventory.instance.equipmentConnect[i].key == ArmorSlot.Ring)
                            {
                                if (PlayerInventory.instance.equipmentConnect[i].ES.isEmpty)
                                {
                                    PlayerInventory.instance.equipmentConnect[i].ES.UnEquipItem();
                                    RolledLoot rempLoot = PlayerInventory.instance.equipmentConnect[i].ES.EquipItem(item);
                                    item.transferLoot(rempLoot);
                                    Debug.Log(rempLoot.itemID);
                                    if (rempLoot.itemID == -1)
                                    {

                                        emptySlot();
                                    }
                                    else
                                    {
                                       
                                        UnequipItem();
                                    }

                                }
                                else if (PlayerInventory.instance.equipmentConnect[i + 1].ES.isEmpty && PlayerInventory.instance.equipmentConnect[i + 1].ES.armorSlot ==ArmorSlot.Ring)
                                {
                                    PlayerInventory.instance.equipmentConnect[i].ES.UnEquipItem();
                                    RolledLoot rempLoot = PlayerInventory.instance.equipmentConnect[i+1].ES.EquipItem(item);
                                    item.transferLoot(rempLoot);
                                    Debug.Log(rempLoot.itemID);
                                    if (rempLoot.itemID == -1)
                                    {

                                        emptySlot();
                                    }
                                    else
                                    {
                                       
                                        UnequipItem();
                                    }
                                }
                                else
                                {
                                    PlayerInventory.instance.equipmentConnect[i].ES.UnEquipItem();
                                    RolledLoot rempLoot = PlayerInventory.instance.equipmentConnect[i].ES.EquipItem(item);
                                    item.transferLoot(rempLoot);
                                    Debug.Log(rempLoot.itemID);
                                    if (rempLoot.itemID == -1)
                                    {

                                        emptySlot();
                                    }
                                    else
                                    {
                                       
                                        UnequipItem();
                                    }
                                }
                                break;   
                            }
                            else
                            {
                                WeaponEquip();
                                int j = 0;
                                foreach (SpriteRenderer SR in PlayerInventory.instance.equipmentConnect[i].ES.graphicsSpriteRenderers)
                                {
                                    SR.sprite = item.equipmentSprites[j];
                                    j++;
                                }
                                PlayerInventory.instance.equipmentConnect[i].ES.UnEquipItem();
                                RolledLoot tempLoot = PlayerInventory.instance.equipmentConnect[i].ES.EquipItem(item);
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
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    public void WeaponEquip(){
        if (item.mainHand)// laitetaan ase main handiin
        {
            hand[0].EquipWeapon(item);

        }
        else if (item.offHand) // laitetaan ase off handiin
        {
            hand[1].EquipWeapon(item);

        }

    }

    public void UnequipItem()
    {
        iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 1); // set alpha to 1;
        iconImage.texture = item.itemIcon;
        rarityImage.color = PlayerInventory.instance.rarity[item.rarity];
        isEmpty = false;
        Debug.Log("Unequipped Item");
    }

   
}
