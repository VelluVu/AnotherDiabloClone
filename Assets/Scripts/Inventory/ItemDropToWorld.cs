using System.Collections;
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
            GameObject droppedLoot = Instantiate(dropObject,Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x,eventData.position.y,10)), Quaternion.identity);
            droppedLoot.GetComponent<PickUpLoot>().rLoot = droppedLoot.GetComponent<RolledLoot>();
            droppedLoot.GetComponent<PickUpLoot>().rLoot.transferLoot(eventData.pointerDrag.transform.parent.GetComponent<RolledLoot>());
            droppedLoot.GetComponent<SpriteRenderer>().sprite = droppedLoot.GetComponent<PickUpLoot>().rLoot.lootSprite;
            droppedLoot.GetComponent<PickUpLoot>().DropSetup();
            
            if(eventData.pointerDrag.transform.parent.GetComponent<LootSlot>() != null)
            {
                eventData.pointerDrag.transform.parent.GetComponent<LootSlot>().emptySlot();
            }
            else if(eventData.pointerDrag.transform.parent.GetComponent<EquipmentSlot>() != null)
            {
                eventData.pointerDrag.transform.parent.GetComponent<EquipmentSlot>().emptySlot();
                eventData.pointerDrag.transform.parent.GetComponent<EquipmentSlot>().removeWeapons();
            }
          
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
