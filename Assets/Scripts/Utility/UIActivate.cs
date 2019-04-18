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
        DoorLeverScript.DoorLeverActivationInfoDurationEvent += TextDisplayDuration;
        ElevatorLever.ElevatorLeverActivationInfoDurationEvent += TextDisplayDuration;

    }
    private void OnDisable ( )
    {
        DoorLeverScript.DoorLeverActivationInfoDurationEvent -= TextDisplayDuration;
        ElevatorLever.ElevatorLeverActivationInfoDurationEvent -= TextDisplayDuration;
    }

    public void TextDisplayDuration( float fadeTime )
    {
        eventMessageUI.SetActive ( true );
        StartCoroutine ( TimeToDeactivateUI ( eventMessageUI, fadeTime ) );
    }

    IEnumerator TimeToDeactivateUI ( GameObject ui, float time)
    {
        yield return new WaitForSeconds ( time );
        ui.SetActive ( false );
    }
}
