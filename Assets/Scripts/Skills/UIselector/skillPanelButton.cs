using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class skillPanelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // tarkoituksenka pitää holdia vain mikä ability kyseessä ja ehkä jonkilainen skill discription
    public Ability ability;
    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)button).OnPointerEnter(eventData);
        Debug.Log("Display Desc");
        
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)button).OnPointerExit(eventData);
        
    }


}
