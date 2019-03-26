using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    RolledLoot tempObject;
    public RolledLoot item; // the item
    public bool isEmpty;
    public RawImage iconImage;
    public RawImage rarityImage;
    public string SlotName;
    TMPro.TMP_Text slotNameText;

    private void Awake()
    {
        slotNameText = GetComponentInChildren<TMPro.TMP_Text>(); 
        item = GetComponent<RolledLoot>();
        emptySlot();
        item.itemID = -1;
    }
    public void emptySlot()
    {
        iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 0); // set alpha to 0;
        rarityImage.color = new Color(rarityImage.color.r, rarityImage.color.g, rarityImage.color.b, 0); // set alpha to 0;
        isEmpty = true;
        item.itemID = -1;
        slotNameText.enabled = true;


    }
    public void fillSlot()
    {
        iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 1); // set alpha to 0;
        rarityImage.color = new Color(rarityImage.color.r, rarityImage.color.g, rarityImage.color.b, 1); // set alpha to 0;
        rarityImage.color = PlayerInventory.instance.rarity[item.Rarity];
        iconImage.texture = item.itemIcon;
        isEmpty = false;
        slotNameText.enabled = false;
    }
    public RolledLoot EquipItem(RolledLoot loot)
    {
        Destroy(tempObject);
        tempObject = gameObject.AddComponent<RolledLoot>();
        tempObject.transferLoot(item);
        
        item.transferLoot(loot);
        rarityImage.color = PlayerInventory.instance.rarity[loot.Rarity];
        iconImage.texture = loot.itemIcon;
        if (isEmpty)
        {
            fillSlot();
        }
        Debug.Log(tempObject.itemID);
       
        return tempObject;

    }
    public void UnEquip()
    {
        PlayerInventory.instance.AddItem(item,1);
        emptySlot();

    }
    public bool checkEmpty()
    {
        return isEmpty;
    }


}
