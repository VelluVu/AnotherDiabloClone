using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class skillPanelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // tarkoituksenka pitää holdia vain mikä ability kyseessä ja ehkä jonkilainen skill discription
    public Ability ability;
    public Ability lastAbility;
    public GameObject toolTip;
    public Image darkMask;
    public float playerLevel=99;
    public float lastPlayerLevel;

    private GameObject player;
    
    private PlayerClass pc;
    private skillPanelItemDragH skillPanelItemDragH;
    buttonToolTip buttontoolTip;

    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerClass>();
        skillPanelItemDragH = GetComponent<skillPanelItemDragH>();
        buttontoolTip = toolTip.GetComponent<buttonToolTip>();
       
        
        //toolTip.SetActive(false);
       
    }

    private void Start()
    {
        playerLevel = pc.playerLevel.BaseValue;
        lastPlayerLevel = playerLevel;
        if(ability == null)
        {
            return;
        }
        lastAbility = ability;
        darkMask.sprite = ability._sprite;
        darkMask.enabled = false;
        checkAbilityLevel();
        setToolTipValues();
        toolTip.SetActive(false);

    }

    public void checkAbilityLevel()
    {
        lastPlayerLevel = playerLevel;
        

        if(playerLevel >= ability.levelToUnlock  || ability.isEnabled)
        {
            darkMask.enabled = false;
            skillPanelItemDragH.isDragable = true;
        }
        else
        {
            darkMask.enabled = true;
            skillPanelItemDragH.isDragable = false;
        }

        setToolTipValues();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)button).OnPointerEnter(eventData);
        GameObject ob = transform.parent.transform.parent.gameObject;
       
        if(ob != null)
        {
            transform.parent.transform.parent.transform.SetAsLastSibling();
        }
        toolTip.SetActive(true);
        
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)button).OnPointerExit(eventData);
        toolTip.SetActive(false);
    }

    void setToolTipValues()
    {
        buttontoolTip.setNameText(ability._name);
        buttontoolTip.setManaText(ability._manaUsage);
        buttontoolTip.setCooldownText(ability._cooldown);
        buttontoolTip.setDurationText(ability._duration);
        buttontoolTip.setDescText(ability.desc);
    }

    private void Update()
    {
        playerLevel = pc.playerLevel.BaseValue;
        if(playerLevel != lastPlayerLevel)
        {
            checkAbilityLevel();
        }

        
    }

}
