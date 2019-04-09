using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;
/// <summary>
/// Alustaa vihollisen statit Scriptable objectista, tämän kaveriksi lisätään varmaan vielä muutama Class laskeen rolleja jne...
/// </summary>
public class EnemyStats : MonoBehaviour
{
    
    public EnemyStatsObject enemy;

    public string myName = "Hirvitys";

    public int xpReward;

    #region enemyStats
    public CharacterStat moveSpeed;
    public CharacterStat jumpForce;
    public CharacterStat attackSpeed;
    public CharacterStat attackDamage;
    public CharacterStat maxHealth;
    public CharacterStat health;
    public CharacterStat maxMana;
    public CharacterStat mana;
    public CharacterStat armor;
    public CharacterStat chaseMultiplier;
    #endregion

    #region EnemyStatMods
    public StatModifier healthLoss;
    public StatModifier healthFill;
    public StatModifier manaLoss;
    public StatModifier manaFill;
    #endregion

    [Header ( "Kuinka nopeesti liikkuu jahdatessa pelaajaa" )]
    
    public EnemyType enemyType;
    public GroundEnemyType groundEnemyType;
    public FlyingEnemyType flyingEnemyType;
    public BossEnemyType bossEnemyType;
    public EnemyVariations[] enemyVariations;

    private void Awake ( )
    {
        myName = enemy._name;
        moveSpeed.BaseValue = enemy._moveSpeed;
        chaseMultiplier.BaseValue = enemy._chaseMultiplier;
        jumpForce.BaseValue = enemy._jumpForce;
        attackSpeed.BaseValue = enemy._attackSpeed;
        attackDamage.BaseValue = enemy._attackDamage;
        health.BaseValue = enemy._health;
        maxHealth.BaseValue = enemy._health;
        mana.BaseValue = enemy._mana;
        maxMana.BaseValue = enemy._mana;
        armor.BaseValue = enemy._armor;
        xpReward = enemy._xpReward;
        enemyType = enemy._enemyType;

        if ( enemyType == EnemyType.GroundEnemy )
        {
            groundEnemyType = enemy._groundEnemyType;
        }
        else if(enemyType == EnemyType.FlyingEnemy)
        {
            flyingEnemyType = enemy._flyingEnemyType;
        }
        else if ( enemyType == EnemyType.EliteEnemy )
        {
            enemyVariations = enemy._enemyVariation;
        }
        else if(enemyType == EnemyType.Boss)
        {
            bossEnemyType = enemy._bossEnemyType;
        }       
    }

    private void OnEnable ( )
    {
        CheckPoint.checkPointEvent += ResetEnemy;
    }

    private void OnDisable ( )
    {
        CheckPoint.checkPointEvent -= ResetEnemy;
    }

    private void Update ( )
    {
        EnemyCheckHealth ( );
    }

    public void EnemyCheckHealth()
    {
        if ( health.BaseValue >= maxHealth.Value)
        {
            health.BaseValue = maxHealth.Value;
        }

        if (mana.BaseValue >= maxMana.Value)
        {
            mana.BaseValue = maxMana.Value;
        }
    }
   
    public void ResetEnemy( )
    {
        health.RemoveAllModifiersFromSource ( healthLoss );
        health.RemoveAllModifiersFromSource ( healthFill );
        health.BaseValue = maxHealth.BaseValue;
        mana.BaseValue = maxMana.BaseValue;
    }

    public void RestoreEnemyHealth( float amount)
    {
        health.BaseValue += amount;
    }

    public void RestoreEnemyMana(float amount)
    {
        mana.BaseValue += amount;
    }

}
