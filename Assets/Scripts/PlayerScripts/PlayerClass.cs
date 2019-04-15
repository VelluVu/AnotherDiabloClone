using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats; //Character stat assetin käyttäminen
using TMPro;

/// <summary>
/// Alustaa pelaajan valitseman classin, ja sisältää pelaajan statit, Attribuutit, skillpojot, level jne.
/// </summary>
public class PlayerClass : MonoBehaviour
{

    public static PlayerClass instance; // tekee tästä skriptin joka löytyy PlayerClass.instance
    [Tooltip ( "This List Includes all possible classes" )]
    public List<PlayerStatsObject> classes = new List<PlayerStatsObject> ( );

    public List<CharacterStat> listCharacterStats = new List<CharacterStat> ( );
    public string myName = "Jomppe";
    public string className = "Nephalem";
    public ClassType classType;

    #region MovementStats
    public CharacterStat moveSpeed;
    public CharacterStat jumpForce;
    public CharacterStat extraJumpForce;
    public CharacterStat extraWallJumpForce;
    #endregion

    #region CombatStats
    [Header ("All Player Stats")]
    public CharacterStat baseDamage;
    public CharacterStat baseDamageMax;
    public CharacterStat baseAttackSpeed;
    public CharacterStat maxHealth;
    public CharacterStat health;
    public CharacterStat maxMana;
    public CharacterStat mana;
    public CharacterStat stamina;
    public CharacterStat armor;
    public CharacterStat healthRegeneration;
    public CharacterStat healthOnKill;
    
    public CharacterStat cooldownReduction;
    public CharacterStat criticalHitChance;
    public CharacterStat criticalHitDamage;
    public CharacterStat manaCostReduction;
    public CharacterStat meleeDamageReduction;
    public CharacterStat rangedDamageReduction;
   
    #region elemental resistances
    public CharacterStat fireResistance;
    public CharacterStat coldResistance;
    public CharacterStat poisonResistance;
    public CharacterStat lightningResistance;
    public CharacterStat physicalResistance;
    public CharacterStat allResistance;
    #endregion
    #endregion

    #region Utility Stats
    public CharacterStat goldFind;
    public CharacterStat magicFind;
    public CharacterStat pickupRadius;
    #endregion

    //Tässä vois olla vaikka boostit jne..
    #region StatModifiers
    public StatModifier healthLoss;
    public StatModifier healthFill;
    public StatModifier manaLoss;
    public StatModifier manaFill;
    #endregion

    #region BaseStats and Leveling Stats
    public CharacterStat strength;
    public CharacterStat dexterity;
    public CharacterStat endurance;
    public CharacterStat energy;
    public CharacterStat statPoint;
    public CharacterStat skillPoint;
    public CharacterStat playerLevel;
    #endregion

    #region Delegaatit
    public delegate void PassiveManaRegenDelegate ( float amount );
    public static event PassiveManaRegenDelegate passiveManaRegenEvent;
    #endregion

    #region bools
    public bool fullHealth = false; // if health is full
    public bool fullMana = false; // if mana is full 
    public int chosenClass = 0; //tää muuttuu sitten ku class selection ui on tehty
    public static bool isInitStats = false; //jos uusiHahmo asetetaan trueksi hahmon tehdessä ja tästä eteenpäin pitäisi olla false...
    #endregion

    private void Awake ( )
    {
        if ( instance == null )
        {
            instance = this;
        }
        else if ( instance != this )
        {
            Destroy ( this );
        }
        AddToList ( );
        isInitStats = true; //alustetaan vielä tässä vaiheessa ennen character selection UI:ta trueksi is initStats
        if ( isInitStats )
        {
            isInitStats = false;
            InitializeHeroClass ( );
        }

    }

    private void Start ( )
    {
        checkForChanges ( );
    }

    private void OnEnable ( )
    {
        CheckPoint.checkPointEvent += PlayerEnterCheckPoint;
    }

    private void OnDisable ( )
    {
        CheckPoint.checkPointEvent -= PlayerEnterCheckPoint;
    }

    public void checkForChanges ( )
    {
        foreach ( CharacterStat cStat in listCharacterStats )
        {
            cStat.isDirty = true;
        }
        BuildSmallBoxStatsText ( );
    }

