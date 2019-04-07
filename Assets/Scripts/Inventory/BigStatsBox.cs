using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BigStatsBox : MonoBehaviour
{

    public GameObject background;
    public static BigStatsBox instance;
    public Button detailsButton;
    public TMP_Text detailsText;

    public List<TMP_Text> textList = new List<TMP_Text>();
    
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
        textList[4].text = "Critical Hit Chance: " + PlayerClass.instance.criticalHitChance.Value +"%";
        textList[5].text = "Critical Hit Damage: " + PlayerClass.instance.criticalHitDamage.Value;
        //textList[6].text = "Energy: " + PlayerClass.instance.energy.Value;
        //textList[7].text = "Energy: " + PlayerClass.instance.energy.Value;

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
