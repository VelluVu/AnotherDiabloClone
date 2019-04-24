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

    [HideInInspector] public int mouseOverId=-1;
    [HideInInspector] public Ability abilityInMouse;
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


    // pystytään laittamaan uusi ability listasta idN perusteella
    public void setNewAbility(Ability ab,int buttonId)
    {
        if (cooldownButtons[buttonId].notInCooldown)
        {
            cooldownButtons[buttonId].Initialize(ab, player);
            abilityContainer.activeAbilities[buttonId] = ab;
        }
        else
        {
            Debug.Log("You cant add new skill if its in cooldown");
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
