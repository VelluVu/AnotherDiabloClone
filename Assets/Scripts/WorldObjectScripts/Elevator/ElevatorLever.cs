using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLever : MonoBehaviour
{
    bool triggered;
    public float infoTextFadeTime;
    public List<ElevatorTrigger> triggers = new List<ElevatorTrigger>();
    public List<Elevator> elevators = new List<Elevator>();

    public delegate void ElevatorLeverActivationInfoDurationDelegate ( float duration );
    public static event ElevatorLeverActivationInfoDurationDelegate ElevatorLeverActivationInfoDurationEvent;

    public delegate void OnElevatorLeverActivateDelegate( Vector2 position, float infoTextFadeTime );
    public static event OnElevatorLeverActivateDelegate OnElevatorLeverActivateEvent;

    private void OnTriggerStay2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Player" ) && Input.GetButtonDown ( "Interaction" ) )
        {
            if ( !triggered )
            {
                triggered = true;

                OnLeverActivated ( );
              
                foreach ( var elevator in elevators )
                {
                    elevator.StartElevator ( );
                }

                foreach ( var trigger in triggers )
                {
                    trigger.UsedLever ( );
                }
            }
        }
    }

    void OnLeverActivated()
    {
        if( ElevatorLeverActivationInfoDurationEvent != null)
        {
            ElevatorLeverActivationInfoDurationEvent ( infoTextFadeTime );
        }

        if ( OnElevatorLeverActivateEvent != null )
        {
            OnElevatorLeverActivateEvent ( transform.position, infoTextFadeTime );
        }
    }

    public void ResetTrigger ( )
    {
        triggered = false;
    }

    public void UsedTrigger()
    {
        triggered = true;
    }

}
