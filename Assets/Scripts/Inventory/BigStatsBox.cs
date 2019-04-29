using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BigStatsBox : MonoBehaviour
{

    public GameObject background;
    public static BigStatsBox instance;
    public Button detailsButton;
    public TMP_Text detailsText;

    public List<TMP_Text> textList = new List<TMP_Text>();
    public List<TMP_Text> smallStatsTextList = new List<TMP_Text>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        Open(false);
        detailsButton.onClick.AddListener(() => Open(true));
    }
    public void BuildText()
    {
        //Primary Stats
        textList[0].text = "Strength: "+ "<color=green>" + PlayerClass.instance.strength.Value.ToString("F0") + "</color>";
        textList[1].text = "Dexterity: " + "<color=green>" + PlayerClass.instance.dexterity.Value.ToString("F0") + "</color>";
        textList[2].text = "Endurance: " + "<color=green>" + PlayerClass.instance.endurance.Value.ToString("F0") + "</color>";
        textList[3].text = "Energy: " + "<color=green>" + PlayerClass.instance.energy.Value.ToString("F0") + "</color>";
        // Offensive Stats
        textList[4].text = PlayerClass.instance.criticalHitChance.Value.ToString("F0") + "%"+" Critical Hit Chance";
        textList[5].text = PlayerClass.instance.criticalHitDamage.Value.ToString("F0") + "%"+" Critical Hit Damage";
        textList[6].text = "Attacks per second: "+ (1/ PlayerClass.instance.baseAttackSpeed.Value).ToString("F2");
        textList[7].text = "+"+Mathf.Round(PlayerClass.instance.cooldownReduction.Value).ToString("F0")+ " % Cooldown Reduction";
        
        // Defensive Stats
        //resistance *5
        // Armor,
        // block
        // melee+range damage reduction.
        //
        // Recovery Stats
        // Utility Stats
       

        //textList[7].text = "Energy: " + PlayerClass.instance.energy.Value;




        smallStatsTextList[0].text = "Strength: " + PlayerClass.instance.strength.Value.ToString("F0");
        smallStatsTextList[1].text = "Dexterity:" + PlayerClass.instance.dexterity.Value.ToString("F0");
        smallStatsTextList[2].text = "Endurance " + PlayerClass.instance.endurance.Value.ToString("F0");
        smallStatsTextList[3].text = "Energy: " + PlayerClass.instance.energy.Value.ToString("F0");
        smallStatsTextList[4].text = "Armor: " + PlayerClass.instance.armor.Value.ToString("F0");
        smallStatsTextList[5].text = "Health: " + PlayerClass.instance.maxHealth.Value.ToString("F0");
        smallStatsTextList[6].text = "Mana: " + PlayerClass.instance.maxMana.Value.ToString("F0");
        smallStatsTextList[7].text = "Stamina: " + PlayerClass.instance.stamina.Value.ToString("F0");

       
    }
    public void Open(bool open)
    {
        detailsButton.onClick.RemoveAllListeners();
        background.SetActive(open);
        if (open)
        {
            detailsText.text = "Hide Details";
            detailsButton.onClick.AddListener(() => Open(false));
        }
        else
        {
            detailsText.text = "Details";
            detailsButton.onClick.AddListener(() => Open(true));
        }
    }

    
}
