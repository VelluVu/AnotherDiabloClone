using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour,IDropHandler
{
    public bool isEquipmentSlot = false;
    public LootSlot droppedLoot;
    public EquipmentSlot equipmentSlot;
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            droppedLoot = eventData.pointerDrag.transform.parent.GetComponent<LootSlot>();
            if (droppedLoot != null)
            {
                if (isEquipmentSlot)
                {// jos siirtää inventorysta equipment slottiin

                    
                    equipmentSlot = transform.parent.GetComponent<EquipmentSlot>();
                   
                    if (droppedLoot.item.armorSlot != equipmentSlot.armorSlot)
                    {

                        return;
                    }
                   
                    if (!droppedLoot.isEmpty)
                    {
                        droppedLoot.item.Equip();
                        droppedLoot.WeaponEquip();
                        transform.parent.GetComponent<EquipmentSlot>().UnEquipItem();
                        RolledLoot tempLoot = gameObject.AddComponent<RolledLoot>(); //Create Temporary Loot
                        tempLoot.transferLoot(transform.parent.GetComponent<RolledLoot>()); //transfer loot to tempLoot
                        transform.parent.GetComponent<RolledLoot>().transferLoot(droppedLoot.GetComponent<RolledLoot>());
                        droppedLoot.GetComponent<RolledLoot>().transferLoot(tempLoot);
                        if (equipmentSlot.isEmpty)
                        {
                            droppedLoot.emptySlot();
                        }
                        else
                        {
                            droppedLoot.UnequipItem();
                        }
                        equipmentSlot.fillSlot();
                        Destroy(tempLoot);
                    }
                    foreach (var v in PlayerInventory.instance.equipmentConnect) // check which equipment position this can be placed in
                    {
                        if (v.key == droppedLoot.item.armorSlot)
                        {
                            int i = 0;
                            foreach (SpriteRenderer SR in v.ES.graphicsSpriteRenderers)
                            {
                                Debug.Log(v.ES.item.equipmentSprites[i]);
                                SR.sprite = v.ES.item.equipmentSprites[i];
                                i++;
                            }
                            break;
                        }
                    }
                }
                else // drag and drop between two inventory slots
                {
                    if (!droppedLoot.isEmpty)
                    {
                        if (droppedLoot.item.stackable && transform.parent.GetComponent<RolledLoot>().itemName == droppedLoot.item.itemName)
                        {
                            if(transform.parent.GetComponent<LootSlot>().stackSize + droppedLoot.stackSize <= PlayerInventory.maxStackSize)
                            {
                                transform.parent.GetComponent<LootSlot>().stackSize += droppedLoot.stackSize;
                                transform.parent.GetComponent<LootSlot>().stackSizeTextEnable(true);
                                droppedLoot.emptySlot();
                            }
                           
                            return;
                            
                        }
                        
                        if(transform.parent.GetComponent<ConsumableSpot>() != null && !droppedLoot.item.consumable)
                        {
                            return;
                        }
                        else if(droppedLoot.item.consumable && !transform.parent.GetComponent<LootSlot>().isEmpty && droppedLoot.GetComponent<ConsumableSpot>() != null)
                        {
                            if (!transform.parent.GetComponent<LootSlot>().item.consumable)
                            {
                                return;
                            }
                            
                        }
                        RolledLoot tempLoot = gameObject.AddComponent<RolledLoot>(); //Create Temporary Loot
                        tempLoot.transferLoot(transform.parent.GetComponent<RolledLoot>()); //transfer loot to tempLoot
                        int tempStackSize = transform.parent.GetComponent<LootSlot>().stackSize;
                        transform.parent.GetComponent<RolledLoot>().transferLoot(droppedLoot.GetComponent<RolledLoot>());
                        transform.parent.GetComponent<LootSlot>().stackSize = droppedLoot.stackSize;
                        droppedLoot.GetComponent<RolledLoot>().transferLoot(tempLoot);
                        droppedLoot.stackSize = tempStackSize;
                        droppedLoot.stackSizeTextEnable(droppedLoot.item.stackable);
                        transform.parent.GetComponent<LootSlot>().stackSizeTextEnable(transform.parent.GetComponent<RolledLoot>().stackable);

                        if (transform.parent.GetComponent<LootSlot>().isEmpty)
                        {
                            droppedLoot.emptySlot();
                        }
                        else
                        {
                            droppedLoot.UnequipItem();
                        }
                        
                        transform.parent.GetComponent<LootSlot>().UnequipItem();
                        if (transform.parent.GetComponent<ConsumableSpot>() != null)
                        {
                            Debug.Log(transform.parent.GetChild(0).localPosition);
                            Debug.Log(transform.parent.GetChild(0).position);
                            transform.parent.GetChild(0).localPosition = new Vector3(0, 27) ;
                        }
                        Destroy(tempLoot);
                        
                    }
                }
            }
            else
            {
                equipmentSlot = eventData.pointerDrag.transform.parent.GetComponent<EquipmentSlot>();
                if (isEquipmentSlot) // equipment slot to equipment slot
                {
                    if (equipmentSlot.item.armorSlot != transform.parent.GetComponent<EquipmentSlot>().armorSlot)
                    {

                        return;
                    }
                    RolledLoot tempLoot = gameObject.AddComponent<RolledLoot>();
                    tempLoot.transferLoot(transform.parent.GetComponent<RolledLoot>()); //transfer loot to tempLoot
                    transform.parent.GetComponent<RolledLoot>().transferLoot(equipmentSlot.GetComponent<RolledLoot>());
                    equipmentSlot.GetComponent<RolledLoot>().transferLoot(tempLoot);
                    if (transform.parent.GetComponent<EquipmentSlot>().isEmpty)
                    {
                        equipmentSlot.emptySlot();
                    }
                    else
                    {
                        equipmentSlot.fillSlot();
                    }
                    transform.parent.GetComponent<EquipmentSlot>().fillSlot();
                    Destroy(tempLoot);
                }
                else // equipment slot to inventory
                {
                    if (!equipmentSlot.isEmpty)
                    {
                        if (transform.parent.GetComponent<LootSlot>().item.armorSlot == equipmentSlot.armorSlot || transform.parent.GetComponent<LootSlot>().isEmpty)
                        {
                            equipmentSlot.removeWeapons();
                            RolledLoot tempLoot = gameObject.AddComponent<RolledLoot>();
                            tempLoot.transferLoot(transform.parent.GetComponent<RolledLoot>()); //transfer loot to tempLoot
                            transform.parent.GetComponent<RolledLoot>().transferLoot(equipmentSlot.item);
                            equipmentSlot.GetComponent<RolledLoot>().transferLoot(tempLoot);
                            transform.parent.GetComponent<LootSlot>().stackSize = 1;
                            if (transform.parent.GetComponent<LootSlot>().isEmpty)
                            {
                                equipmentSlot.emptySlot();
                            }
                            else
                            {
                                equipmentSlot.fillSlot();
                                

                            }
                            transform.parent.GetComponent<LootSlot>().UnequipItem();
                            equipmentSlot.item.Equip();
                            
                            Destroy(tempLoot);

                        }
                        else
                        {
                            Debug.LogWarning("Can't move to spot with wrong type of equipment!");
                            return;
                        }
                    }
                    

                }

            }

        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
