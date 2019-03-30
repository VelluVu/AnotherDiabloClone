using System.Collections;
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
    string key;
    Canvas sortCanvas;
    bool waitForEndDrag;
    EquipmentSlot ES; // check if changed
    LootSlot LS;// check if changed
    

    void Awake()
    {
        ToolTip = GameObject.Find("ToolTip");
        TT = ToolTip.GetComponent<ToolTip>();
       
    }
    void Start()
    {
        if (ToolTip.activeSelf)
        {
            ToolTip.SetActive(false);
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
    }
}

   

   
