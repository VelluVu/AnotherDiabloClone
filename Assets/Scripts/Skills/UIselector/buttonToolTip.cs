using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class buttonToolTip : MonoBehaviour
{
    
    public TMP_Text nameOfSkill;
    public TMP_Text cooldownText;
    public TMP_Text manaText;
    public TMP_Text durationText;
    public TMP_Text descText;

    public void setNameText(string name)
    {
        nameOfSkill.text = name;
    }

    public void setCooldownText(float cooldown)
    {
        if(cooldown <= 0)
        {
            cooldownText.enabled = false;
        }
        else
        {
            cooldownText.enabled = true;
        }

        cooldownText.text = "Cooldown: " + cooldown.ToString(); 
    }

    public void setManaText(float mana)
    {
        if(mana <= 0)
        {
            manaText.enabled = false;
        }
        else
        {
            manaText.enabled = true;
        }
        manaText.text = "Mana: " + mana.ToString();
    }

    public void setDurationText(float duration)
    {
        if(duration <= 0)
        {
            durationText.enabled = false;
        }
        else
        {
            durationText.enabled = true;
        }

        durationText.text = "Duration: " + duration.ToString();
    }

    public void setDescText(string desc)
    {
        descText.text = desc;
    }
    


}