    private void Update ( )
    {
        checkHealth ( );
        PassiveManaRegen ( );
    }

    public void checkHealth ( ) // tarkistetaan että health ei ole suurempi kuin maxHealth;
    {

        if ( health.Value >= maxHealth.Value )
        {

            //health.AddModifier(new StatModifier(-(health.Value - maxHealth.Value), StatModType.Flat));

            health.RemoveAllModifiers ( );
            health.BaseValue = maxHealth.Value;
            fullHealth = true;
            //Debug.Log ( health.Value );


        }
        else
        {
            fullHealth = false;
        }
        if ( mana.Value >= maxMana.Value )
        {
            //mana.AddModifier(new StatModifier(-(mana.Value - maxMana.Value), StatModType.Flat));
            mana.BaseValue = maxMana.Value;
            mana.RemoveAllModifiers ( );

            fullMana = true;
        }
        else
        {
            fullMana = false;
        }
    }

    public void PassiveManaRegen()
    {
        mana.BaseValue += maxMana.Value * 0.01f * Time.deltaTime;

        if(passiveManaRegenEvent != null)
        {
            passiveManaRegenEvent ( maxMana.Value * 0.01f * Time.deltaTime );
        }
    }

    public void PlayerEnterCheckPoint ( )
    {
        FullyRestoreHealthAndMana ( );
    }

    public void FullyRestoreHealthAndMana ( )
    {
        FullyRestoreHealth ( );
        FullyRestoreMana ( );
        
    }

    public void FullyRestoreHealth ( )
    {
        health.RemoveAllModifiersFromSource ( healthLoss );
        health.RemoveAllModifiersFromSource ( healthFill );
        health.BaseValue = maxHealth.Value;
    }

    public void FullyRestoreMana ( )
    {
        mana.RemoveAllModifiersFromSource ( manaLoss );
        mana.RemoveAllModifiersFromSource ( manaFill );
        mana.BaseValue = maxMana.BaseValue;
    }

    public void RestoreHealth ( float amount )
    {
        health.BaseValue += amount;
    }

    public void RestoreMana ( float amount )
    {
        mana.BaseValue += amount;
    }

    /// <summary>
    /// Resetoi statit takas valitun classin mukaisiksi
    /// </summary>
    public void ResetStats ( )
    {

        InitializeHeroClass ( );

    }

    /// <summary>
    /// Asettaa valitun classin indexin muuttujaan chosenClass
    /// </summary>
    /// <param name="chosenClass"></param>
    public void ChosenClass ( int chosenClass )
    {
        this.chosenClass = chosenClass;
    }

    /// <summary>
    /// Pelin alussa alustaa pelaajan statit valitun classin mukaan
    /// </summary>
    public void InitializeHeroClass ( )
    {
        myName = classes [ chosenClass ]._name;
        className = classes [ chosenClass ]._className;
        classType = classes [ chosenClass ]._classType;

        extraJumpForce.BaseValue = classes [ chosenClass ]._extraJumpForce;
        moveSpeed.BaseValue = classes [ chosenClass ]._moveSpeed;
        jumpForce.BaseValue = classes [ chosenClass ]._jumpForce;
        extraWallJumpForce.BaseValue = classes [ chosenClass ]._extraWallJumpForce;

        baseDamage.BaseValue = classes [ chosenClass ]._baseDamage;
        baseAttackSpeed.BaseValue = classes [ chosenClass ]._baseAttackSpeed;
        health.BaseValue = classes [ chosenClass ]._health;
        mana.BaseValue = classes [ chosenClass ]._mana;
        stamina.BaseValue = classes [ chosenClass ]._stamina;
        armor.BaseValue = classes [ chosenClass ]._armor;
        maxHealth.BaseValue = classes [ chosenClass ]._maxHealth;
        maxMana.BaseValue = classes [ chosenClass ]._maxMana;
        healthRegeneration.BaseValue = classes[chosenClass]._healthRegeneration;
        healthOnKill.BaseValue = classes[chosenClass]._healthOnKill;

        cooldownReduction.BaseValue = classes[chosenClass]._cooldownReduction;
        criticalHitChance.BaseValue = classes [ chosenClass ]._criticalHitChance;
        criticalHitDamage.BaseValue = classes [ chosenClass ]._criticalHitDamage;
        manaCostReduction.BaseValue = classes[chosenClass]._manaCostReduction;
        meleeDamageReduction.BaseValue = classes[chosenClass]._meleeDamageReduction;
        rangedDamageReduction.BaseValue = classes[chosenClass]._rangedDamageReduction;


        fireResistance.BaseValue = classes [ chosenClass ]._fireResistance;
        coldResistance.BaseValue = classes [ chosenClass ]._coldResistance;
        poisonResistance.BaseValue = classes [ chosenClass ]._poisonResistance;
        lightningResistance.BaseValue = classes [ chosenClass ]._lightningResistance;
        physicalResistance.BaseValue = classes [ chosenClass ]._physicalResistance;
        allResistance.BaseValue = classes[chosenClass]._allResistance;

        goldFind.BaseValue = classes[chosenClass]._goldFind;
        magicFind.BaseValue = classes[chosenClass]._magicFind;
        pickupRadius.BaseValue = classes[chosenClass]._pickupRadius;

        strength.BaseValue = classes [ chosenClass ]._strength;
        dexterity.BaseValue = classes [ chosenClass ]._dexterity;
        endurance.BaseValue = classes [ chosenClass ]._endurance;
        energy.BaseValue = classes [ chosenClass ]._energy;


    }

