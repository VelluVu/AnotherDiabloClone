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

    public List<StatDisplay> statDisplayList = new List<StatDisplay>();
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
        statDisplayList[0].Setup("<color=yellow>" + PlayerClass.instance.strength.Value.ToString("F0") + "</color>"+" Strength " ,"Strength helps your attack damage");
        statDisplayList[1].Setup( "<color=yellow>" + PlayerClass.instance.dexterity.Value.ToString("F0") + "</color>"+ " Dexterity","EMPTY");
        statDisplayList[2].Setup("<color=yellow>" + PlayerClass.instance.endurance.Value.ToString("F0") + "</color>" + " Endurance", "EMPTY");
        statDisplayList[3].Setup("<color=yellow>" + PlayerClass.instance.energy.Value.ToString("F0") + "</color>" + " Energy", "EMPTY");
        // Offensive Stats
        statDisplayList[4].Setup("<color=yellow>"+PlayerClass.instance.criticalHitChance.Value.ToString("F0") + "%"+"</color>" + " Critical Hit Chance", "EMPTY");
        statDisplayList[5].Setup("<color=yellow>"+PlayerClass.instance.criticalHitDamage.Value.ToString("F0") + "%"+ "</color>" + " Critical Hit Damage", "EMPTY");
        statDisplayList[6].Setup("<color=yellow>"+(1/ PlayerClass.instance.baseAttackSpeed.Value).ToString("F2")+ "</color>"+ " Attacks per second", "EMPTY");
        statDisplayList[7].Setup("<color=yellow>+" +Mathf.Round(PlayerClass.instance.cooldownReduction.Value).ToString("F0")+ "% </color>Cooldown Reduction", "EMPTY");

        // Defensive Stats
        statDisplayList[8].Setup("<color=yellow>"+PlayerClass.instance.armor.Value.ToString("F0") + "</color>" + " Armor", "You take less damage from hits");
        statDisplayList[9].Setup("<color=yellow>" + (PlayerClass.instance.fireResistance.Value + PlayerClass.instance.allResistance.Value).ToString("F0") + "</color>" + " Fire Resistance" , "You take less damage from fire effects");
        statDisplayList[10].Setup("<color=yellow>" + (PlayerClass.instance.coldResistance.Value + PlayerClass.instance.allResistance.Value).ToString("F0") + "</color>" + " Cold Resistance", "You take less damage from cold effects");
        statDisplayList[11].Setup("<color=yellow>" + (PlayerClass.instance.lightningResistance.Value + PlayerClass.instance.allResistance.Value).ToString("F0") + "</color>" + " Lightning Resistance", "You take less damage from lightning effects");
        statDisplayList[12].Setup("<color=yellow>" + (PlayerClass.instance.poisonResistance.Value + PlayerClass.instance.allResistance.Value).ToString("F0") + "</color>" + " Poison Resistance", "You take less damage from poison effects");
        statDisplayList[13].Setup("<color=yellow>" + (PlayerClass.instance.physicalResistance.Value + PlayerClass.instance.allResistance.Value).ToString("F0") + "</color>" + " Physical Resistance", "You take less damage from physical effects");
        statDisplayList[14].Setup("<color=yellow>" + (PlayerClass.instance.meleeDamageReduction.Value).ToString("F0") + "</color>" + "% Melee Damage Reduction", "You take less damage from melee attacks");
        statDisplayList[15].Setup("<color=yellow>" + (PlayerClass.instance.rangedDamageReduction.Value).ToString("F0") + "</color>" + "% Ranged Damage Reduction", "You take less damage from ranged attacks");
        statDisplayList[16].Setup("<color=yellow>" + (PlayerClass.instance.block.Value).ToString("F0") + "</color>" + " Block", "You take less damage when blocking");

        //Recovery Stats
        statDisplayList[17].Setup("<color=yellow>"+(PlayerClass.instance.healthRegeneration.Value).ToString("F0")+ "</color>" + " Health Regeneration", "Regain health over time");
        statDisplayList[18].Setup("<color=yellow>"+(PlayerClass.instance.healthOnKill.Value).ToString("F0")+ "</color>" + " Life On Kill", "Regain life on kill");
        // Utility Stats
        statDisplayList[19].Setup("<color=yellow>+"+(PlayerClass.instance.goldFind.Value).ToString("F0") + "</color>" + "% Gold Find", "You gain more gold");
        statDisplayList[20].Setup("<color=yellow>+"+(PlayerClass.instance.magicFind.Value).ToString("F0") + "</color>" + "% Magic Find", "You are more adept at finding magical items");
       
        
       


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
            BuildText();
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
