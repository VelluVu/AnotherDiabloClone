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

    public bool addToSlot(RolledLoot loot, int count,bool autoEquip)
    {
        
        if (isFilled)
        {
            return false;
        }
        else if (isEmpty)
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
            if (Settings.instance.autoEquip && autoEquip) // if AutoEquip is on
            {
                CheckIfEquipSlotEmpty();
            }
            
            return true;

        }
        else if (loot.stackable && stackSize + count < PlayerInventory.maxStackSize)
        {
            stackSize += count;
            return true;
        }
        return false;
    }
    public void CheckIfEquipSlotEmpty()
    {
        Debug.Log("CheckIfEmpty");
        Debug.Log(item.equippable);
        if (item.equippable)
        {
            for(int i = 0; i < PlayerInventory.instance.equipmentConnect.Count; i++)
            { 
                if (PlayerInventory.instance.equipmentConnect[i].key == item.armorSlot)
                {
                    
                    
                    if (PlayerInventory.instance.equipmentConnect[i].ES.isEmpty)
                    {
                        Debug.Log("AAAAAAAAAAAAAAAAAAAAAASSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSDDDDDDDDDDDDDDDDDDD");
                        EquipItem(true);
                        return;
                    }
                    else if (PlayerInventory.instance.equipmentConnect[i + 1].ES.isEmpty && PlayerInventory.instance.equipmentConnect[i].key == "Ring")
                    {
                        Debug.Log("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSDDDDDDDDDDDDDDDDDDD");
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
            Debug.Log(item.equippable);
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
                            else if(PlayerInventory.instance.equipmentConnect[i+1].ES.isEmpty &&  item.armorSlot == "Ring" && PlayerInventory.instance.equipmentConnect[i + 1].ES.SlotName == "Ring");
                            {
                                RolledLoot tempLoot = PlayerInventory.instance.equipmentConnect[i+1].ES.EquipItem(item);
                                item.transferLoot(tempLoot);
                                emptySlot();
                                return;
                            }
                        }
                        else
                        {
                            if(PlayerInventory.instance.equipmentConnect[i].key == "Ring")
                            {
                                if (PlayerInventory.instance.equipmentConnect[i].ES.isEmpty)
                                {
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
                                else if (PlayerInventory.instance.equipmentConnect[i + 1].ES.isEmpty && PlayerInventory.instance.equipmentConnect[i + 1].ES.SlotName == "Ring")
                                {
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
        rarityImage.color = PlayerInventory.instance.rarity[item.Rarity];
        isEmpty = false;
        Debug.Log("Unequipped Item");
    }
   
}
