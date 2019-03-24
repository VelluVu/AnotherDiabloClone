using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Weapon Scriptable Object, jos tämä kasvaa täytyy täydentää Item class
/// </summary>
[CreateAssetMenu ( fileName = "Item", menuName = "Items/Item", order = 1 )]
public class ItemObject : ScriptableObject
{
    #region abstractData
    public string itemName = "Default Item"; //esineen nimi
    public Sprite itemInventoryImage;  //kuva mikä näkyy inventaariossa
    public Sprite itemSprite;  //esineen grafiikka
    #endregion

    #region weaponData
    public float itemDamage; //Aseen vahinko
    public float itemSpeed; //Aseen nopeus
    #endregion

    #region consumableData
    public float healthGain; //Jos esim pottu
    #endregion

    #region armorData
    public float armorRating; //Jos haarniska
    #endregion

    #region booleans 
    public bool isArmor; // Onko haarniska
    public bool isConsumable; //Onko consumable item
    public bool isRightHand; //Onko oikeanKädenAse
    public bool isLeftHand; //VasemmanKäden
    public bool isDualWield; //Onko dualwieldase
    public bool isBothHand; //Onko kahdenkäden
    #endregion

    //nämä voisi ottaa käyttöön?
    public enum itemType { Weapon, Armor, Consumable }; //enumipaikka 0 == ase, 1 == potsku tai jotain, 2 == haarniska??

}
