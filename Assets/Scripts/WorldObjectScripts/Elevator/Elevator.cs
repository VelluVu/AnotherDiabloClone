using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public float elevateSpeed;
    public bool up;
    public float maxDistance;
    Vector2 pos;
    ElevatorTrigger trig;
    public List<ElevatorLever> levers = new List<ElevatorLever>();
    bool started;

    private void Start ( )
    {
       
        pos = new Vector2 ( transform.position.x, transform.position.y );
        trig = gameObject.GetComponentInChildren<ElevatorTrigger> ( );
        
    }

    private void Update ( )
    {
     
        if( up && transform.position.y >= maxDistance )
        {
            Debug.Log ( "Saavuttiin päämäärään" );
            started = false;
            up = false;
            trig.ResetTrigger ( );
            foreach ( var lever in levers )
            {
                lever.ResetTrigger ( );
            }
            
        }
        if(  !up && transform.position.y <= pos.y  )
        {
            Debug.Log ( "Saavuttiin päämäärään" );
            started = false;
            up = true;
            trig.ResetTrigger ( );
            foreach ( var lever in levers )
            {
                lever.ResetTrigger ( );
            }
        }

        if( started)
        {
            MoveElevator ( );
        }
        
    }

    public void StartElevator ( )
    {
        Debug.Log ( "Triggered Elevator " );

        started = true;
        
    }

    void MoveElevator()
    {

        if ( up && transform.position.y <= maxDistance )
        {

            transform.Translate ( Vector3.up * elevateSpeed * Time.deltaTime );

        }
        else if ( !up && transform.position.y >= pos.y )
        {

            transform.Translate ( Vector3.down * elevateSpeed * Time.deltaTime );

        }
    }
    

}
