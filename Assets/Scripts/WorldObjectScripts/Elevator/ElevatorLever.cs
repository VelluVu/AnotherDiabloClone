using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLever : MonoBehaviour
{
    bool triggered;
    public List<ElevatorTrigger> triggers = new List<ElevatorTrigger>();
    public List<Elevator> elevators = new List<Elevator>();

    private void OnTriggerStay2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Player" ) && !triggered && Input.GetButtonDown ( "Interaction" ))
        {

            triggered = true;

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

    public void ResetTrigger ( )
    {
        triggered = false;
    }

    public void UsedTrigger()
    {
        triggered = true;
    }

}
