using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeAndManaTracker : MonoBehaviour
{

    public Image healthPool;
    public Image manaPool;

    private void OnEnable ( )
    {
        Player.playerTakeDamageEvent += LoseHealth;
        Player.playerHealHealthEvent += HealHealth;
        Player.playerRestoreManaEvent += RestoreMana;
        Player.playerUseManaEvent += LoseMana;
        PlayerClass.passiveManaRegenEvent += ManaRegen;   
    }

    private void OnDisable ( )
    {
        Player.playerTakeDamageEvent -= LoseHealth;
        Player.playerHealHealthEvent -= HealHealth;
        Player.playerRestoreManaEvent -= RestoreMana;
        Player.playerUseManaEvent -= LoseMana;
        PlayerClass.passiveManaRegenEvent -= ManaRegen;    
    }

    public void ManaRegen ( float value )
    {
        manaPool.fillAmount += value * ( 1 / PlayerClass.instance.maxMana.Value );
    }

    /// <summary>
    /// Päivittää healthpoolia
    /// </summary>
    /// <param name="value"></param>
    public void LoseHealth ( float value )
    {
        healthPool.fillAmount -= value * ( 1 / PlayerClass.instance.maxHealth.Value );
    }

    /// <summary>
    /// Päivittää manapoolia
    /// </summary>
    /// <param name="value"></param>
    public void LoseMana ( float value )
    {
        manaPool.fillAmount -= value * ( 1 / PlayerClass.instance.maxMana.Value );
    }

    private void HealHealth ( float amount, bool restoreAll )
    {
        if ( restoreAll )
        {
            healthPool.fillAmount = 1f;
        }
        else
        {
            healthPool.fillAmount += amount * ( 1 / PlayerClass.instance.maxHealth.Value );
        }
    }

    private void RestoreMana ( float amount, bool restoreAll )
    {
        if ( restoreAll )
        {
            manaPool.fillAmount = 1f;
        }
        else
        {
            manaPool.fillAmount += amount * ( 1 / PlayerClass.instance.maxMana.Value );
        }
    }

}
