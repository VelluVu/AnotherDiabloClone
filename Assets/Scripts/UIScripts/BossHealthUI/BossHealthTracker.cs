using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthTracker : MonoBehaviour
{

    public Image healthFill;
    EnemyStats trackedStats;
    public GameObject bos;

    private void OnEnable ( )
    {    
        BossEventTrigger.BossEvent += BossStarted;
    }

    private void OnDisable ( )
    {    
        BossEventTrigger.BossEvent -= BossStarted;
    }

    public void BossStarted( GameObject currentBoss )
    {
        trackedStats = currentBoss.GetComponent<EnemyStats> ( );
        bos = currentBoss;
    }

    private void Update ( )
    {
        UpdateHealthBar ( );
    }

    void UpdateHealthBar()
    {
        healthFill.fillAmount = trackedStats.health.Value / trackedStats.maxHealth.Value;

        if( healthFill.fillAmount <= 0)
        {
            gameObject.SetActive ( false );
        }
    }
}
