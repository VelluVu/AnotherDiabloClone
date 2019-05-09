using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Erinlaisten ovien toiminnallisuus:
/// 1. NormaaliOvi aukeaa napista
/// 2. Aukeaa napista jos pelaajalla on tietty avain
/// 3. Aukeaa kun väännetään vipua
/// 4. Aukeaa kun tietty bossi tuhotaan
/// </summary>
public class DoorScript : MonoBehaviour
{

    Animator doorAnimator;
    AudioSource source;
    BoxCollider2D doorCollider;
    BoxCollider2D triggerCol;
    [Header ( "Millä tavoin ovi aukaistaan" )]
    public DoorType doorType;
    [Header ( "AvainTyyppi minkä tarvii jos on avaimella avattava" )]
    public KeyType keyType;
    [Header ( "BossiLista, oviaukeaa joku näistä on null" )]
    public List<GameObject> bosses = new List<GameObject> ( );
    public bool isOpen;
    //string message, Color color, float fontSize, float fadeTime
    public delegate void DoorSoundDelegate ( AudioSource source, ObjectSoundType objSound );
    public static event DoorSoundDelegate DoorSoundEvent;

    public delegate void DoorActivateMessageDelegate ( string message, Color color, float fontSize, float fadeTime );
    public static event DoorActivateMessageDelegate doorActivateMessageEvent;

    public delegate void DoorActivationMessageDurationDelegate ( float duration );
    public static event DoorActivationMessageDurationDelegate DoorActivationMessageDurationEvent;

    private void Start ( )
    {
        doorAnimator = gameObject.GetComponent<Animator> ( );
        doorCollider = gameObject.GetComponent<BoxCollider2D> ( );
        triggerCol = gameObject.GetComponentInChildren<BoxCollider2D> ( );
        source = gameObject.GetComponent<AudioSource> ( );

    }

    public void Aukene ( )
    {
        if ( !isOpen )
        {
            if ( DoorSoundEvent != null )
            {
                DoorSoundEvent ( source, ObjectSoundType.Door );
            }
            doorAnimator.SetTrigger ( "OpenDoor" );
            isOpen = true;
            doorCollider.enabled = false;
            triggerCol.enabled = false;
            gameObject.layer = LayerMask.NameToLayer ( "Default" );
            gameObject.GetComponent<DoorScript> ( ).enabled = false;
        }
    }

    /// <summary>
    /// voisi olla myös bool joka saa true kun tulee palautus toiselta osapuolelta
    /// </summary>
    /// <param name="key"></param>
    public void Aukene ( KeyType key )
    {
        if ( key == keyType )
        {
            if ( !isOpen )
            {
                if ( DoorSoundEvent != null )
                {
                    DoorSoundEvent ( source, ObjectSoundType.Door );
                }
                isOpen = true;
                doorAnimator.SetTrigger ( "OpenDoor" );
                doorCollider.enabled = false;
                gameObject.GetComponent<DoorScript> ( ).enabled = false;
            }
        }
    }

    public void DoorConditionCheck ( )
    {

        switch ( doorType )
        {
            case DoorType.normalDoor:
                Aukene ( );
                break;

            case DoorType.keyLockedDoor:
               
                if ( doorActivateMessageEvent != null )
                {
                    doorActivateMessageEvent ( "This Door is locked", Color.white, 64f, 2f );
                }
                if( DoorActivationMessageDurationEvent != null )
                {
                    DoorActivationMessageDurationEvent ( 3f );
                }
                break;

            case DoorType.leverLockedDoor:
                
                if ( doorActivateMessageEvent != null )
                {
                    doorActivateMessageEvent ( "This Door Has Lever Mechanism connected to somewhere", Color.white, 64f, 2f );
                }
                if ( DoorActivationMessageDurationEvent != null )
                {
                    DoorActivationMessageDurationEvent ( 3f );
                }
                
                break;

            case DoorType.bossLockedDoor:
                foreach ( var boss in bosses )
                {
                    if ( boss == null )
                    {
                        Aukene ( );
                    }
                    else
                    {
                        if ( doorActivateMessageEvent != null )
                        {
                            doorActivateMessageEvent ( "This Door opens after boss kill", Color.white, 34f, 3f );
                        }
                        if ( DoorActivationMessageDurationEvent != null )
                        {
                            DoorActivationMessageDurationEvent ( 3f );
                        }
                    }
                }
                break;
            default:
                break;

        }
    }
}
