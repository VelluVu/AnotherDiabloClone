using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBarScript : MonoBehaviour
{
    public GameObject player;
    public AbilityContainer abilityContainer;
    public List<Ability> activeAbilities;
    public List<int> activeIDs;
    public AbilityCoolDown[] cooldownButtons;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //abilityContainer = Resources.FindObjectsOfTypeAll<AbilityContainer>()[0];
    }
    private void Start()
    {
        cooldownButtons = GetComponentsInChildren<AbilityCoolDown>();
        StartGame();
    }


    public void StartGame()
    {
        for (int i = 0; i < cooldownButtons.Length; i++)
        {
            cooldownButtons[i].Initialize(abilityContainer.activeAbilities[i], player);
            cooldownButtons[i].id = i;
        }
    }
}
