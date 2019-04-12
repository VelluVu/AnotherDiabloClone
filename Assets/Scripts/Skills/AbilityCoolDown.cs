using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilityCoolDown : MonoBehaviour
{
    public string abilityButton = "1";
    public Image darkMask;
    public Text coolDownTextDisplay;

    //tämä ability pitää määrittää vielä jossain nyt testiks se o serial..
    [SerializeField] private Ability ability;
    private GameObject player;

    private Image myButtonImage;
    private AudioSource abilitySource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Initialize(ability, player);
    }

    public void Initialize(Ability selectedAbility, GameObject player)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();
        myButtonImage.sprite = ability._sprite;
        darkMask.sprite = ability._sprite;
        coolDownDuration = ability._cooldown;
        ability.Initialize(player);
        AbilityReady();
    }

    // Update is called once per frame
    void Update()
    {
        bool coolDownComplete = (Time.time > nextReadyTime);
        if (coolDownComplete)
        {
            AbilityReady();
            if (Input.GetKeyDown(abilityButton))
            {
                ButtonTriggered();
            }
        }
        else
        {
            CoolDown();
        }
    }

    private void AbilityReady()
    {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCd.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    private void ButtonTriggered()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;

        //abilitySource.clip = ability._sound;
        //if(abilitySource.clip != null)
        //    abilitySource.Play();
        ability.TriggerAbility();
    }

}
