using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{
    public List<TMP_Text> texts = new List<TMP_Text>();
    public GameObject statBackground;
    public bool statsOpen;
    public Button backButton;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(() => DoOnClick("Back"));
    }
    void DoOnClick(string action)
    {
        statsOpen = false;
        statBackground.SetActive(false);
    }
    public void OpenMenu(bool open)
    {
        statBackground.SetActive(open);
        if (open)
        {
            statsOpen = true;
            Stats.instance.Load();
        }
        else
        {
            statsOpen = false;
        }
    }
    public void UpdateStats()
    {
        texts[0].text = "Time Played: " + Stats.instance.playTime;
        texts[1].text = "Enemies Killed: " + Stats.instance.enemiesKilled;
        texts[2].text = "Items looted: " + Stats.instance.itemsLooted;
        texts[3].text = "Total Damage Taken: " + Stats.instance.damageTaken;
        texts[4].text = "Total Damage Dealt: " + Stats.instance.damageDealt;
        texts[5].text = "Fire Damage Dealt: " + Stats.instance.fireDamageDealt;
        texts[6].text = "Cold Damage Dealt: " + Stats.instance.coldDamageDealt;
        texts[7].text = "Poison Damage Dealt: " + Stats.instance.poisonDamageDealt;
        texts[8].text = "Lighting Damage Dealt: " + Stats.instance.lightningDamageDealt;
        texts[9].text = "Physical Damage Dealt: " + Stats.instance.physicalDamageDealt;
        texts[10].text = "Deaths: " + Stats.instance.deaths;
       
    }
    
}
