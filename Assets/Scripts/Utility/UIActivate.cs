using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivate : MonoBehaviour
{
    public GameObject combatUI;
    public GameObject eventMessageUI;

    private void Awake ( )
    {
        combatUI.SetActive ( true );
    }

    private void OnEnable ( )
    {
        DoorLeverScript.leverActivationEvent += ActivateEventMessageUI;
    }
    private void OnDisable ( )
    {
        DoorLeverScript.leverActivationEvent -= ActivateEventMessageUI;
    }

    public void ActivateEventMessageUI(float eventDuration)
    {
        eventMessageUI.SetActive ( true );
        StartCoroutine ( TimeToDeactivateUI (eventMessageUI, eventDuration));
    }

    IEnumerator TimeToDeactivateUI ( GameObject ui, float time)
    {
        yield return new WaitForSeconds ( time );
        ui.SetActive ( false );
    }
}
