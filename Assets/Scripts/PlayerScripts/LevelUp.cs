using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;

/// <summary>
/// Hoitaa hahmon Level upit
/// </summary>
public class LevelUp : MonoBehaviour
{
  
    public CharacterStat currentXP;
    [Header ("TO BOOST XP gains, add modifiers to this stat ( 1.0 +)")]
    public CharacterStat xpBoost;
    [Header ("XP reguired to Level Up")]
    public CharacterStat toLevelXP;
    [Header ("XP reguirement to next level multiplier")]
    public float toLevelXPMultiplier;
    [Header ( "STATPOINTS gained on level up" )]
    public float statPointGain;

    public delegate void OnLevelUpDelegate ( );
    public static event OnLevelUpDelegate levelUpEvent;

    private void OnEnable ( )
    {
        StateController.enemyDeathEvent += GetEnemyDeathXpReward;
    }

    private void OnDisable ( )
    {
        StateController.enemyDeathEvent -= GetEnemyDeathXpReward;
    }

    public void GetEnemyDeathXpReward ( Transform enemyTransform , int xpReward )
    {
        GainXP ( xpReward, ReferenceHolder.instance.player.stats );
        
    }

    /// <summary>
    /// Lisää XPtä hahmolle!
    /// </summary>
    /// <param name="xpAmount"></param>
    /// <param name="playerClass"></param>
    public void GainXP ( int xpAmount, PlayerClass playerClass )
    {
        if(xpBoost.Value < 1.0f)
        {
            xpBoost.BaseValue = 1.0f;
        }
        currentXP.BaseValue += xpAmount * xpBoost.Value;

        if(currentXP.Value >= toLevelXP.Value)
        {
            currentXP.BaseValue = 0;
            toLevelXP.BaseValue *= toLevelXPMultiplier;
            GainLevel ( playerClass , 1 );
        }
    }

    /// <summary>
    /// Lisää Level hahmolle , tapahtuu myös kun current xp >= toLevelXP
    /// statPojot
    /// ja vakio lisät?
    /// </summary>
    /// <param name="playerClass"></param>
    public void GainLevel ( PlayerClass playerClass, int levelAmount )
    {
      
        playerClass.playerLevel.BaseValue += levelAmount;
        playerClass.statPoint.BaseValue += statPointGain;
        playerClass.health.BaseValue += playerClass.playerLevel.Value * playerClass.endurance.Value;
        playerClass.mana.BaseValue += playerClass.playerLevel.Value * playerClass.energy.Value;
        //gameObject.GetComponent<Player> ( ).PlayerMessage ( "Level " + playerClass.playerLevel.Value );

        //Lisätään vaan joka toinen level yks skillpojo
        if ( playerClass.playerLevel.Value % 2 == 0 )
        {
            playerClass.skillPoint.BaseValue += 1;
        }

        if ( levelUpEvent != null )
        {
            Debug.Log ( "Level Up" );
            levelUpEvent ( );
        }
    }

}
