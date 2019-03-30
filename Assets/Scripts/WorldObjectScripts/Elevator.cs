using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    Rigidbody2D rb;
    public float elevateSpeed;
    public bool up;
    public float maxDistance;
    Vector2 pos;
    ElevatorTrigger trig;
    bool started;

    private void Start ( )
    {
        rb = gameObject.GetComponent<Rigidbody2D> ( );
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
        }
        if(  !up && transform.position.y <= pos.y  )
        {
            Debug.Log ( "Saavuttiin päämäärään" );
            started = false;
            up = true;
            trig.ResetTrigger ( );
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
