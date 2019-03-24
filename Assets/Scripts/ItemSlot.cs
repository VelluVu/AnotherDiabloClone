using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Slotti Johon voi asettaa itemin tällä hetkellä vain aseen
/// </summary>
public class ItemSlot : MonoBehaviour
{

    public Item _slotItem;
    public WeaponPlaceHolder [ ] hand;
    public Image _slotPicture;
    public Sprite _defaultPicture;
    bool isFull = false;

    private void Start ( )
    {
        _slotPicture = gameObject.GetComponent<Image> ( );
    }

    //Aseta slottiin parametrinä esine
    public void FillSlot ( Item item )
    {
        if ( isFull == true )
        {
            if ( FindObjectOfType<Inventory> ( ).AddItemToInventory ( item ) )
            {
                Debug.Log ( "Ylimääräinen Ase meni toiseen invislottiin, mikäli vapaita löytyy" );
            }
        }
        else
        {
            isFull = true;
            _slotItem = item;
            _slotPicture.sprite = item.itemInventoryImage;
        }
    }

    //Ota slotista ase clikkaamalla sitä
    public void UseItem ( )
    {
        //jos slotti ei ole tyhjä
        if ( _slotItem != null )
        {
            //Jos ase on oikeankäden ase asetetaan ase siihen
            if ( _slotItem.isRightHand && !_slotItem.isLeftHand )
            {
                Item tempWep;
                //tempWep = hand [ 0 ].EmptyHand ( ); //Laitetaan vanha ase talteen        
                tempWep = hand [ 0 ].NewWeapon ( _slotItem );
                EmptySlot ( );
                if ( tempWep != null )
                    FillSlot ( tempWep );
            }
            else if ( _slotItem.isLeftHand && !_slotItem.isRightHand ) //vasen
            {
                Item tempWep;
                tempWep = hand [ 1 ].NewWeapon ( _slotItem );
                EmptySlot ( );
                if ( tempWep != null )
                    FillSlot ( tempWep );
            }
            else if ( _slotItem.isDualWield ) //Asettaa molempiin käsiin
            {
                Item tempWep;
                Item copyToOffHand = _slotItem;

                tempWep = hand [ 0 ].NewWeapon ( _slotItem );  //otetaan käytössä ollut ase talteen
                EmptySlot ( ); //Tyhjätään inventory paikka
                if ( tempWep != null )
                    FillSlot ( tempWep ); //asetetaan se slottiin

                tempWep = hand [ 1 ].NewWeapon ( copyToOffHand );

                if ( tempWep != null )
                    if ( FindObjectOfType<Inventory> ( ).AddItemToInventory ( tempWep ) )
                    {
                        Debug.Log ( "Ylimääräinen Ase meni toiseen invislottiin" );
                    }

            }
            else if ( _slotItem.isBothHand ) //Kahden käden ase vie ensimmäisen paikan toinen täytyy olla tyhjä
            {
                Item tempWep = null;
                tempWep = hand [ 0 ].NewWeapon ( _slotItem );
                EmptySlot ( );
                if ( tempWep != null )
                    FillSlot ( tempWep );

                tempWep = hand [ 1 ].EmptyHand ( );
                hand [ 1 ].isEguipped = true;
                if ( tempWep != null )
                    if ( FindObjectOfType<Inventory> ( ).AddItemToInventory ( tempWep ) )
                    {
                        Debug.Log ( "Ylimääräinen Ase meni toiseen invislottiin" );
                    }

            }
            else
            {

                _slotPicture.sprite = _defaultPicture;
                isFull = false;
            }
        }

        //muuten älä tee mittää
    }

    //Tyhjää slotti eli heitä kaivoon
    public void EmptySlot ( )
    {
        _slotItem = null;
        _slotPicture.sprite = _defaultPicture;
        isFull = false;
    }

    //Tsekkaa onko tällä hetkellä tyhjä
    public bool IsEmpty ( )
    {
        if ( !isFull )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