    public void BuildSmallBoxStatsText ( )
    {
       
        BigStatsBox.instance.BuildText ( );
        

    }
    public void AddToList ( ) // tarkista että sama järjestys joka on enumissa stat
    {
        listCharacterStats.Add ( moveSpeed ); // 0
        listCharacterStats.Add ( jumpForce ); // 1
        listCharacterStats.Add ( extraJumpForce ); // 2
        listCharacterStats.Add ( baseDamage ); // 3
        listCharacterStats.Add ( baseAttackSpeed ); // 4
        listCharacterStats.Add ( health ); //5
        listCharacterStats.Add ( mana ); // 6
        listCharacterStats.Add ( stamina ); // 7
        listCharacterStats.Add ( armor );// 8
        listCharacterStats.Add ( fireResistance );// 9
        listCharacterStats.Add ( coldResistance );//10
        listCharacterStats.Add ( poisonResistance );// 11
        listCharacterStats.Add ( lightningResistance );// 12
        listCharacterStats.Add ( strength );// 13
        listCharacterStats.Add ( dexterity );// 14
        listCharacterStats.Add ( endurance );//15
        listCharacterStats.Add ( energy );// 16
        listCharacterStats.Add ( physicalResistance );//17
        listCharacterStats.Add ( maxHealth ); //18
        listCharacterStats.Add ( maxMana ); //19
        listCharacterStats.Add ( criticalHitChance ); //20
        listCharacterStats.Add ( criticalHitDamage ); //21
        listCharacterStats.Add (cooldownReduction);//22
        listCharacterStats.Add(allResistance); //23
        listCharacterStats.Add(goldFind); //24
        listCharacterStats.Add(magicFind); //25
        listCharacterStats.Add(healthRegeneration);//26
        listCharacterStats.Add(manaCostReduction);//27
        listCharacterStats.Add(meleeDamageReduction);//28
        listCharacterStats.Add(rangedDamageReduction);//29
        listCharacterStats.Add(healthOnKill);//30
        listCharacterStats.Add(pickupRadius);//31




    }

}
[SerializeField]
public enum Stat // tarkista että on samassa järjesyksessä mikä addToList
{
    MoveSpeed, JumpForce, ExtraJumpForce, BaseDamage,
    BaseAttackSpeed, Health, Mana, Stamina, Armor,
    fireResistance, coldResistance, poisonResistance,
    lightningResistance, Strength, Dexterity,
    Endurance, Energy, physicalResistance, maxHealth, maxMana,
    criticalHitChance, criticalHitDamage,cooldownReduction,allResistance,
    goldFind,magicFind,healthRegeneration,manaCostReduction,meleeDamageReduction,
    rangedDamageReduction,healthOnKill,pickupRadius,
    
}

