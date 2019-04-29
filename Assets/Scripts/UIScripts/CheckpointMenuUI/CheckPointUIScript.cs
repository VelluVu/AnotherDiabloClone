using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menu : 
/// 1. Pelaaja voi portata erialueelle -> avaa kartan / valikon missä lista paikoista
/// 2. Pelaaja voi asettaa levelpojoja / skillpojoja -> avaa toisen valikon
/// 3. Pelaaja voi vaihtaa skillejä, mitä osaa -> avaa toisen valikon
/// 4. Pelaaja voi tallentaa/load
/// </summary>
public class CheckPointUIScript : MonoBehaviour
{

    public List<Button> buttons = new List<Button> ( );
    [Space]
    public GameObject heroStatsUI;
    public GameObject travelUI;

    public delegate void OnExitCheckPointUIDelegate ( );
    public static event OnExitCheckPointUIDelegate onExitCheckPointUIExitEvent;

    private void Awake ( )
    {
        buttons [ 0 ].onClick.AddListener ( ( ) => TravelSelected ( ) );
        buttons [ 1 ].onClick.AddListener ( ( ) => HeroStatsSelected ( ) );
        buttons [ 2 ].onClick.AddListener ( ( ) => AbilitySelected ( ) );
        buttons [ 3 ].onClick.AddListener ( ( ) => ExitCheckPoint ( ) );
    }

    private void OnEnable ( )
    {        
        CheckPoint.checkPointEvent += OnEnterCheckPointEvent;
    }

    private void OnDisable ( )
    {
        CheckPoint.checkPointEvent -= OnEnterCheckPointEvent;
    }

    public void OnEnterCheckPointEvent ( )
    {

        transform.GetChild ( 0 ).gameObject.SetActive ( true );

    }

    /// <summary>
    /// Avaa Ability Menun
    /// </summary>
    private void AbilitySelected ( )
    {

    }

    /// <summary>
    /// Avaa hero stats / skill tree menun
    /// </summary>
    private void HeroStatsSelected ( )
    {
        heroStatsUI.SetActive ( true );
    }

    /// <summary>
    /// Avaa checkpoint menun missä lista unlockatuista alueista
    /// </summary>
    private void TravelSelected ( )
    {
        travelUI.SetActive ( true );
    }

    private void ExitCheckPoint ( )
    {

        transform.GetChild ( 0 ).gameObject.SetActive ( false );
        heroStatsUI.SetActive ( false );

        if ( onExitCheckPointUIExitEvent != null )
        {
            onExitCheckPointUIExitEvent ( );
        }
    }
}
