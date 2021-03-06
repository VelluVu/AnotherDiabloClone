﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropToWorld : MonoBehaviour, IDropHandler
{
    public GameObject dropObject;

    
    CanvasGroup CG;

    private void Awake()
    {
        CG = GetComponent<CanvasGroup>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {

            GameObject droppedLoot = Instantiate(dropObject, Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10)), Quaternion.identity);
            droppedLoot.GetComponent<PickUpLoot>().rLoot = droppedLoot.GetComponent<RolledLoot>();
            droppedLoot.GetComponent<PickUpLoot>().rLoot.transferLoot(eventData.pointerDrag.transform.parent.GetComponent<RolledLoot>());
            droppedLoot.GetComponent<SpriteRenderer>().sprite = droppedLoot.GetComponent<PickUpLoot>().rLoot.lootSprite;
            droppedLoot.GetComponent<PickUpLoot>().DropSetup();
            if (eventData.pointerDrag.transform.parent.GetComponent<LootSlot>() != null)
            {
                if (!eventData.pointerDrag.transform.parent.GetComponent<LootSlot>().isEmpty)
                {
                    droppedLoot.GetComponent<PickUpLoot>().Count = eventData.pointerDrag.transform.parent.GetComponent<LootSlot>().stackSize;
                    eventData.pointerDrag.transform.parent.GetComponent<LootSlot>().emptySlot();
                }
                else
                {
                    Destroy(droppedLoot);
                    return;
                }
                
            }
            else if(eventData.pointerDrag.transform.parent.GetComponent<EquipmentSlot>() != null)
            {
                if (!eventData.pointerDrag.transform.parent.GetComponent<EquipmentSlot>().isEmpty)
                {
                    droppedLoot.GetComponent<PickUpLoot>().Count = 1;
                    eventData.pointerDrag.transform.parent.GetComponent<EquipmentSlot>().emptySlot();
                    eventData.pointerDrag.transform.parent.GetComponent<EquipmentSlot>().removeWeapons();
                }
                else
                {
                    Destroy(droppedLoot);
                    return;
                }
                
            }
              

            
            droppedLoot.transform.SetParent(PlayerInventory.instance.LootHolder);
        }
    }
    private void Update()
    {
        if (PlayerInventory.instance.draggingInventoryItem)
        {
            CG.blocksRaycasts = true;
        }
        else
        {
            CG.blocksRaycasts = false;
        }
    }

    
}
