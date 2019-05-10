using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats; //Character stat assetin käyttäminen
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

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
    string saveName = "/player.dat";

    #region MovementStats
    public CharacterStat moveSpeed; // old move speed new variables in player 
    public CharacterStat jumpForce; // old jumpforce
    public CharacterStat extraJumpForce;
    public CharacterStat extraWallJumpForce;
    #endregion

    #region CombatStats
    [Header ( "All Player Stats" )]
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

    public CharacterStat block;
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
    public CharacterStat fallDamageReduction;
    public Dictionary<DamageType, CharacterStat> damageTypes = new Dictionary<DamageType, CharacterStat> ( );

    #endregion
    #endregion

    float oldStr;
    float olddex;
    float oldEnd;
    float oldEnergy;
    public float healthTickRate;
    public float timeToGen;

    #region Utility Stats
    public CharacterStat experienceOnKill;
    public CharacterStat goldFind;
    public CharacterStat magicFind;
    public CharacterStat pickupRadius;
    public CharacterStat experienceBonus;
    #endregion



    //Tässä vois olla vaikka boostit jne..
    #region StatModifiers
    public StatModifier healthLoss;
    public StatModifier healthFill;
    public StatModifier manaLoss;
    public StatModifier manaFill;
    public StatModifier healthBoost;
    public StatModifier strengthBoost;
    public StatModifier armorBoost;
    public StatModifier attackSpeedBoost;


    public StatModifier strengthEffect;
    public StatModifier dexterityEffect;
    public StatModifier energyEffect;
    public StatModifier enduranceEffect;
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
        SetStatNames ( );
        AddToList ( );
        isInitStats = true; //alustetaan vielä tässä vaiheessa ennen character selection UI:ta trueksi is initStats
        if ( isInitStats )
        {
            isInitStats = false;
            InitializeHeroClass ( );
        }
        CreateResistanceDictionary ( );

    }
    private void CreateResistanceDictionary ( )
    {
        damageTypes.Add ( DamageType.Fire, fireResistance );
        damageTypes.Add ( DamageType.Cold, coldResistance );
        damageTypes.Add ( DamageType.Lightning, lightningResistance );
        damageTypes.Add ( DamageType.Physical, physicalResistance );
        damageTypes.Add ( DamageType.Poison, poisonResistance );
        damageTypes.Add ( DamageType.Raw, fallDamageReduction );
    }
    private void Start ( )
    {
        checkForChanges ( );
        
    }

    private void OnEnable ( )
    {
        CheckPoint.checkPointEvent += PlayerEnterCheckPoint;
        StateController.enemyDeathEvent += HealOnKill;
    }

    private void OnDisable ( )
    {
        CheckPoint.checkPointEvent -= PlayerEnterCheckPoint;
        StateController.enemyDeathEvent -= HealOnKill;
    }

    public void checkForChanges ( )
    {
        foreach ( CharacterStat cStat in listCharacterStats )
        {
            cStat.isDirty = true;
        }
        BuildSmallBoxStatsText ( );
        UpdateStatEffect ( );
    }

    private void Update ( )
    {
        checkHealth ( );
        PassiveManaRegen ( );

        if ( Time.time > timeToGen )
        {
            timeToGen = Time.time + healthTickRate;
            health.BaseValue += healthRegeneration.Value;
            Debug.Log ( healthRegeneration.Value );
        }
        

        //Debug.Log ( strength.Value );
        //Debug.Log ( dexterity.Value );
        //Debug.Log ( endurance.Value );
        //Debug.Log ( energy.Value );

        //Debug.Log ( baseDamage.Value + " - " + baseDamageMax.Value );
        //Debug.Log ( criticalHitChance.Value + " - " + criticalHitDamage.Value );
        //Debug.Log ( maxHealth.Value );
        //Debug.Log ( maxMana.Value );
    }

    public void UpdateStatEffect()
    {
        baseDamage.RemoveModifier ( strengthEffect );
        baseDamageMax.RemoveModifier ( strengthEffect );
        criticalHitChance.RemoveModifier ( dexterityEffect );
        criticalHitDamage.RemoveModifier ( dexterityEffect );
        maxHealth.RemoveModifier ( enduranceEffect );
        maxMana.RemoveModifier ( energyEffect );

        strengthEffect = new StatModifier ( strength.Value * 0.5f ,StatModType.Flat );
        dexterityEffect = new StatModifier ( dexterity.Value * 0.1f ,StatModType.Flat );
        enduranceEffect = new StatModifier ( endurance.Value * 2.0f,StatModType.Flat );
        energyEffect = new StatModifier ( energy.Value * 2.0f ,StatModType.Flat );
     
        baseDamage.AddModifier ( strengthEffect );
        baseDamageMax.AddModifier ( strengthEffect );
        criticalHitChance.AddModifier (dexterityEffect );
        criticalHitDamage.AddModifier ( dexterityEffect );
        maxHealth.AddModifier ( enduranceEffect );
        maxMana.AddModifier ( energyEffect );
     
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

    public void PassiveManaRegen ( )
    {
        mana.BaseValue += maxMana.Value * 0.0001f * Time.deltaTime;   
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

    public void HealOnKill( Transform transform, int xp, StateController origin )
    {
        ReferenceHolder.instance.player.OnRestoreHealth ( Mathf.Round(healthOnKill.Value), false );
        ReferenceHolder.instance.player.levelUp.GainXP((int)experienceOnKill.Value, this);
        //Debug.Log ( healthOnKill.Value );
        //Debug.Log ( experienceOnKill.Value );
    }

    public void BoostHealth(float percent, float time)
    {
        healthBoost = new StatModifier ( percent * 0.01f, StatModType.PercentAdd );
        maxHealth.AddModifier ( healthBoost );
        ReferenceHolder.instance.player.OnRestoreHealth ( health.Value / maxHealth.Value * maxHealth.Value, false );
        StartCoroutine ( BuffTime (maxHealth, healthBoost, time ) );
    }

    public void BoostStrength(float percent, float time)
    {
        strengthBoost = new StatModifier(percent * 0.01f, StatModType.PercentAdd);
        strength.AddModifier(strengthBoost);
        StartCoroutine(BuffTime(strength, strengthBoost, time));
        checkForChanges();
        Invoke("checkForChanges", time+0.1f);
    }

    public void BoostArmor(float percent, float time)
    {
        armorBoost = new StatModifier(percent * 0.01f, StatModType.PercentAdd);
        armor.AddModifier(armorBoost);
        StartCoroutine(BuffTime(armor, armorBoost, time));
        checkForChanges();
        Invoke("checkForChanges", time + 0.1f);
    }

    public void BoostAttackSpeed(float percent, float time)
    {
        attackSpeedBoost = new StatModifier(percent * 0.01f, StatModType.PercentAdd);
        baseAttackSpeed.AddModifier(attackSpeedBoost);
        StartCoroutine(BuffTime(baseAttackSpeed, attackSpeedBoost, time));
        checkForChanges();
        Invoke("checkForChanges", time + 0.1f);
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
        mana.BaseValue = maxMana.Value;
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

        moveSpeed.BaseValue = classes [ chosenClass ]._moveSpeed;
        extraJumpForce.BaseValue = classes [ chosenClass ]._extraJumpForce;
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
        healthRegeneration.BaseValue = classes [ chosenClass ]._healthRegeneration;
        healthOnKill.BaseValue = classes [ chosenClass ]._healthOnKill;
        block.BaseValue = classes [ chosenClass ]._block;

        cooldownReduction.BaseValue = classes [ chosenClass ]._cooldownReduction;
        criticalHitChance.BaseValue = classes [ chosenClass ]._criticalHitChance;
        criticalHitDamage.BaseValue = classes [ chosenClass ]._criticalHitDamage;
        manaCostReduction.BaseValue = classes [ chosenClass ]._manaCostReduction;
        meleeDamageReduction.BaseValue = classes [ chosenClass ]._meleeDamageReduction;
        rangedDamageReduction.BaseValue = classes [ chosenClass ]._rangedDamageReduction;


        fireResistance.BaseValue = classes [ chosenClass ]._fireResistance;
        coldResistance.BaseValue = classes [ chosenClass ]._coldResistance;
        poisonResistance.BaseValue = classes [ chosenClass ]._poisonResistance;
        lightningResistance.BaseValue = classes [ chosenClass ]._lightningResistance;
        physicalResistance.BaseValue = classes [ chosenClass ]._physicalResistance;
        allResistance.BaseValue = classes [ chosenClass ]._allResistance;
        fallDamageReduction.BaseValue = classes [ chosenClass ]._fallDamageReduction;

        goldFind.BaseValue = classes [ chosenClass ]._goldFind;
        magicFind.BaseValue = classes [ chosenClass ]._magicFind;
        pickupRadius.BaseValue = classes [ chosenClass ]._pickupRadius;
        experienceOnKill.BaseValue = classes [ chosenClass ]._experienceOnKill;
        experienceBonus.BaseValue = classes [ chosenClass ]._experienceBonus;

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
        listCharacterStats.Add ( cooldownReduction );//22
        listCharacterStats.Add ( allResistance ); //23
        listCharacterStats.Add ( goldFind ); //24
        listCharacterStats.Add ( magicFind ); //25
        listCharacterStats.Add ( healthRegeneration );//26
        listCharacterStats.Add ( manaCostReduction );//27
        listCharacterStats.Add ( meleeDamageReduction );//28
        listCharacterStats.Add ( rangedDamageReduction );//29
        listCharacterStats.Add ( healthOnKill );//30
        listCharacterStats.Add ( pickupRadius );//31
        listCharacterStats.Add ( block );//32
        listCharacterStats.Add ( fallDamageReduction ); //33
        listCharacterStats.Add ( experienceOnKill );//34
        listCharacterStats.Add ( experienceBonus );//35

    }

    void SetStatNames ( )
    {
        moveSpeed.statName = "Move Speed";
        jumpForce.statName = "Jump Force";
        extraJumpForce.statName = "Extra Jump Force";
        baseDamage.statName = "Base Damage";
        baseAttackSpeed.statName = "Base Attack Speed";
        health.statName = "Health";
        mana.statName = "Mana";
        stamina.statName = "Stamina";
        armor.statName = "Armor";
        fireResistance.statName = "Fire Resistance";
        coldResistance.statName = "Cold Resistance";
        poisonResistance.statName = "Poison Resistance";
        lightningResistance.statName = "Lightning Resistance";
        strength.statName = "Strength";
        dexterity.statName = "Dexterity";
        endurance.statName = "Endurance";
        energy.statName = "Energy";
        physicalResistance.statName = "Physical Resistance";
        maxHealth.statName = "Max Health";
        maxMana.statName = "Max Mana";
        criticalHitChance.statName = "Critical Hit Chance";
        criticalHitDamage.statName = "Critical Hit Damage";
        cooldownReduction.statName = "Cooldown Reduction";
        allResistance.statName = "All Resistances";
        goldFind.statName = "Gold Find";
        magicFind.statName = "Magic Find";
        healthRegeneration.statName = "Health Regeneration";
        manaCostReduction.statName = "Mana Cost Reduction";
        meleeDamageReduction.statName = "Melee Damage Reduction";
        rangedDamageReduction.statName = "Ranged Damage Reduction";
        healthOnKill.statName = "Health On Kill";
        pickupRadius.statName = "Pick Up Radius";
        block.statName = "Block";
        fallDamageReduction.statName = "Fall Damage Reduction";
        experienceOnKill.statName = "Experience On Kill";
        experienceBonus.statName = "Experience Bonus";
    }

    IEnumerator BuffTime ( CharacterStat stat, StatModifier mod, float time)
    {
        yield return new WaitForSeconds ( time );
        stat.RemoveModifier ( mod );
    }

   

    public void Save(int checkpointId)
    {


        saveName = "/player.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveName);
        PlayerSaveData data = new PlayerSaveData();

        data.level = (int)playerLevel.Value;
        data.skillPoints = (int)skillPoint.Value;
        data.statPoints = (int)statPoint.Value;
        data.experience = (int)ReferenceHolder.instance.player.levelUp.currentXP.Value;
        data.checkPoint = checkpointId;


        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + saveName))
        {
            Debug.Log("<color=red>" +"Player Loaded"+ "</color>");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveName, FileMode.Open);
            PlayerSaveData data = (PlayerSaveData)bf.Deserialize(file);
            file.Close();

            playerLevel.BaseValue = data.level;
            statPoint.BaseValue = data.statPoints;
            skillPoint.BaseValue = data.skillPoints;
            ReferenceHolder.instance.player.levelUp.currentXP.BaseValue = data.experience;
            foreach(CheckPoint check in FindObjectsOfType<CheckPoint>())
            {
                Debug.Log("<color=red>"+check.areaName+"</color>");
                if((int)check.areaName == data.checkPoint)
                {
                    ReferenceHolder.instance.player.transform.position = check.transform.position;
                    break;
                }
            }
        }
    }


}
[Serializable]
class PlayerSaveData
{
    public int level;
    public int statPoints;
    public int skillPoints;
    public int experience;
    public int checkPoint;
}

[SerializeField]
public enum Stat // tarkista että on samassa järjesyksessä mikä addToList
{
    MoveSpeed, JumpForce, ExtraJumpForce, BaseDamage,
    BaseAttackSpeed, Health, Mana, Stamina, Armor,
    fireResistance, coldResistance, poisonResistance,
    lightningResistance, Strength, Dexterity,
    Endurance, Energy, physicalResistance, maxHealth, maxMana,
    criticalHitChance, criticalHitDamage, cooldownReduction, allResistance,
    goldFind, magicFind, healthRegeneration, manaCostReduction, meleeDamageReduction,
    rangedDamageReduction, healthOnKill, pickupRadius, block, fallDamageReduction, experienceOnKill,
    experienceBonus,


}


