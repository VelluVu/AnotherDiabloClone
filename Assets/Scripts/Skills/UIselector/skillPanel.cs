using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillPanel : MonoBehaviour
{
    public int countOfButtons = 4;
    public Transform scrollpanel;
    public GameObject button;
    public GameObject leftButton;
    public GameObject rightButton;
    public List<Ability> abilities=new List<Ability>();
    public List<skillPanelButton> skillpanelButtons = new List<skillPanelButton>();
    private List<Ability> xAb;
    [HideInInspector]public List<GameObject> buttons;

    private RectTransform buttonRect;
    private float widthButton;
    private float heighButton;
    private float cap = 20;
       
    void Start()
    {
        buttonRect = scrollpanel.GetComponent<RectTransform>();
        widthButton = buttonRect.rect.width;
        heighButton = buttonRect.rect.height;

        setButtons();
    }

    void setButtons()
    {
        RectTransform newBRect;
        Image sprite;
        Ability ability;
        skillPanelButton skillPanelButton;
        float x = 0;
        for (int i = 0; i < countOfButtons; i++)
        {
            x = cap;
            GameObject newB = Instantiate(button) as GameObject;
            newBRect = newB.GetComponent<RectTransform>();
            sprite = newB.GetComponent<Image>();
            skillPanelButton = newB.GetComponent<skillPanelButton>();
            skillpanelButtons.Add(skillPanelButton);
       
            newB.transform.SetParent(scrollpanel, false);
            newBRect.offsetMax = new Vector2((widthButton + x) * i, 0);
            newBRect.offsetMin = new Vector2((heighButton + x) * i, 0);
            ability = abilities[i];
            sprite.sprite = ability._sprite;
            skillPanelButton.ability = ability;

            buttons.Add(newB);
           
        }
        
        buttonRect.localPosition += new Vector3(((widthButton+cap)/2)*(-1 + countOfButtons), 0, 0)*-1;
        leftButton.GetComponent<RectTransform>().localPosition = new Vector3(cap + (102)*countOfButtons,0,0)*-1;
        rightButton.GetComponent<RectTransform>().localPosition = new Vector3(cap + (102)*countOfButtons,0,0);
    }
    // refressaaa skillpaneelin skillit => booli on nappi joko vasen tai oikea.. muuttaa skillien järjestyksen taulussa
    public void moveAbilitiesTo(bool toRight)
    {
        Image sprite;
        Ability ability;
        skillPanelButton skillPanelButton;
        GameObject button;       
        if (abilities.Count <= countOfButtons)
        {            
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
        for (int i = 0; i < countOfButtons; i++)
        {
            button = buttons[i];
            sprite = button.GetComponent<Image>();
            skillPanelButton = button.GetComponent<skillPanelButton>();
            ability = abilities[i];
            sprite.sprite = ability._sprite;
            skillPanelButton.ability = ability;
        }

        foreach (skillPanelButton spb in skillpanelButtons)
        {
            spb.checkAbilityLevel();
        }
        

    }

}
