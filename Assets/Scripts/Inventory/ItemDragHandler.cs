using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour,IDragHandler,IEndDragHandler
{
    
    public Transform movableObject;
    string key;
    Canvas sortCanvas;
    public void OnDrag(PointerEventData eventData)
    {
        if(sortCanvas == null)
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

    public void OnEndDrag(PointerEventData eventData)
    {

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
    
        movableObject.localPosition = Vector3.zero;
        Destroy(sortCanvas);
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
