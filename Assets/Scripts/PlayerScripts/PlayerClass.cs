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
    [Header ("This List Includes all possible classes")]
    public List<PlayerStatsObject> classes = new List<PlayerStatsObject>();
  
    public List<CharacterStat> listCharacterStats = new List<CharacterStat>();
    public string myName = "Jomppe";
    public string className = "Nephalem";
    public ClassType classType;
    #region MovementStats
    public CharacterStat moveSpeed;
    public CharacterStat jumpForce;
    public CharacterStat extraJumpForce;
    #endregion

    #region CombatStats
    public CharacterStat baseDamage;
    public CharacterStat baseAttackSpeed;
    public CharacterStat maxHealth;
    public CharacterStat health;
    public CharacterStat maxMana;
    public CharacterStat mana;
    public CharacterStat stamina;
    public CharacterStat armor;

    public CharacterStat criticalHitChance;
    public CharacterStat criticalHitDamage;
    #region elemental resistances
    public CharacterStat fireResistance;
    public CharacterStat coldResistance;
    public CharacterStat poisonResistance;
    public CharacterStat lightningResistance;
    public CharacterStat physicalResistance;
    #endregion
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
    
    #region displayStats
    public TMP_Text leftSmallStatsText;
    public TMP_Text rightSmallStatsText;
    #endregion

    #region bools
    public bool fullHealth = false; // if health is full
    public bool fullMana= false; // if mana is full
    #endregion
    public int chosenClass = 0; //tää muuttuu sitten ku class selection ui on tehty
    public static bool isInitStats = false; //jos uusiHahmo asetetaan trueksi hahmon tehdessä ja tästä eteenpäin pitäisi olla false...

    private void Awake ( )
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        AddToList();
        isInitStats = true; //alustetaan vielä tässä vaiheessa ennen character selection UI:ta trueksi is initStats
        if ( isInitStats )
        {
            isInitStats = false;
            InitializeHeroClass ( );
        }
       
    }
    private void Start()
    {
        checkForChanges();
    }
    public void checkForChanges()
    {
        foreach (CharacterStat cStat in listCharacterStats)
        {
            float randomFloat = cStat.Value;
        }
        BuildSmallBoxStatsText();
    }
    private void Update()
    {

        checkHealth();
    }
    public void checkHealth() // tarkistetaan että health ei ole suurempi kuin maxHealth;
    {
        if (health.Value >= maxHealth.Value)
        {

            health.AddModifier(new StatModifier(-(health.Value - maxHealth.Value), StatModType.Flat));
            checkForChanges();
            fullHealth = true;

        }
        else
        {
            fullHealth = false;
        }
        if(mana.Value >= maxMana.Value)
        {
            mana.AddModifier(new StatModifier(-(mana.Value - maxMana.Value), StatModType.Flat));
            checkForChanges();
            fullMana = true;
        }
        else
        {
            fullHealth = false;
        }
    }

    /// <summary>
    /// Resetoi statit takas valitun classin mukaisiksi
    /// </summary>
    public void ResetStats()
    {
       
        InitializeHeroClass ( );
        
    }

    /// <summary>
    /// Asettaa valitun classin indexin muuttujaan chosenClass
    /// </summary>
    /// <param name="chosenClass"></param>
    public void ChosenClass( int chosenClass)
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
        baseDamage.BaseValue = classes [ chosenClass ]._baseDamage;
        baseAttackSpeed.BaseValue = classes [ chosenClass ]._baseAttackSpeed;
        health.BaseValue = classes [ chosenClass ]._health;
        mana.BaseValue = classes [ chosenClass ]._mana;
        stamina.BaseValue = classes [ chosenClass ]._stamina;
        armor.BaseValue = classes [ chosenClass ]._armor;
        maxHealth.BaseValue = classes[chosenClass]._maxHealth;
        maxMana.BaseValue = classes[chosenClass]._maxMana;

        criticalHitChance.BaseValue = classes[chosenClass]._criticalHitChance;
        criticalHitDamage.BaseValue = classes[chosenClass]._criticalHitDamage;


        fireResistance.BaseValue = classes[chosenClass]._fireResistance;
        coldResistance.BaseValue = classes[chosenClass]._coldResistance;
        poisonResistance.BaseValue = classes[chosenClass]._poisonResistance;
        lightningResistance.BaseValue = classes[chosenClass]._lightningResistance;
        physicalResistance.BaseValue = classes[chosenClass]._physicalResistance;

        strength.BaseValue = classes [ chosenClass ]._strength;
        dexterity.BaseValue = classes [ chosenClass ]._dexterity;
        endurance.BaseValue = classes [ chosenClass ]._endurance;
        energy.BaseValue = classes [ chosenClass ]._energy;

        
    }

    public void BuildSmallBoxStatsText()
    {
        leftSmallStatsText.text = "Strength: " + strength.Value.ToString() + "\nDexterity: " + dexterity.Value.ToString() 
            +"\nEndurance: "+endurance.Value.ToString() +"\nEnergy: "+ energy.Value.ToString();
        rightSmallStatsText.text = "Armor: " + armor.Value.ToString() + "\nHealth: " +maxHealth.Value.ToString()+ "\nMana " + maxMana.Value.ToString()
            + "\nStamina: " + stamina.Value.ToString();
        BigStatsBox.instance.BuildText();
        
    }
    public void AddToList() // tarkista että sama järjestys joka on enumissa stat
    {
        listCharacterStats.Add(moveSpeed); // 0
        listCharacterStats.Add(jumpForce); // 1
        listCharacterStats.Add(extraJumpForce); // 2
        listCharacterStats.Add(baseDamage); // 3
        listCharacterStats.Add(baseAttackSpeed); // 4
        listCharacterStats.Add(health); //5
        listCharacterStats.Add(mana); // 6
        listCharacterStats.Add(stamina); // 7
        listCharacterStats.Add(armor);// 8
        listCharacterStats.Add(fireResistance);// 9
        listCharacterStats.Add(coldResistance);//10
        listCharacterStats.Add(poisonResistance);// 11
        listCharacterStats.Add(lightningResistance);// 12
        listCharacterStats.Add(strength);// 13
        listCharacterStats.Add(dexterity);// 14
        listCharacterStats.Add(endurance);//15
        listCharacterStats.Add(energy);// 16
        listCharacterStats.Add(physicalResistance);//17
        listCharacterStats.Add(maxHealth); //18
        listCharacterStats.Add(maxMana); //19
        listCharacterStats.Add(criticalHitChance); //20
        listCharacterStats.Add(criticalHitDamage); //21


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
    criticalHitChance,criticalHitDamage,


}
