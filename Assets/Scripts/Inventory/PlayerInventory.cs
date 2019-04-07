using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class PlayerInventory : MonoBehaviour
{
    
    public static PlayerInventory instance;
    /*  TODO
     *  - Sort items/
     *  
     *  
     * 
     */
    public int gold; // raha
    public TMP_Text goldText; // rahan teksti
    public GameObject stackDividerObject; // objekti joka instantioidaan kun jaetaan itemi osiin
    [HideInInspector]
    public const int maxStackSize = 99;
    [HideInInspector]
    public const int InventorySlots = 24;
    public List<LootSlot> LootList; // inventory itemien lista
    public Transform LootHolder; // Lootit spawnaa tämän childiksi
    [HideInInspector]
    public bool splittingItem = false;// is item being split??
    [HideInInspector] public LootSlot splitItemObject;
    [HideInInspector]
    public bool draggingInventoryItem = false; // inventory item is being dragged
    #region equipmentSlots
   
    [System.Serializable]
    public struct equipmentSlots // Equipmentslot Structi
    {
        public ArmorSlot key;
        public EquipmentSlot ES;
    }
    public List<equipmentSlots> equipmentConnect = new List<equipmentSlots>();
    #endregion
    #region Dictionaries
    public Dictionary<Rarity, Color> rarity = new Dictionary<Rarity, Color>
    {
        {Rarity.Common,Color.white },
        {Rarity.Uncommon,Color.green},
        {Rarity.Rare,Color.blue },
        {Rarity.Epic,Color.yellow },
        {Rarity.Legendary,new Color(1,0.647f,0,1)},
        
    };
    public Dictionary<Rarity, string> rarityString = new Dictionary<Rarity, string>
    {
        {Rarity.Common,"Common"},
        {Rarity.Uncommon,"Uncommon"},
        {Rarity.Rare,"Rare"},
        {Rarity.Epic,"Epic"},
        {Rarity.Legendary,"Legendary"},

    };
    public Dictionary<ArmorSlot, string> typeDictionary = new Dictionary<ArmorSlot, string>
    {
        {ArmorSlot.Belt,"Belt"},{ArmorSlot.Boots,"Boots"},{ArmorSlot.Bracer,"Bracers"},
        {ArmorSlot.Chest,"Chest Armor"},{ArmorSlot.Glove,"Gloves"},{ArmorSlot.Helm,"Helm"},
        {ArmorSlot.Mainhand,"Weapon"},{ArmorSlot.Necklace,"Necklace"},{ArmorSlot.None,""},
        {ArmorSlot.Offhand,"Shield"},{ArmorSlot.Pants,"Pants"},{ArmorSlot.Ring,"Ring"},
        {ArmorSlot.Shoulder,"Shoulder"},{ArmorSlot.Consumable,"Consumable"},



    };
    #endregion
    #region Saving
    public List<RolledLoot> saveIdInventory = new List<RolledLoot>();
    public List<RolledLoot> saveIdEquip = new List<RolledLoot>();
    public List<int> saveStackSize = new List<int>();
    string saveName = "/inventory.dat"; // used for saving
    public Loot[] allLoot;
    RolledLoot placeHolder;
    #endregion
    

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
        allLoot = Resources.FindObjectsOfTypeAll<Loot>();
        

    }
    public void AddItem(RolledLoot loot, int count,bool autoEquip)
    {

        if (loot.stackable)
        {
            foreach (LootSlot slot in LootList)
            {
                if (slot.checkIfAddToStack(loot, count))
                {
                    
                    return;
                }
            }
        }
        foreach(LootSlot slot in LootList)
        {
            if (slot.addToSlot(loot, count,autoEquip))
            {
                
                
                return;
            }
        }
        
    }
    private void Update()
    {
        if (DevMode.instance.DevModeOn)
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                LoadInventory();
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                SaveInventory();
            }
        }
       
        RefreshMoney();
    }
    public void AddMoney(int amount)
    {
        if(amount < 0)
        {
            if(gold + amount < 0)
            {
                Debug.LogWarning("Not enough gold");
                return;
            }
            
        }
        RefreshMoney();
        gold += amount;
        
    }
    public void RefreshMoney()
    {
        goldText.text = gold.ToString();
    }
    void emptyInventory()
    {
        foreach (LootSlot slot in LootList)
        {
            slot.emptySlot();

        }
    }
    void emptyEquipment()
    {
        foreach (equipmentSlots slot in equipmentConnect)
        {
            slot.ES.emptySlot();
            foreach (RollAttribute r in slot.ES.item.attributes)
            {
                r.Unequip();
            }
        }
    }
    void ClearLists()
    {
        saveIdEquip.Clear();
        saveIdInventory.Clear();
        saveStackSize.Clear();
    }
    public void SaveInventory()
    {

        ClearLists();
        foreach(LootSlot slot in LootList)
        {
            if (!slot.isEmpty)
            {
                saveIdInventory.Add(slot.item);
                saveStackSize.Add(slot.stackSize);
                
            }
            
        }
        foreach(equipmentSlots slot in equipmentConnect)
        {
            if (!slot.ES.isEmpty)
            {
                saveIdEquip.Add(slot.ES.item);
            }
        }
        saveName = "/inventory.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveName);
        SaveData data = new SaveData();
        for(int i = 0; i < saveIdInventory.Count; i++)
        {
            data.lootID.Add(saveIdInventory[i].originLoot.itemID);
            data.lootCount.Add(saveStackSize[i]);
            
            Debug.Log(saveIdInventory[i].originLoot.itemID);
            Debug.Log(saveStackSize[i]);
        }
        for(int i = 0; i <saveIdEquip.Count;i++)
        {
            data.equipId.Add(saveIdEquip[i].originLoot.itemID);
        }
          
           
            
        
        

        bf.Serialize(file, data);
        file.Close();
    }
  
    public void LoadInventory()
    {
        if (File.Exists(Application.persistentDataPath + saveName))
        {
            Debug.Log("HELOOOEE");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveName, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            emptyEquipment();
            emptyInventory();
            for(int i = 0;i<data.lootID.Count;i++)
            {
                for(int j = 0; j < allLoot.Length; j++)
                {
                    if(allLoot[j].itemID == data.lootID[i])
                    {
                        placeHolder = gameObject.AddComponent<RolledLoot>();
                        placeHolder.rollLoot(allLoot[j]);
                        AddItem(placeHolder,data.lootCount[i], false);
                        Destroy(placeHolder);
                    }
                }
            }
            //add equipped items
            for(int i = 0; i < data.equipId.Count; i++)
            {
                for (int j = 0; j < allLoot.Length; j++)
                {
                    if (allLoot[j].itemID == data.equipId[i])
                    {
                        placeHolder = gameObject.AddComponent<RolledLoot>();
                        placeHolder.rollLoot(allLoot[j]);
                        AddItem(placeHolder, 1, true);
                        Destroy(placeHolder);
                    }
                }
            }
            

        }
    }

}

[Serializable]
class SaveData
{
    
    public List<int> lootID = new List<int>();
    public List<int> lootCount = new List<int>();

    public List<int> equipId = new List<int>();
    
    //public List<int> rolledValue1;
    //public List<int> rolledValue2;
    //public List<int> rolledValue3;

}


