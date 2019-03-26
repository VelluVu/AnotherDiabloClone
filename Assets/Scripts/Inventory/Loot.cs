using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "Loot/Loot", order = 1)]
public class Loot : ScriptableObject
{
    [Header("Item Info")]
    #region itemInfo
    public string itemName; // name of the item
    [TextArea (3,5)]
    public string description; // Description of the item
    public int itemID; // id of the item
    public Texture itemIcon; // itemin iconi
    public Sprite equippedSprite; // sprite when equipped
    public Sprite lootSprite; // sprite for the loot
    public string Rarity; // item rarity
    public int itemLevel; // the level of the item
    #endregion


    #region attributes
    public List<Attribute> attributes;
    #endregion

    #region bools
    public bool equippable;
    public bool consumable;
    public bool stackable;
    public bool mainHand;
    public bool offHand;
    #endregion

    #region weaponData
    public float damageMin; // minimi damage mitä ase tekee
    public float damageMax; // maksimi damage mitä ase tekee
    #endregion

    #region armorData
    public float armor; // armorin määrä
    public string armorSlot; // mihin slottiin tämä sopii
    #endregion

    #region consumableData
    public float potency; // consumablen vahvuus
    #endregion

    #region value
    public float buyPrice;
    public float sellPrice;
    #endregion
}
