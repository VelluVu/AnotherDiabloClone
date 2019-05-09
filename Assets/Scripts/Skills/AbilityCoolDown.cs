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
    public Image durationBar;
    public Text coolDownTextDisplay;
    public Text abilityName;
    public bool notInCooldown;
 
    private Ability ability;
    private GameObject player;
    private AbilityBarScript bar;

    private Image myButtonImage;
    private AudioSource abilitySource;
    private float coolDownDuration;
    private float skillDuration;
    private float skillDurationTimeLeft;
    private float nextReadyTime;
    private float coolDownTimeLeft;

    private PlayerClass pc;
    private Player p;
    private bool isEnabled;
    private bool showDurBar;
    private bool allowDoAir;
    private float manaUsage;
    private float manaNow;
    private float playerMana;
   
    private float playerLvl;
    private float lastPlayerLvl;

    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        darkMask.fillAmount = 1;
        clickButton.onClick.AddListener(ButtonTriggered);
        bar = GetComponentInParent<AbilityBarScript>();
        pc = player.GetComponent<PlayerClass>();
        p = player.GetComponent<Player>();
        
        
        
    }

    public void Initialize(Ability selectedAbility, GameObject player)
    {
        ability = selectedAbility;      
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();
        myButtonImage.sprite = ability._sprite;
        darkMask.sprite = ability._sprite;
        coolDownDuration = ability._cooldown;
        skillDuration = ability._duration;
        showDurBar = ability._showDurBar;
        allowDoAir = ability._allowDoThisInAir;
        manaUsage = ability._manaUsage;
        
        ability.Initialize(player);
        AbilityReady();

        abilityName.text = ability._name;
        abilityName.enabled = false;

        durationBar.enabled = false;
        
        playerLvl = PlayerClass.instance.playerLevel.BaseValue;
        lastPlayerLvl = playerLvl;

        if (ability.checkLevel && playerLvl >= ability.levelToUnlock  )
        {
            ability.isEnabled = true;
        }

        isEnabled = ability.isEnabled;
        
       


    }

    // Update is called once per frame
    void Update()
    {
        playerLvl = pc.playerLevel.BaseValue;
        manaNow = pc.mana.BaseValue;
        

        if (playerLvl != lastPlayerLvl)
        {
            Debug.Log("GOT LEVEL!");            
            Initialize(ability, player);
        }

        bool coolDownComplete = (Time.time > nextReadyTime);
        notInCooldown = coolDownComplete;
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

        actDurBar();
        //checkMana();
    }

    public Ability getAbility()
    {
        return ability;
    }
    private void AbilityReady()
    {
        darkMask.fillAmount = 1;
        coolDownTextDisplay.enabled = false;       
        clickButton.interactable = true;
        if (isEnabled && checkMana())
        {
            darkMask.enabled = false;
        }
        else
        {
            darkMask.enabled = true;
        }
        
    }

    private void CoolDown()
    {
        if (!isEnabled)
        {
            return;
        }
            
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCd.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
        clickButton.interactable = false;
    }
    
    bool checkMana()
    {
        if(manaNow < manaUsage)
        {
            // true;
            
            return false;
        }
        else
        {
            //false
            return true;
        }

    }
    bool checkInAir()
    {
        if(p.isAir && !allowDoAir)
        {
            return true;
        }

        return false;
    }
    void actDurBar()
    {
        if (skillDuration <= 0 || skillDurationTimeLeft <= 0 || !showDurBar )
        {
            if (durationBar.IsActive())
                durationBar.enabled = false;
            return;
        }
        if (!durationBar.IsActive())
            durationBar.enabled = true;

        skillDurationTimeLeft -= Time.deltaTime;
        durationBar.fillAmount = (skillDurationTimeLeft / skillDuration);


    }

    public void ButtonTriggered()
    {
        

        if (!isEnabled || manaNow < manaUsage || checkInAir())
        {
            return;
        }
            
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;
        skillDurationTimeLeft = skillDuration;

        //abilitySource.clip = ability._sound;
        //if(abilitySource.clip != null)
        //    abilitySource.Play();

        

        ability.TriggerAbility(ability);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)clickButton).OnPointerEnter(eventData);
        
        abilityName.enabled = true;
        bar.mouseOverId = id;
        
        
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)clickButton).OnPointerExit(eventData);
        abilityName.enabled = false;
        bar.mouseOverId = -1;
        
    }
}
