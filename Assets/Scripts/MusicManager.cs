using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource musicPlayer;
    public List<AudioClip> tracks = new List<AudioClip> ( );
    //public Transform [ ] musicTriggers;

    private void Awake ( )
    {
        musicPlayer = gameObject.GetComponent<AudioSource> ( );
    }

    private void OnEnable ( )
    {
        MusicTrigger.EnterAreaEvent += ChangeTrack;
    }

    private void OnDisable ( )
    {
        MusicTrigger.EnterAreaEvent -= ChangeTrack;
    }

    public void ChangeTrack ( AreaName curAreaName )
    {

        Debug.Log ( curAreaName );

        switch ( curAreaName )
        {
            case AreaName.Area1:
                Debug.Log ( musicPlayer.clip );
                Debug.Log ( tracks [ 0 ] );

                musicPlayer.clip = tracks [ 0 ];

                break;

            case AreaName.Area2:
                Debug.Log ( musicPlayer.clip );
                Debug.Log ( tracks [ 0 ] );

                musicPlayer.clip = tracks [ 1 ];

                break;

            case AreaName.Area3:
                Debug.Log ( musicPlayer.clip );
                Debug.Log ( tracks [ 0 ] );

                musicPlayer.clip = tracks [ 2 ];

                break;

            case AreaName.Area4:
                Debug.Log ( musicPlayer.clip );
                Debug.Log ( tracks [ 0 ] );

                musicPlayer.clip = tracks [ 3 ];

                break;

            default:
                break;

        }

        musicPlayer.Play ( );


    }
}
