using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats; //Character stat assetin käyttäminen

/// <summary>
/// Alustaa pelaajan valitseman classin
/// </summary>
public class PlayerClass : MonoBehaviour
{

    [Header ("This List Includes all possible classes")]
    public List<PlayerStatsObject> classes = new List<PlayerStatsObject>();

    public string myName = "jokuUkko";
    public string className = "perusJamppa";

    #region MovementStats
    public CharacterStat moveSpeed;
    public CharacterStat jumpForce;
    #endregion

    #region CombatStats
    public CharacterStat baseDamage;
    public CharacterStat baseAttackSpeed;
    public CharacterStat health;
    public CharacterStat mana;
    public CharacterStat stamina;
    public CharacterStat armor;
    #endregion

    #region BaseStats
    public CharacterStat strength;
    public CharacterStat dexterity;
    public CharacterStat endurance;
    public CharacterStat energy;
    public CharacterStat statPoint;
    public CharacterStat skillPoint;
    public CharacterStat playerLevel;
    #endregion

    public int chosenClass = 0; //tää muuttuu sitten ku class selection ui on tehty
    public static bool isInitStats = false; //jos uusiHahmo asetetaan trueksi hahmon tehdessä ja tästä eteenpäin pitäisi olla false...

    //alustetaan vielä tässä vaiheessa ennen character selection UI:ta trueksi is initStats
    private void Awake ( )
    {
        isInitStats = true;
        if ( isInitStats )
        {
            isInitStats = false;
            InitializeHeroClass ( );
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

        moveSpeed.BaseValue = classes [ chosenClass ]._moveSpeed;
        jumpForce.BaseValue = classes [ chosenClass ]._jumpForce;
        baseDamage.BaseValue = classes [ chosenClass ]._baseDamage;
        baseAttackSpeed.BaseValue = classes [ chosenClass ]._baseAttackSpeed;
        health.BaseValue = classes [ chosenClass ]._health;
        mana.BaseValue = classes [ chosenClass ]._mana;
        stamina.BaseValue = classes [ chosenClass ]._stamina;
        armor.BaseValue = classes [ chosenClass ]._armor;

        strength.BaseValue = classes [ chosenClass ]._strength;
        dexterity.BaseValue = classes [ chosenClass ]._dexterity;
        endurance.BaseValue = classes [ chosenClass ]._endurance;
        energy.BaseValue = classes [ chosenClass ]._energy;
    }
}
