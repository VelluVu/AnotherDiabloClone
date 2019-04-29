using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingCombatTextManager: MonoBehaviour
{

    public GameObject floatingCombatText;
    Vector2 textSpawnPos;

    //public delegate void PlayerNotifyDelecate ( Transform transform, string message, Color color );
    //public static event PlayerNotifyDelecate playerNotifyEvent;

    private void OnEnable ( )
    {

        Player.playerNotifyEvent += NotifyEvent;
        StateController.enemyNotifyEvent += NotifyEvent;
        DoorLeverScript.LeverActivationFloatingTextEvent += SpawnText;
        ElevatorLever.OnElevatorLeverActivateEvent += NotifyEvent;
     
    }

    private void OnDisable ( )
    {     
        
        Player.playerNotifyEvent -= NotifyEvent;
        StateController.enemyNotifyEvent -= NotifyEvent;
        DoorLeverScript.LeverActivationFloatingTextEvent -= SpawnText;
        ElevatorLever.OnElevatorLeverActivateEvent -= NotifyEvent;

    }

    public void NotifyEvent ( Vector2 position, float fadeTime )
    {
        SpawnText ( position, "Clunk", Color.yellow );
    }

    public void NotifyEvent ( Transform transform, string message, Color color )
    {

        SpawnText ( transform.position, message, color );

    }

    /// <summary>
    /// Ilmoitus funktio
    /// </summary>
    /// <param name="message"></param>
    /// <param name="spawnPos"></param>
    /// <param name="textColor"></param>
    public void SpawnText ( Vector2 spawnPos, string message, Color textColor )
    {
        textSpawnPos = new Vector2 ( Random.Range ( -0.01f, 0.01f ), 0.05f );
        GameObject text = Instantiate ( floatingCombatText, spawnPos + textSpawnPos, Quaternion.identity );
        text.transform.SetParent(ReferenceHolder.instance.combatTextHolder);
        TextMeshProUGUI textMesh = text.GetComponentInChildren<TextMeshProUGUI> ( );
        textMesh.color = textColor;
        textMesh.text = message;

    }
}
