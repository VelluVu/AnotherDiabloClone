using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    bool triggered;

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if(collision.gameObject.CompareTag("Player") && !triggered)
        {
            triggered = true;
            gameObject.GetComponentInParent<Elevator> ( ).StartElevator (  );
        }
    }

    public void ResetTrigger()
    {
        triggered = false;
    }

}
