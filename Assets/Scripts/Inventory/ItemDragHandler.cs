﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{


    GameObject ToolTip;
    ToolTip TT; // tooltip
    public Transform movableObject;
    public string key;
    public Canvas sortCanvas;
    bool waitForEndDrag;
    EquipmentSlot ES; // check if changed
    LootSlot LS;// check if changed
    

    void Awake()
    {
        ToolTip = GameObject.Find("ToolTip");
        if(ToolTip != null)
        {
            TT = ToolTip.GetComponent<ToolTip>();
        }
        
       
    }
    void Start()
    {
        if(ToolTip != null)
        {
            if (ToolTip.activeSelf)
            {
                ToolTip.SetActive(false);
            }
        }
       

    }


    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerInventory.instance.draggingInventoryItem = true;
            if (sortCanvas == null)
            {
                
                sortCanvas = movableObject.gameObject.AddComponent<Canvas>();
                sortCanvas.overrideSorting = true;
                sortCanvas.sortingOrder = 20;
                foreach (var v in PlayerInventory.instance.equipmentConnect)
                {
                    if (v.key == transform.parent.GetComponent<RolledLoot>().armorSlot)
                    {

                        v.ES.gameObject.GetComponent<RawImage>().color = Color.grey;

                        key = v.key;
                    }
                }
            }
            movableObject.transform.position = Input.mousePosition;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PlayerInventory.instance.draggingInventoryItem = false;
        foreach (var v in PlayerInventory.instance.equipmentConnect)
        {
            if (v.key == key)
            {
                if (!v.ES.isEmpty)
                {
                    v.ES.gameObject.GetComponent<RawImage>().color = PlayerInventory.instance.rarity[v.ES.item.Rarity];
                }
                else
                {
                    v.ES.gameObject.GetComponent<RawImage>().color = Color.clear;
                }


            }
        }
        if (waitForEndDrag)
        {
            ToolTip.SetActive(false);
            waitForEndDrag = false;
        }

        movableObject.localPosition = Vector3.zero;
        Destroy(sortCanvas);
    }

   // Detect if hovering changed
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            if (eventData.pointerEnter.transform.parent.GetComponent<EquipmentSlot>() != null)
            {
                if (!eventData.pointerEnter.transform.parent.GetComponent<EquipmentSlot>().isEmpty)
                {
                    EquipmentSlot eSlot = eventData.pointerEnter.transform.parent.GetComponent<EquipmentSlot>();
                    ToolTip.SetActive(true);
                    TT.nameText.text = eSlot.item.itemName;
                    TT.descriptionText.text = eSlot.item.description;
                    Debug.Log("FilledPointerEnter");
                }
                else
                {
                    Debug.Log("EmptyPointerEnter");
                    ToolTip.SetActive(false);
                }

            }
            if (eventData.pointerEnter.transform.parent.GetComponent<LootSlot>() != null)
            {
                if (!eventData.pointerEnter.transform.parent.GetComponent<LootSlot>().isEmpty)
                {
                    LootSlot lSlot = eventData.pointerEnter.transform.parent.GetComponent<LootSlot>();
                    ToolTip.SetActive(true);
                    TT.nameText.text = lSlot.item.itemName;
                    TT.descriptionText.text = lSlot.item.description;

                }
                else
                {
                    Debug.Log("EmptyPointerEnter for LootSlot");
                    ToolTip.SetActive(false);
                }
            }
        }
        
    }
    

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            if (eventData.pointerEnter.transform.parent.GetComponent<EquipmentSlot>() != null)
            {
                if (!eventData.pointerEnter.transform.parent.GetComponent<EquipmentSlot>().isEmpty)
                {
                    ToolTip.SetActive(false);
                }

            }
            if (eventData.pointerEnter.transform.parent.GetComponent<LootSlot>() != null)
            {
                if (!eventData.pointerEnter.transform.parent.GetComponent<LootSlot>().isEmpty)
                {
                    ToolTip.SetActive(false);
                }
            }
        }
        else
        {
            waitForEndDrag =true;
        }

    }
    
    public void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(eventData.pointerPress.transform.parent.GetComponent<EquipmentSlot>() != null)
            {
                if(!eventData.pointerPress.transform.parent.GetComponent<EquipmentSlot>().isEmpty)
                {
                    eventData.pointerPress.transform.parent.GetComponent<EquipmentSlot>().UnEquip();
                    ToolTip.SetActive(false);
                }
            }
            else
            {
                OnPointerEnter(eventData);
            }
            
        }
        if(eventData.button == PointerEventData.InputButton.Left && KeyboardManager.instance.holdingShift && transform.parent.GetComponent<RolledLoot>().stackable 
            && transform.parent.GetComponent<LootSlot>().stackSize > 1)
        {
            GameObject stackBox = Instantiate(PlayerInventory.instance.stackDividerObject,GameObject.Find("UICanvas").transform);
            StackDivide SD = stackBox.GetComponent<StackDivide>();
            SD.maxSize = transform.parent.GetComponent<LootSlot>().stackSize;
            SD.ChangeValue(transform.parent.GetComponent<LootSlot>().stackSize-1);
            SD.parentLootSlot = transform.parent.GetComponent<LootSlot>();
            stackBox.transform.position = new Vector3(transform.position.x-140,transform.position.y);
        }
        if(eventData.button == PointerEventData.InputButton.Left && PlayerInventory.instance.splittingItem && eventData.pointerPress.transform.parent.GetComponent<LootSlot>() != null)
        {
            Debug.Log("SplitItemDrop");
            SplitItemDrop(PlayerInventory.instance.splitItemObject);
        }
        if(eventData.button == PointerEventData.InputButton.Left && transform.parent.GetComponent<RolledLoot>().consumable) // consume item
        {
            
        }
    }
    public void SplitItemDrop(LootSlot theLootSlot)
    {
        Debug.Log(theLootSlot.isEmpty);
        if (!theLootSlot.isEmpty && transform.parent.GetComponent<LootSlot>().isEmpty)
        {
            if (theLootSlot.item.stackable && transform.parent.GetComponent<RolledLoot>().itemName == theLootSlot.item.itemName)
            {
                transform.parent.GetComponent<LootSlot>().stackSize += theLootSlot.stackSize;
                theLootSlot.emptySlot();
                return;

            }
            else
            {

            }
            RolledLoot tempLoot = gameObject.AddComponent<RolledLoot>(); //Create Temporary Loot
            tempLoot.transferLoot(transform.parent.GetComponent<RolledLoot>()); //transfer loot to tempLoot
            int tempStackSize = transform.parent.GetComponent<LootSlot>().stackSize;
            transform.parent.GetComponent<RolledLoot>().transferLoot(theLootSlot.GetComponent<RolledLoot>());
            transform.parent.GetComponent<LootSlot>().stackSize = theLootSlot.stackSize;
            theLootSlot.GetComponent<RolledLoot>().transferLoot(tempLoot);
            theLootSlot.stackSize = tempStackSize;
            theLootSlot.stackSizeTextEnable(theLootSlot.item.stackable);
            transform.parent.GetComponent<LootSlot>().stackSizeTextEnable(transform.parent.GetComponent<RolledLoot>().stackable);

            if (transform.parent.GetComponent<LootSlot>().isEmpty)
            {
                theLootSlot.emptySlot();
            }
            else
            {
                theLootSlot.UnequipItem();
            }
            transform.parent.GetComponent<LootSlot>().UnequipItem();
            Destroy(tempLoot);
            PlayerInventory.instance.splittingItem = false;
            Destroy(PlayerInventory.instance.splitItemObject);
        }
        else
        {
            Debug.LogWarning("Invalid Target for split item. Target must be empty inventory slot");
        }
    }
}

   

   