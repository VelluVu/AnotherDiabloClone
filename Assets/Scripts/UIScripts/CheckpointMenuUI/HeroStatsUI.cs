using Kryz.CharacterStats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroStatsUI : MonoBehaviour
{

    public Button [ ] buttons;
    public TextMeshProUGUI [ ] tmps;
    public GameObject allStatsbox;

    Dictionary<string, float> newBaseStats = new Dictionary<string, float> ( );
    List<string> keys;

    private void Awake ( )
    {

        buttons [ 0 ].onClick.AddListener ( ( ) => RemoveStat ( PlayerClass.instance.strength ) );
        buttons [ 1 ].onClick.AddListener ( ( ) => AddStat ( PlayerClass.instance.strength ) );
        buttons [ 2 ].onClick.AddListener ( ( ) => RemoveStat ( PlayerClass.instance.dexterity ) );
        buttons [ 3 ].onClick.AddListener ( ( ) => AddStat ( PlayerClass.instance.dexterity ) );
        buttons [ 4 ].onClick.AddListener ( ( ) => RemoveStat ( PlayerClass.instance.endurance ) );
        buttons [ 5 ].onClick.AddListener ( ( ) => AddStat ( PlayerClass.instance.endurance ) );
        buttons [ 6 ].onClick.AddListener ( ( ) => RemoveStat ( PlayerClass.instance.energy ) );
        buttons [ 7 ].onClick.AddListener ( ( ) => AddStat ( PlayerClass.instance.energy ) );
        buttons [ 8 ].onClick.AddListener ( ( ) => ExitHeroStatsUI ( ) );
        //buttons [ 9 ].onClick.AddListener ( ( ) => ShowAllStats ( ) );
        buttons [ 10 ].onClick.AddListener ( ( ) => SaveAddedPoints ( ) );

        newBaseStats.Add ( "Strength", PlayerClass.instance.strength.BaseValue );
        newBaseStats.Add ( "Dexterity", PlayerClass.instance.dexterity.BaseValue );
        newBaseStats.Add ( "Endurance", PlayerClass.instance.endurance.BaseValue );
        newBaseStats.Add ( "Energy", PlayerClass.instance.energy.BaseValue );
        keys = new List<string> ( newBaseStats.Keys );
    }

    private void OnEnable ( )
    {
     
        tmps [ 0 ].text = newBaseStats [ "Strength" ].ToString ( );
        tmps [ 1 ].text = "Strength";
        tmps [ 2 ].text = newBaseStats [ "Dexterity" ].ToString ( );
        tmps [ 3 ].text = "Dexterity";
        tmps [ 4 ].text = newBaseStats [ "Endurance" ].ToString ( );
        tmps [ 5 ].text = "Endurance";
        tmps [ 6 ].text = newBaseStats [ "Energy" ].ToString ( );
        tmps [ 7 ].text = "Energy";

        tmps [ 8 ].text = "Points : " + PlayerClass.instance.statPoint.Value;
  
    }

    void RemoveStat ( CharacterStat stat )
    {
       
        foreach ( var key in keys )
        {
            if ( stat.statName == key )
            {
                if ( newBaseStats [ key ] > stat.BaseValue )
                {
                    newBaseStats [ key ]--;
                    PlayerClass.instance.statPoint.BaseValue++;
                    UpdateUI ( );
                }
            }
        }


    }

    void AddStat ( CharacterStat stat )
    {
      
        foreach ( var key in keys )
        {
            if ( stat.statName == key )
            {
                if ( PlayerClass.instance.statPoint.Value > 0 )
                {
                    newBaseStats [ key ]++;
                    PlayerClass.instance.statPoint.BaseValue--;
                    UpdateUI ( );
                }
            }
        }
    }

    void UpdateUI ( )
    {
        tmps [ 0 ].text = newBaseStats [ "Strength" ].ToString ( );
        tmps [ 1 ].text = "Strength";
        tmps [ 2 ].text = newBaseStats [ "Dexterity" ].ToString ( );
        tmps [ 3 ].text = "Dexterity";
        tmps [ 4 ].text = newBaseStats [ "Endurance" ].ToString ( );
        tmps [ 5 ].text = "Endurance";
        tmps [ 6 ].text = newBaseStats [ "Energy" ].ToString ( );
        tmps [ 7 ].text = "Energy";

        tmps [ 8 ].text = "Points : " + PlayerClass.instance.statPoint.Value;
    }

    void ExitHeroStatsUI ( )
    {
        gameObject.SetActive ( false );
    }

    void ShowAllStats ( )
    {
        if ( allStatsbox.activeSelf )
        {
            allStatsbox.SetActive ( false );
        }
        else
        {
            allStatsbox.SetActive ( true );
        }
    }

    void SaveAddedPoints ( )
    {
   
        PlayerClass.instance.strength.BaseValue = newBaseStats [ "Strength" ];
        PlayerClass.instance.dexterity.BaseValue = newBaseStats [ "Dexterity" ];
        PlayerClass.instance.endurance.BaseValue = newBaseStats [ "Endurance" ];
        PlayerClass.instance.energy.BaseValue = newBaseStats [ "Energy" ];

        PlayerClass.instance.UpdateStatEffect ( );

        gameObject.SetActive ( false );
    }




}
