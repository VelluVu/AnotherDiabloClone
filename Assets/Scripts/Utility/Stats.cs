using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Stats : MonoBehaviour
{
    public static Stats instance;
    string saveName = "/stats.dat";
    public int enemiesKilled = 0;
    public int itemsLooted = 0;
    public float playTime= 0;
    public float damageTaken= 0;
    public float damageDealt=0;
    public float fireDamageDealt = 0;
    public float coldDamageDealt = 0;
    public float physicalDamageDealt = 0;
    public float poisonDamageDealt = 0;
    public float lightningDamageDealt = 0;
    public float deaths = 0;
    


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }


    }
    private void Update()
    {
        playTime += Time.deltaTime;
    }
    private void OnEnable()
    {
        StateController.enemyDeathEvent += addEnemiesKilled;
        StateController.enemyTakeDamageEvent += addDamageDealt;
        LootText.itemLootEvent += addLootedItem;
        Player.playerDeathEvent += addDeath;
    }
    private void OnDisable()
    {
        StateController.enemyDeathEvent -= addEnemiesKilled;
        StateController.enemyTakeDamageEvent -= addDamageDealt;
        LootText.itemLootEvent -= addLootedItem;
        Player.playerDeathEvent -= addDeath;

    }
    public void addDeath(Transform trans)
    {
        deaths++;
    }
    public void addLootedItem(Rarity rarity)
    {
        itemsLooted++;
    }
    public void addDamageDealt(GameObject origin, float damage, DamageType damageType)
    {
        damageDealt += damage;
        if(damageType == DamageType.Fire)
        {
            fireDamageDealt += damage;
        }
        if (damageType == DamageType.Cold)
        {
            coldDamageDealt += damage;
        }
        if (damageType == DamageType.Poison)
        {
            poisonDamageDealt += damage;
        }
        if (damageType == DamageType.Physical)
        {
            physicalDamageDealt += damage;
        }
        if (damageType == DamageType.Lightning)
        {
            lightningDamageDealt += damage;
        }
    }
    public void addEnemiesKilled(Transform enemyTransform, int xpReward,StateController origin) // add different types of enemies killed
    {
        enemiesKilled++;
    }
    public void Save()
    {


        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveName);
        SaveStats data = new SaveStats();

        data.damageTaken = damageTaken;
        data.damageDealt = damageDealt;
        data.coldDamageDealt = coldDamageDealt;
        data.fireDamageDealt = fireDamageDealt;
        data.lightningDamageDealt = lightningDamageDealt;
        data.poisonDamageDealt = poisonDamageDealt;
        data.physicalDamageDealt = physicalDamageDealt;
        data.playTime = playTime;
        data.enemiesKilled = enemiesKilled;
        data.itemsLooted = itemsLooted;
        data.deaths = deaths;
        


        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + saveName))
        {
            Debug.Log("<color=red>" + "Stats Loaded" + "</color>");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveName, FileMode.Open);
            SaveStats data = (SaveStats)bf.Deserialize(file);
            file.Close();
            damageTaken = data.damageTaken;
            enemiesKilled = data.enemiesKilled;
            playTime = data.playTime;
            damageTaken = data.damageTaken;
            fireDamageDealt = data.fireDamageDealt;
            coldDamageDealt = data.coldDamageDealt;
            physicalDamageDealt = data.physicalDamageDealt;
            poisonDamageDealt = data.poisonDamageDealt;
            lightningDamageDealt = data.lightningDamageDealt;
            deaths = data.deaths;
           
        }
    }
}
[Serializable]
class SaveStats
{
    public int enemiesKilled = 0;
    public int itemsLooted = 0;
    public float playTime = 0;
    public float damageTaken = 0;
    public float damageDealt = 0;
    public float fireDamageDealt = 0;
    public float coldDamageDealt = 0;
    public float physicalDamageDealt = 0;
    public float poisonDamageDealt = 0;
    public float lightningDamageDealt = 0;
    public float deaths = 0;

}
