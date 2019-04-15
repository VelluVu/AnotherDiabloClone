using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public float elevateSpeed;
    public bool up;
    public float maxDistance;
    Rigidbody2D rb;
    Vector2 pos;
    ElevatorTrigger trig;
    public List<ElevatorLever> levers = new List<ElevatorLever> ( );
    public List<Transform> checkpoints = new List<Transform> ( );
    public bool started;
    Transform next;
    public bool isSimpleUpDownElevator;

    public delegate void ElevatorMovedDelegate ( GameObject elevator, int checkpointIndex );
    public static event ElevatorMovedDelegate elevatorMovedEvent;

    public delegate void ElevatorReachedCheckpoint ( int checkpointIndex );
    public static event ElevatorReachedCheckpoint elevatorReachedCheckpointEvent;

    private void Start ( )
    {

        rb = gameObject.GetComponent<Rigidbody2D> ( );
        pos = new Vector2 ( transform.position.x, transform.position.y );
        trig = gameObject.GetComponentInChildren<ElevatorTrigger> ( );

        if ( !isSimpleUpDownElevator )
        {
            next = checkpoints [ 1 ];
            up = true;
        }
    }

    private void OnEnable ( )
    {
        elevatorMovedEvent += OnLastCheckPoint;
     
    }

    private void OnDisable ( )
    {
        elevatorMovedEvent -= OnLastCheckPoint;
    
    }

    private void Update ( )
    {
        if ( isSimpleUpDownElevator )
        {
            SimpleElevator ( );
        }
        else
        {
            CheckPointElevator ( );
        }
    }

    public void OnLastCheckPoint ( GameObject elevator, int checkpointIndex )
    {
        if ( elevator == gameObject )
        {

            started = false;

            if ( checkpointIndex > 0 )
            {
                up = false;
                next = checkpoints [ checkpointIndex - 1 ];
            }
            else
            {
                up = true;
                next = checkpoints [ checkpointIndex + 1 ];
            }
        }
    }

    public void CheckPointElevator ( )
    {

        if ( started )
        {
            Traverse ( );

            
        }
        if ( !started )
        {
            trig.ResetTrigger ( );

            foreach ( var lever in levers )
            {
                lever.ResetTrigger ( );
            }

        }
    }

    public void Traverse ( )
    {
        if ( Vector2.Distance ( transform.position, next.position ) > 0 )
        {
            transform.Translate ( ( next.position - transform.position ) * elevateSpeed * Time.deltaTime );
        }
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {     
            if ( collision.gameObject.transform == next )
            {
                Debug.Log ( collision.gameObject.name );

            if ( checkpoints [ checkpoints.Count - 1 ] == collision.gameObject.transform )
            {
                Debug.Log ( "End Checkpoint" );
                if ( elevatorMovedEvent != null )
                {
                    elevatorMovedEvent ( gameObject, checkpoints.Count - 1 );
                }
            }
            else if ( checkpoints [ 0 ] == collision.gameObject.transform )
            {
                Debug.Log ( "Start Checkpoint" );
                if ( elevatorMovedEvent != null )
                {
                    elevatorMovedEvent ( gameObject, 0 );
                }
            }
            else
            {
                if ( up )
                {
                    Debug.Log ( collision.gameObject.name );
                    next = checkpoints [ checkpoints.IndexOf ( collision.gameObject.transform ) + 1 ];
                }
                else
                {
                    Debug.Log ( collision.gameObject.name );
                    next = checkpoints [ checkpoints.IndexOf ( collision.gameObject.transform ) - 1 ];
                }
            }
        }
    }


    public void SimpleElevator ( )
    {
        if ( up && transform.position.y >= maxDistance )
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
        if ( !up && transform.position.y <= pos.y )
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

        if ( started )
        {
            MoveElevator ( );
        }
    }

    public void StartElevator ( )
    {
        Debug.Log ( "Triggered Elevator " );

        started = true;

    }

    void MoveElevator ( )
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
