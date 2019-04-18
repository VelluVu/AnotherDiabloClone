using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class AbilityCoolDown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string abilityButton = "1";
    public int id;
    public Button clickButton;
    public Image darkMask;
    public Text coolDownTextDisplay;
    public Text abilityName;

    //public GameObject root;
    //public AbilityBarScript barS;
    

    //tämä ability pitää määrittää vielä jossain nyt testiks se o serial..
    private Ability ability;
    private GameObject player;

    private Image myButtonImage;
    private AudioSource abilitySource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        clickButton.onClick.AddListener(ButtonTriggered);
        //root = transform.root.gameObject;
        //barS = root.GetComponentInChildren<AbilityBarScript>();

        //väli aikainen
        //Initialize(ability, player);
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

        abilityName.text = ability._name;
        abilityName.enabled = false;

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
        clickButton.interactable = true;
    }

    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCd.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
        clickButton.interactable = false;
    }

    public void ButtonTriggered()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;

        //abilitySource.clip = ability._sound;
        //if(abilitySource.clip != null)
        //    abilitySource.Play();
        ability.TriggerAbility(ability);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)clickButton).OnPointerEnter(eventData);
        
        abilityName.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)clickButton).OnPointerExit(eventData);
        abilityName.enabled = false;
    }
}
