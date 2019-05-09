using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBarScript : MonoBehaviour
{
    //pää tarkoitus tälle scriptille on se että laittaa skillit baarille 
    //containeristä pelin alussa 

    private GameObject player;
    
       
    public Color cooldownTextColor=Color.white;
    public Color darkMaskColor = Color.black;
    public Color displayTextColor = Color.gray;

    public int mouseOverId=-1;
    public Ability abilityInMouse;
    [HideInInspector] public AbilityCoolDown[] cooldownButtons;
    public AbilityContainer abilityContainer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        abilityContainer = Resources.FindObjectsOfTypeAll<AbilityContainer>()[0];
        
    }
    private void Start()
    {
        cooldownButtons = GetComponentsInChildren<AbilityCoolDown>();
        StartGame();
    }

    void setAllAbilityThings()
    {
        for(int i = 0; i < abilityContainer.allAbilities.Length; i++)
        {

        }
    }

    bool isAbilityInBar(Ability ability)
    {
        for(int i = 0; i < cooldownButtons.Length; i++)
        {
            if (cooldownButtons[i].getAbility() == ability)
                return true;
        }
        return false;
    }
    // pystytään laittamaan uusi ability listasta idN perusteella
    public void setNewAbility(Ability ab,int buttonId)
    {
        if (cooldownButtons[buttonId].notInCooldown && ab.isEnabled && !isAbilityInBar(ab))
        {
            cooldownButtons[buttonId].Initialize(ab, player);
            abilityContainer.activeAbilities[buttonId] = ab;
        }
        else
        {
            Debug.Log("You cant add new skill if its in cooldown, not in enabled or is already in bar");
        }
        
    }

    
    // laukaistaan kun peli ailkaa ja laitetaan skillit paikoilleen hotbaarille ja kaikki tarvittava samalla
    public void StartGame()
    {
        
        for (int i = 0; i < cooldownButtons.Length; i++)
        {
            AbilityCoolDown ac = cooldownButtons[i];            
            ac.Initialize(abilityContainer.activeAbilities[i], player);
            ac.id = i;
            ac.coolDownTextDisplay.color = cooldownTextColor;
            ac.darkMask.color = darkMaskColor;
            ac.abilityName.color=displayTextColor;
        }
    }
}
