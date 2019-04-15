using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats instance;

    public int enemiesKilled = 0;
    public int itemsLooted = 0;
    public float playTime= 0;
    public float damageTaken= 0;
    public float damageDealt=0;
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
    }
    private void OnDisable()
    {
        StateController.enemyDeathEvent -= addEnemiesKilled;

    }
    public void addEnemiesKilled(Transform enemyTransform, int xpReward)
    {
        enemiesKilled++;
    }
}
