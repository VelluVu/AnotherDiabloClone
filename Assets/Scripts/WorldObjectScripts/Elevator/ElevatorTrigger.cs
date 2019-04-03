using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    bool triggered;
    public List<ElevatorLever> levers = new List<ElevatorLever> ( );

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if(collision.gameObject.CompareTag("Player") && !triggered)
        {
            triggered = true;
            gameObject.GetComponentInParent<Elevator> ( ).StartElevator (  );
            foreach ( var lever in levers )
            {
                lever.UsedTrigger ( );
            }
        }
    }

    public void ResetTrigger()
    {
        triggered = false;
    }

    public void UsedLever()
    {
        triggered = true;
    }

}
