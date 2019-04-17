using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillPanel : MonoBehaviour
{
    public Transform scrollpanel;
    public GameObject button;
    public List<Ability> abilities;
    private List<Ability> xAb;
    public List<GameObject> buttons;

    private ScrollRect scrollRect;

    private RectTransform buttonRect;
    private float widthButton;
    private float heighButton;
    private float cap = 20;
    private int countOFbuttons = 5;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    void Start()
    {
        buttonRect = scrollpanel.GetComponent<RectTransform>();
        widthButton = buttonRect.rect.width;
        widthButton = buttonRect.rect.height;
        

        setButtons();
    }

    void setButtons()
    {
        RectTransform newBRect;
        Image sprite;
        Ability ability;
        skillPanelButton skillPanelButton;
        float x = 0;
        for (int i = 0; i < countOFbuttons; i++)
        {
            x = cap;
            GameObject newB = Instantiate(button) as GameObject;
            newBRect = newB.GetComponent<RectTransform>();
            sprite = newB.GetComponent<Image>();
            skillPanelButton = newB.GetComponent<skillPanelButton>();
       
            newB.transform.SetParent(scrollpanel, false);
            newBRect.offsetMax = new Vector2((widthButton + x) * i, 0);
            newBRect.offsetMin = new Vector2((widthButton + x) * i, 0);
            ability = abilities[i];
            sprite.sprite = ability._sprite;
            skillPanelButton.ability = ability;

            buttons.Add(newB);
            

        }
    }

    public void moveAbilitiesTo(bool toRight)
    {
        Image sprite;
        Ability ability;
        skillPanelButton skillPanelButton;
        GameObject button;       
        if (abilities.Count <= countOFbuttons)
        {
            Debug.Log("RETUNR");
            return;
        }
        if(!toRight)
        {
            ability = abilities[0];
            abilities.RemoveAt(0);
            abilities.Add(ability);           
        }
        else
        {
            ability = abilities[abilities.Count - 1];
            abilities.RemoveAt(abilities.Count - 1);
            abilities.Insert(0, ability);
        }
        for (int i = 0; i < countOFbuttons; i++)
        {
            button = buttons[i];
            sprite = button.GetComponent<Image>();
            skillPanelButton = button.GetComponent<skillPanelButton>();
            ability = abilities[i];
            sprite.sprite = ability._sprite;
            skillPanelButton.ability = ability;
        }

    }


    private void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.M))
        {
            
        }

        
    }




}
