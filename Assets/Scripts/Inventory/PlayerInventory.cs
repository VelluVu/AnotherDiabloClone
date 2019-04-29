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
    [HideInInspector] public bool loading;

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
    public Dictionary<Rarity,int> rarityLootCountPrimary = new Dictionary<Rarity, int> ()
    {
        {Rarity.Common,1},
        {Rarity.Uncommon,2},
        {Rarity.Rare,3},
        {Rarity.Epic,4},
        {Rarity.Legendary,5},
    };
    public Dictionary<Rarity,int> rarityLootCountSecondary= new Dictionary<Rarity, int> ()
    {
        {Rarity.Common,1},
        {Rarity.Uncommon,1},
        {Rarity.Rare,2},
        {Rarity.Epic,2},
        {Rarity.Legendary,2},
    };
    public Dictionary<ArmorSlot, string> typeDictionary = new Dictionary<ArmorSlot, string>
    {
        {ArmorSlot.Belt,"Belt"},{ArmorSlot.Boots,"Boots"},{ArmorSlot.Bracer,"Bracers"},
        {ArmorSlot.Chest,"Chest Armor"},{ArmorSlot.Glove,"Gloves"},{ArmorSlot.Helm,"Helm"},
        {ArmorSlot.Mainhand,"Weapon"},{ArmorSlot.Necklace,"Necklace"},{ArmorSlot.None,""},
        {ArmorSlot.Offhand,"Shield"},{ArmorSlot.Pants,"Pants"},{ArmorSlot.Ring,"Ring"},
        {ArmorSlot.Shoulder,"Shoulder"},{ArmorSlot.Consumable,"Consumable"},



    };
    public Dictionary<ArmorSlot, float> armorSlotArmorModifier = new Dictionary<ArmorSlot, float>
    {
        {ArmorSlot.Belt,0.4f},{ArmorSlot.Boots,0.4f},{ArmorSlot.Bracer,0.4f},
        {ArmorSlot.Chest,1f},{ArmorSlot.Glove,0.4f},{ArmorSlot.Helm,1f},
        {ArmorSlot.Pants,0.8f},
        {ArmorSlot.Shoulder,0.6f}

    };
    #endregion
    #region Saving
    string saveName = "/inventory.dat"; // used for saving
    public Loot[] allLoot;
    RolledLoot placeHolder;
    public LootSlot consumablespot;
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
        int lootCount = 0;
        allLoot = Resources.FindObjectsOfTypeAll<Loot>();
        foreach (Loot loot in allLoot)
        {
            loot.itemID = lootCount;
            lootCount++;
        }   
    }
    public bool IsLootSlotsFull()
    {
        foreach(LootSlot slot in LootList)
        {
            if (slot.isEmpty)
            {
                return false;
            }
        }
        return true;
    }
    public void AddItem(RolledLoot loot, int count,bool autoEquip)
    {
        if (IsLootSlotsFull())
        {
            Debug.LogWarning("Inventory Full");
            return;
        }
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
    public void SaveInventory()
    {

       
        saveName = "/inventory.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveName);
        SaveData data = new SaveData();
        for(int i = 0; i < LootList.Count; i++)
        {
            if (!LootList[i].isEmpty)
            {
                int attributeCount = 0;
                data.lootID.Add(LootList[i].item.originLoot.itemID);
                data.lootStackSize.Add(LootList[i].stackSize);
                foreach(RollAttribute roll in LootList[i].item.attributes)
                {
                    attributeCount++;
                    data.attributeValue.Add(roll.value);
                    data.attributeValue2.Add(roll.value2Min);
                    data.attributeId.Add(roll.originAttribute.id);
                }
                data.attributeAmount.Add(attributeCount);
            }
        }

        for(int i = 0; i < equipmentConnect.Count;i++)
        {
            if (!equipmentConnect[i].ES.isEmpty)
            {
                int attributeCount = 0;
                data.equipId.Add(equipmentConnect[i].ES.item.originLoot.itemID);
                
                foreach (RollAttribute roll in equipmentConnect[i].ES.item.attributes)
                {
                attributeCount++;
                data.equipAttributeValue.Add(roll.value);
                data.equipAttributeValue2.Add(roll.value2Min);
                data.equipAttributeId.Add(roll.originAttribute.id);
                }
               
                
                data.equipAttributeAmount.Add(attributeCount);
            }
        }
        if(!consumablespot.isEmpty)
        {
            data.consumableID = consumablespot.item.originLoot.itemID;
            data.consumableStackSize = consumablespot.stackSize;
        }
        data.Money = gold;
       
        
          
           
            
        
        

        bf.Serialize(file, data);
        file.Close();
    }
  
    public void LoadInventory()
    {
        if (File.Exists(Application.persistentDataPath + saveName))
        {
            Debug.Log("Inventory Loaded");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveName, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            emptyEquipment();
            emptyInventory();
            //add equipped items
            loading = true;
            int equipCount = 0;
            for (int i = 0; i < data.equipId.Count; i++)
            {
                for (int j = 0; j < allLoot.Length; j++)
                {
                    if (allLoot[j].itemID == data.equipId[i])
                    {
                        placeHolder = gameObject.AddComponent<RolledLoot>();
                        placeHolder.rollLoot(allLoot[j]);
                        for (int k = 0; k < data.equipAttributeAmount[i]; k++)
                        {
                            Debug.Log(k);
                            RollAttribute tempAttribute =gameObject.AddComponent<RollAttribute>();
                            placeHolder.attributes.Add(tempAttribute);
                            placeHolder.attributes[k].rolledLoot = placeHolder;
                            placeHolder.attributes[k] = tempAttribute.rollAttribute(AttributePerSlot.instance.allAttributes[data.equipAttributeId[equipCount + k]]);
                            placeHolder.attributes[k].value = data.equipAttributeValue[equipCount + k];
                            placeHolder.attributes[k].value2Min = data.equipAttributeValue2[equipCount + k];
                            Destroy(tempAttribute);

                        }
                        equipCount += data.equipAttributeAmount[i];
                        AddItem(placeHolder, 1, true);
                        Destroy(placeHolder);
                    }
                }
            }
            int count = 0;
            for (int i = 0;i<data.lootID.Count;i++)
            {
                for(int j = 0; j < allLoot.Length; j++)
                {
                    if(allLoot[j].itemID == data.lootID[i])
                    {
                        
                        placeHolder = gameObject.AddComponent<RolledLoot>();
                        placeHolder.rollLoot(allLoot[j]);
                        for(int k = 0;k < data.attributeAmount[i]; k++)
                        {

                            RollAttribute tempAttribute =gameObject.AddComponent<RollAttribute>();
                            placeHolder.attributes.Add(tempAttribute);
                            placeHolder.attributes[k].rolledLoot = placeHolder;
                            placeHolder.attributes[k] = tempAttribute.rollAttribute(AttributePerSlot.instance.allAttributes[data.attributeId[count+k]]);
                            placeHolder.attributes[k].value = data.attributeValue[count+k];
                            placeHolder.attributes[k].value2Min = data.attributeValue2[count+k];
                            Destroy(tempAttribute);
                            
                        }
                        count += data.attributeAmount[i];
                       
                        AddItem(placeHolder,data.lootStackSize[i], false);
                        Destroy(placeHolder);
                    }
                }
            }
            if(data.consumableID != -1)
            {
                consumablespot.emptySlot();
                consumablespot.item.attributes.Clear();
                consumablespot.item.rollLoot(allLoot[data.consumableID]);
                consumablespot.stackSize = data.consumableStackSize;
                consumablespot.stackSizeTextEnable(true);
                consumablespot.UnequipItem();
                gold = data.Money;
            }
            else
            {
                consumablespot.emptySlot();
            }


            loading = false;
        }
    }

}

[Serializable]
class SaveData
{
    
    //inventory
    public List<int> lootID = new List<int>();
    public List<int> lootStackSize = new List<int>(); // stackSize
    public List<int> attributeAmount = new List<int>();
    public List<float> attributeValue = new List<float>();
    public List<float> attributeValue2 = new List<float>();
    public List<int> attributeId = new List<int>();

    public int Money;

//equipped
    public List<int> equipId = new List<int>();
    public List<int> equipAttributeAmount = new List<int>();
    public List<float> equipAttributeValue = new List<float>();
    public List<float> equipAttributeValue2 = new List<float>();

     public List<int> equipAttributeId = new List<int>();



    //Save consumablespot
    public  int consumableID = -1;
    public  int consumableStackSize;

}


