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
    public void CreateDrop(Transform trans,int number)
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
            CreateGoldDrop(number,trans,randomGoldInt);
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
            createdObject.GetComponent<PickUpLoot>().Setup(2);

        }
        
        
    }
    public void CreateGoldDrop(int xp,Transform trans,int randomRoll)
    {
        GameObject gold = Instantiate(goldDrop, LootHolder.transform);
        gold.transform.position = trans.position;
        
        gold.GetComponent<Gold>().amount = randomRoll + xp;
        Debug.Log("Gold Dropped: " + randomRoll + xp);
    }
}
