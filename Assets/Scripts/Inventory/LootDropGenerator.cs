using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropGenerator : MonoBehaviour
{
    public static LootDropGenerator instance;
    public GameObject dropObject;
    public GameObject goldDrop;
    public Transform LootHolder;
    Rarity rarityChecker;
    Dictionary<Rarity, int> chestRarity = new Dictionary<Rarity, int>()
    {
        {Rarity.Common,10 },
        {Rarity.Uncommon,30 },
        {Rarity.Rare,60},
        {Rarity.Epic,80 },
        {Rarity.Legendary,100 },


    };
    // Start is called before the first frame update
    void Awake()
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
    private void OnEnable()
    {
        StateController.enemyDeathEvent += CreateDrop;
    }
    private void OnDisable()
    {
        StateController.enemyDeathEvent -= CreateDrop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateDrop(Transform trans,int number,StateController origin)
    {
        List<Loot> tempRarityLootList = new List<Loot>();
        int randomInt = Random.Range(1, 101);
        if(randomInt == 100) {
            randomInt += Random.Range(1, 101);
        }
        randomInt += number;
        int randomGoldInt =Random.Range(1, 101);
        Debug.Log("Random Gold Int: " + randomGoldInt);
        if (randomGoldInt > 30)
        {
            CreateGoldDrop(number,trans,randomGoldInt,origin);
        }
        
        Debug.Log("XP reward: " + number);
        Debug.Log("Random int: " + randomInt);
        if(randomInt<60)
        {
            Debug.Log("No drop");
            return;
        }
        else
        {
            GameObject createdObject = Instantiate(dropObject, LootHolder);
            createdObject.transform.position = trans.position;
            createdObject.transform.rotation = Quaternion.identity;
            createdObject.transform.SetParent(LootHolder);
            if(randomInt <= 99)
            {
                rarityChecker = Rarity.Common;
            }
            else if(randomInt >99 && randomInt< 130)
            {
                rarityChecker = Rarity.Uncommon;
            }
            else if (randomInt >= 130 && randomInt < 170)
            {
                rarityChecker = Rarity.Rare;
            }
            else if (randomInt >= 170 && randomInt <= 195)
            {
                rarityChecker = Rarity.Epic;
            }
            else
            {
                rarityChecker = Rarity.Legendary;
            }
            foreach(Loot loot in PlayerInventory.instance.allLoot)
            {
                if(loot.rarity == rarityChecker)
                {
                    tempRarityLootList.Add(loot);
                    Debug.Log("AddedLoot to templist");
                }
            }
            createdObject.GetComponent<PickUpLoot>().loot = tempRarityLootList[Random.Range(0, tempRarityLootList.Count)];
            if (createdObject.GetComponent<PickUpLoot>().loot.consumable)
            {
                createdObject.GetComponent<PickUpLoot>().Count = Random.Range(1, 20); // on nyt vaan asettettu arvo
            }
            createdObject.GetComponent<PickUpLoot>().setupDone = true;
            createdObject.GetComponent<PickUpLoot>().Setup(origin.enemyStats.level);

        }
        
        
    }
    public void CreateGoldDrop(int xp,Transform trans,int randomRoll,StateController origin)
    {
        GameObject gold = Instantiate(goldDrop, LootHolder.transform);
        gold.transform.position = trans.position;
        
        gold.GetComponent<Gold>().amount = (randomRoll + xp )*origin.enemyStats.level;
        Debug.Log("Gold Dropped: " + (randomRoll + xp)*origin.enemyStats.level);
    }
    public void ChestDrop(Transform trans,Rarity rarity,int chestLevel)
    {
        List<Loot> tempRarityLootList = new List<Loot>();
        List<Rarity> tempRarityList = new List<Rarity>();
        int randomAmountRoll = 1;
        if (rarity == Rarity.Common)
        {
            randomAmountRoll = Random.Range(1,2);
            
        }
        else if(rarity == Rarity.Uncommon)
        {
            randomAmountRoll = Random.Range(1, 3);
        }
        else if(rarity == Rarity.Rare)
        {
            randomAmountRoll = Random.Range(1, 4);
        }
        else if (rarity == Rarity.Epic)
        {
            randomAmountRoll = Random.Range(2, 4);
        }
        else if (rarity == Rarity.Legendary)
        {
            randomAmountRoll = Random.Range(3, 5);
        }
        int reduceBy = 0; 
        for(int i = 0; i < randomAmountRoll; i++)
        {
            
            int randomRoll = Random.Range(1, 101);
            if(randomRoll == 100)
            {
                randomRoll += Random.Range(1, 101);
            }
            randomRoll += chestRarity[rarity];
            randomRoll -= reduceBy;
            if (randomRoll <= 99)
            {
                tempRarityList.Add(Rarity.Common);
            }
            else if (randomRoll > 99 && randomRoll < 130)
            {
                tempRarityList.Add(Rarity.Uncommon);
              
            }
            else if (randomRoll >= 130 && randomRoll < 170)
            {
                tempRarityList.Add(Rarity.Rare);
                
            }
            else if (randomRoll >= 170 && randomRoll <= 195)
            {
                tempRarityList.Add(Rarity.Epic);
                
            }
            else
            {
                tempRarityList.Add(Rarity.Legendary);
               
            }
            reduceBy += 30;
        }
        for(int i = 0; i < randomAmountRoll; i++)
        {
            
            rarityChecker = tempRarityList[i];
            foreach (Loot loot in PlayerInventory.instance.allLoot)
            {
                if (loot.rarity == rarityChecker)
                {
                    tempRarityLootList.Add(loot);
                    Debug.Log("AddedLoot to templist");
                }
            }

            GameObject createdObject = Instantiate(dropObject, LootHolder);
            createdObject.transform.position = trans.position + new Vector3(0, 0.5f) ;
            createdObject.transform.SetParent(LootHolder);
            createdObject.GetComponent<PickUpLoot>().loot = tempRarityLootList[Random.Range(0, tempRarityLootList.Count)];
            if (createdObject.GetComponent<PickUpLoot>().loot.consumable)
            {
                createdObject.GetComponent<PickUpLoot>().Count = Random.Range(1, 20); // on nyt vaan asettettu arvo
            }
            createdObject.GetComponent<PickUpLoot>().Setup(chestLevel);
        }
        




    }
}
