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
    BoxCollider2D doorCollider;
    [Header ("Millä tavoin ovi aukaistaan")]
    public DoorType doorType;
    [Header ("AvainTyyppi minkä tarvii jos on avaimella avattava")]
    public KeyType keyType;  
    [Header ( "BossiLista, oviaukeaa joku näistä on null" )]
    public List<GameObject> bosses = new List<GameObject>( );
    public bool isOpen;

    private void Start ( )
    {
        doorAnimator = gameObject.GetComponent<Animator> ( );
        doorCollider = gameObject.GetComponent<BoxCollider2D> ( );

        
    }

    public void Aukene()
    {
        
        doorAnimator.SetTrigger ( "OpenDoor" );
        isOpen = true;
        doorCollider.enabled = false;
        gameObject.GetComponent<DoorScript> ( ).enabled = false;

    }

    /// <summary>
    /// voisi olla myös bool joka saa true kun tulee palautus toiselta osapuolelta
    /// </summary>
    /// <param name="key"></param>
    public void Aukene ( KeyType key )
    {
        if ( key == keyType )
        {
            isOpen = true;
            doorAnimator.SetTrigger ( "OpenDoor" );
            doorCollider.enabled = false;
            gameObject.GetComponent<DoorScript> ( ).enabled = false;
        }
    }

    private void OnTriggerStay2D ( Collider2D collision )
    {
        if (collision.gameObject.CompareTag ( "Player" ) )
        {
            if(Input.GetButton("Interaction"))
            {
                switch ( doorType )
                {
                    case DoorType.normalDoor:
                        Aukene ( );
                        break;
                    case DoorType.keyLockedDoor:
                        //Etsii pelaajan inventaariosta avaimen tai 
                        //KeyType key = collision.gameObject.GetComponent<Inventory>().GetKeyType()?
                        //Aukene(key);
                        //lähettää viestin pelaajalle, että onko avainta ja pelaaja kutsuu aukene avaimella
                        //collision.gameObject.GetComponent<Player>().SendMessage(keyType);
                        break;
                    case DoorType.leverLockedDoor:
                        //Aukeaa vivusta
                        break;
                    case DoorType.bossLockedDoor:
                        foreach ( var boss in bosses )
                        {
                            if(boss == null)
                            {
                                Aukene ( );
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }


}
