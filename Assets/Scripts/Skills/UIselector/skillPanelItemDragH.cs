using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class skillPanelItemDragH : MonoBehaviour, IDragHandler,IEndDragHandler,IBeginDragHandler
{
    public bool isDragable = true;
    private AbilityBarScript abilityBar;
    private skillPanelButton skillPanelButton;

    private AbilityContainer abilityContainer;
    private GameObject newButton;

    Vector2 max;
    Vector2 min;
    RectTransform rectThis;
    Transform lastTrans;

    
    
    private void OnEnable()
    {
        abilityBar = FindObjectOfType<AbilityBarScript>();        
        skillPanelButton = GetComponent<skillPanelButton>();
        abilityContainer = Resources.FindObjectsOfTypeAll<AbilityContainer>()[0];
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragable)
            return;
        
        skillPanelButton.toolTip.SetActive(false);
        
        
        transform.position = Input.mousePosition;
    }

    // kun draggi loppuu niin palautetaan otettu buttoni takas paikoilleen ja testataan jos hiiri on hotbaarilla
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragable)
            return;

        rectThis.offsetMin = min;
        rectThis.offsetMax = max;
        transform.SetSiblingIndex(lastTrans.GetSiblingIndex());
        //Invoke("addAbilityToBar", 0.05f);
        StartCoroutine(addAbilityToBar());
        Destroy(newButton);
    }

    // laittaa abilityn hot baarille jos siinä ei oo cooldownia ja jos se on dragatty
    IEnumerator addAbilityToBar()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        

        int id = abilityBar.mouseOverId;
        Ability ability = abilityBar.abilityInMouse;
        if(id >= 0 && ability != null)
        {
            
            abilityBar.setNewAbility(ability, id);
        }

        abilityBar.abilityInMouse = null;
    }

    // Tekee  vale kuvan skillista skillpaneeliin ja laittaa abilitin ylös mikä otettu hiirellä kiinni
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if(!isDragable)
        {
            return;
        }

        rectThis = GetComponent<RectTransform>();
        lastTrans = transform;
        transform.SetAsLastSibling();
        max = rectThis.offsetMax;
        min = rectThis.offsetMin;
        RectTransform rectT;

        newButton = Instantiate(this.gameObject, Input.mousePosition, Quaternion.identity);        
        newButton.transform.SetParent(this.gameObject.transform.parent, false);
        newButton.GetComponent<Image>().sprite = this.gameObject.GetComponent<Image>().sprite;
        newButton.transform.SetAsFirstSibling();

        rectT = newButton.GetComponent<RectTransform>();
        rectT.offsetMax = max;
        rectT.offsetMin = min;

        abilityBar.abilityInMouse = skillPanelButton.ability;
        
        
    }
}
