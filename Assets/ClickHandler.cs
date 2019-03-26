using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{

    #region pointerHandler
    public UnityEvent onLeft;
    public UnityEvent onRight;
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeft.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRight.Invoke();
        }

    }

    
}
