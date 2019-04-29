using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;
/// <summary>
/// Alustaa vihollisen statit Scriptable objectista, tämän kaveriksi lisätään varmaan vielä muutama Class laskeen rolleja jne...
/// </summary>
public class EnemyStats : MonoBehaviour
{
    [Header ("Drag and Drop Enemy Scriptable Object here")]
    public EnemyStatsObject enemy;

    public string myName = "Hirvitys";

    public int xpReward;
    public int level;

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

    #region Enemy Resistances
    public CharacterStat fireResistance;
    public CharacterStat coldResistance;
    public CharacterStat poisonResistance;
    public CharacterStat lightningResistance;
    public CharacterStat physicalResistance;
    #endregion

    #region EnemyStatMods
    public StatModifier healthLoss;
    public StatModifier healthFill;
    public StatModifier manaLoss;
    public StatModifier manaFill;
    #endregion
 
    public EnemyType enemyType;
    public GroundEnemyType groundEnemyType;
    public FlyingEnemyType flyingEnemyType;
    public BossEnemyType bossEnemyType;
    public List<EnemyVariations> enemyVariations= new List<EnemyVariations>();

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

        fireResistance.BaseValue = enemy._fireResistance;
        poisonResistance.BaseValue = enemy._poisonResistance;
        coldResistance.BaseValue = enemy._frostResistance;
        lightningResistance.BaseValue = enemy._lightningResistance;
        physicalResistance.BaseValue = enemy._physicalResistance;

        xpReward = enemy._xpReward;
        enemyType = enemy._enemyType;
        level = enemy._level;


        if ( enemyType == EnemyType.GroundEnemy )
        {
            groundEnemyType = enemy._groundEnemyType;
        }
        else if ( enemyType == EnemyType.FlyingEnemy )
        {
            flyingEnemyType = enemy._flyingEnemyType;
        }
        else if ( enemyType == EnemyType.EliteEnemy )
        {          
            for ( int i = 0 ; i < enemy._enemyVariation.Length ; i++ )
            {
                enemyVariations.Add ( enemy._enemyVariation [ i ] );
            }        
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
   
    public float GetHealthPercent ( )
    {
        return health.Value / maxHealth.Value * 100f;
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
