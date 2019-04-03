using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StackDivide : MonoBehaviour
{
    public LootSlot parentLootSlot;
    public GameObject spawnObject;
    public Button addOne;
    public Button subtractOne;
    public Button okButton;
    public Button cancelButton;
    public TMP_InputField inputField;
    public int CurrentSize;
    ItemDragHandler dragger;
    public int maxSize; // stackin maksimikoko
    public bool dragging = false;
    public GameObject hideGraphics;
    
    
   

    private void Awake()
    {
        addOne.onClick.AddListener(() => ChangeValue(1));
        subtractOne.onClick.AddListener(() => ChangeValue(-1));
        okButton.onClick.AddListener(() => Close(false));
        cancelButton.onClick.AddListener(() => Close(true));
        inputField.onValueChanged.AddListener(delegate { SetValue(inputField.text); });
        
        
    }
    private void Start()
    {
        inputField.Select();
    }
    private void Update()
    {
        if (dragging)
        {
            PlayerInventory.instance.splittingItem = true;
            if (dragger.sortCanvas == null)
            {

                dragger.sortCanvas = dragger.movableObject.gameObject.AddComponent<Canvas>();
                dragger.sortCanvas.overrideSorting = true;
                dragger.sortCanvas.sortingOrder = 20;
                foreach (var v in PlayerInventory.instance.equipmentConnect)
                {
                    if (v.key == dragger.transform.parent.GetComponent<RolledLoot>().armorSlot)
                    {

                        v.ES.gameObject.GetComponent<RawImage>().color = Color.grey;

                        dragger.key = v.key;
                    }
                }
            }
            dragger.movableObject.transform.position = Input.mousePosition;
        }
    }
    public void Close(bool cancel)
    {
        if (!cancel)
        {
            if (CurrentSize ==0)
            {
                Debug.LogWarning("Please enter a number");
                return;
            }
            GameObject newObject = Instantiate(spawnObject,PlayerInventory.instance.transform);
            dragger = newObject.GetComponentInChildren<ItemDragHandler>();
            newObject.GetComponent<RolledLoot>().transferLoot(parentLootSlot.item);
            newObject.GetComponent<LootSlot>().iconImage.texture = newObject.GetComponent<RolledLoot>().itemIcon;
            newObject.GetComponent<LootSlot>().isEmpty = false;
            newObject.transform.position = new Vector2(9000, 9000);
            dragger.movableObject.position = Input.mousePosition;
            dragger.movableObject.GetComponent<RawImage>().color = Color.white;
            newObject.GetComponent<LootSlot>().stackSize = CurrentSize;
            newObject.GetComponentInChildren<TMP_Text>().text = CurrentSize.ToString();
            newObject.GetComponentInChildren<TMP_Text>().enabled = true;
            dragging = true;
            hideGraphics.SetActive(false);
            parentLootSlot.stackSize -= CurrentSize;
            parentLootSlot.stackSizeTextEnable(true);
            PlayerInventory.instance.splitItemObject = newObject.GetComponent<LootSlot>();



            //add cursor to ondragEvent
        }
        //Destroy(gameObject);
    }

    public void ChangeValue(int value)
    {
       
        if(CurrentSize+value >0 && CurrentSize+value < maxSize)
        {
            CurrentSize += value;
            inputField.text = CurrentSize.ToString();
        }
       
    }
    public void SetValue(string text)
    {
        int value;
        int.TryParse(text, out value);
        Debug.Log(value);
        if(value == 0)
        {
            CurrentSize = 0;
            return;
        }
        if (value > 0 && value < maxSize)
        {
            CurrentSize = value;
           
        }
        else if(value >=maxSize)
        {
            CurrentSize = maxSize - 1;
        }
        else
        {
            CurrentSize = 1;
        }
          
        inputField.text = CurrentSize.ToString();
    }
    
}
