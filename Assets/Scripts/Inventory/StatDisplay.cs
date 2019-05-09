using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


public class StatDisplay : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject toolTipObject;
    public GameObject toolTip;
    public TMP_Text _displayText;
    private string _toolTipText;
    private string tempString;
   

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered a Stat");
        toolTip = Instantiate(toolTipObject, transform);
        toolTip.transform.SetParent(ReferenceHolder.instance.UICanvas);
        toolTip.GetComponentInChildren<TMP_Text>().text = _toolTipText;
        tempString = _displayText.text;
        _displayText.text = "<color=red>" + tempString + "</color>";
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _displayText.text = tempString;
        Destroy(toolTip);
    }
    private void OnDisable()
    {
        if (toolTip!= null)
        {
            Destroy(toolTip);
        }
        _displayText.text = tempString;
    }

    public void Setup(string displayText,string toolTipText)
    {
       
        _toolTipText = toolTipText;
        _displayText.text = displayText;

    }

}
