using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    /*  TODO
     *  - Sort items/
     *  
     *  
     * 
     */

    [HideInInspector]
    public const int maxStackSize = 99;
    [HideInInspector]
    public const int InventorySlots = 24;
    public List<LootSlot> LootList;
    [System.Serializable]
    public struct equipmentSlots
    {
        public string key;
        public EquipmentSlot ES;
    }
    public List<equipmentSlots> equipmentConnect = new List<equipmentSlots>();
    public Dictionary<string, Color> rarity = new Dictionary<string, Color>
    {
        {"Common",Color.white },
        {"Uncommon",Color.green},
        {"Rare",Color.blue },
        {"Epic",Color.red },
        {"Legendary",new Color(1,0.647f,0,1)},
        {"",new Color(0,0,0,0) }
    };


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        

    }
    public void AddItem(RolledLoot loot, int count)
    {
        
        
        foreach(LootSlot slot in LootList)
        {
            if (slot.addToSlot(loot, count))
            {
                Debug.Log("Picked Up "+loot.itemName);
                return;
            }
        }
    }
    
}
