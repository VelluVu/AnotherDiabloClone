using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classi handlaa itemslottien järjestystä ja sinne esineen asettamista
/// </summary>
public class Inventory : MonoBehaviour
{

    public List<ItemSlot> itemSlots = new List<ItemSlot> ( );

    //Aseta esine inventaarioon ensimmäiseen tyhjään paikkaan
    public bool AddItemToInventory ( Item item )
    {
        foreach ( var slot in itemSlots )
        {
            if ( slot.IsEmpty ( ) )
            {
                slot.FillSlot ( item );
                return true; //palauttaa true jos asetus onnistui
            }
        }
        //Inventaarion kaikki slotit täynnä, tehdään jotain --> palauttaa false
        return false; //false jos ei onnistu

    }

    public bool RemoveItemFromInventory ( Item item )
    {

        foreach ( var slot in itemSlots )
        {
            if ( slot.IsEmpty ( ) )
            {
                continue;
            }
            if ( slot._slotItem == item )
            {
                slot.EmptySlot ( );
                return true; //poisto onnistui
            }
        }

        return false; // poisto ei onnistunut

    }

}