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
        textList[0].text = "Strength: " + PlayerClass.instance.strength.Value;
        textList[1].text = "Dexterity: " + PlayerClass.instance.dexterity.Value;
        textList[2].text = "Endurance " + PlayerClass.instance.endurance.Value;
        textList[3].text = "Energy: " + PlayerClass.instance.energy.Value;
        // Offensive Stats
        textList[4].text = PlayerClass.instance.criticalHitChance.Value +"%"+" Critical Hit Chance";
        textList[5].text = PlayerClass.instance.criticalHitDamage.Value + "%"+" Critical Hit Damage";
        //textList[6].text = ": " + PlayerClass.instance.energy.Value;
        //textList[7].text = "Energy: " + PlayerClass.instance.energy.Value;




        smallStatsTextList[0].text = "Strength: " + PlayerClass.instance.strength.Value.ToString();
        smallStatsTextList[1].text = "Dexterity:" + PlayerClass.instance.dexterity.Value.ToString();
        smallStatsTextList[2].text = "Endurance " + PlayerClass.instance.endurance.Value.ToString();
        smallStatsTextList[3].text = "Energy: " + PlayerClass.instance.energy.Value.ToString();
        smallStatsTextList[4].text = "Armor: " + PlayerClass.instance.armor.Value.ToString();
        smallStatsTextList[5].text = "Health: " + PlayerClass.instance.maxHealth.Value.ToString();
        smallStatsTextList[6].text = "Mana: " + PlayerClass.instance.maxMana.Value.ToString();
        smallStatsTextList[7].text = "Stamina: " + PlayerClass.instance.stamina.Value.ToString();

       
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
