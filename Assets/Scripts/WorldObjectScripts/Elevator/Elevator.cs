using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    [Range ( 0.1f, 5f )] public float elevateSpeed;   
    [Range ( 0f, 25f )] public float maxDistance;

    //Rigidbody2D rb;
    Vector2 pos;
    ElevatorTrigger trig;
    Transform next;
    public GameObject spriteRendererHolder;
    public SpriteRenderer sr;
    BoxCollider2D coll;
    AudioSource source;

    public List<ElevatorLever> levers = new List<ElevatorLever> ( );
    public List<Transform> checkpoints = new List<Transform> ( );

    public bool up;
    public bool started;
    [Tooltip ("Uncheck to make it work with checkpoints")] public bool isSimpleUpDownElevator;

    public delegate void ElevatorMovedDelegate ( GameObject elevator, int checkpointIndex );
    public static event ElevatorMovedDelegate elevatorMovedEvent;

    public delegate void ElevatorReachedCheckpoint ( );
    public static event ElevatorReachedCheckpoint elevatorReachedCheckpointEvent;

    public delegate void ElevatorSoundDelegate ( AudioSource source, ObjectSoundType objSound );
    public static event ElevatorSoundDelegate ElevatorSoundEvent;

    private void Start ( )
    {
        coll = gameObject.GetComponent<BoxCollider2D> ( );
        pos = new Vector2 ( transform.position.x, transform.position.y );
        trig = gameObject.GetComponentInChildren<ElevatorTrigger> ( );
        source = gameObject.GetComponent<AudioSource> ( );

        coll.size = sr.sprite.bounds.size * new Vector2 (1 * spriteRendererHolder.transform.localScale.x,1 * spriteRendererHolder.transform.localScale.y );

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
            source.Pause ( );
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
        if ( Vector2.Distance ( transform.position, next.position ) != 0 )
        {
            //transform.Translate ( ( next.position - transform.position ) * elevateSpeed * Time.deltaTime );
            transform.position = Vector3.MoveTowards ( transform.position, next.position, elevateSpeed * Time.deltaTime );
        }
       
        if ( Vector2.Distance ( transform.position, next.position ) <= 0.1f )
        {
            if ( checkpoints [ checkpoints.Count - 1 ] == next.gameObject.transform )
            {
                Debug.Log ( "End Checkpoint" );
                if ( elevatorMovedEvent != null )
                {
                    elevatorMovedEvent ( gameObject, checkpoints.Count - 1 );
                }
            }
            else if ( checkpoints [ 0 ] == next.gameObject.transform )
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
                    Debug.Log ( next.gameObject.name );
                    Debug.Log ( "REACHED CHECKPOINT" );
                    if ( elevatorReachedCheckpointEvent != null )
                    {
                        elevatorReachedCheckpointEvent ( );
                    }
                    next = checkpoints [ checkpoints.IndexOf ( next.gameObject.transform ) + 1 ];
                }
                else
                {
                    Debug.Log ( next.gameObject.name );
                    Debug.Log ( "REACHED CHECKPOINT" );
                    if (elevatorReachedCheckpointEvent != null)
                    {
                        elevatorReachedCheckpointEvent ( );
                    }
                    next = checkpoints [ checkpoints.IndexOf ( next.gameObject.transform ) - 1 ];
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
            source.Pause ( );
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
        source.Play ( );
        if ( ElevatorSoundEvent  != null)
        {
            ElevatorSoundEvent ( source, ObjectSoundType.Elevator );
        }
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
    
    #region Commented Out
    //private void OnTriggerEnter2D ( Collider2D collision )
    //{

    //    if ( collision.gameObject.transform == next )
    //    {

    //        Debug.Log ( collision.gameObject.name );

    //        if ( checkpoints [ checkpoints.Count - 1 ] == collision.gameObject.transform )
    //        {
    //            Debug.Log ( "End Checkpoint" );
    //            if ( elevatorMovedEvent != null )
    //            {
    //                elevatorMovedEvent ( gameObject, checkpoints.Count - 1 );
    //            }
    //        }
    //        else if ( checkpoints [ 0 ] == collision.gameObject.transform )
    //        {
    //            Debug.Log ( "Start Checkpoint" );
    //            if ( elevatorMovedEvent != null )
    //            {
    //                elevatorMovedEvent ( gameObject, 0 );
    //            }
    //        }
    //        else
    //        {
    //            if ( up )
    //            {
    //                Debug.Log ( collision.gameObject.name );
    //                next = checkpoints [ checkpoints.IndexOf ( collision.gameObject.transform ) + 1 ];
    //            }
    //            else
    //            {
    //                Debug.Log ( collision.gameObject.name );
    //                next = checkpoints [ checkpoints.IndexOf ( collision.gameObject.transform ) - 1 ];
    //            }

    //        }
    //    }
    //}
    #endregion
}
