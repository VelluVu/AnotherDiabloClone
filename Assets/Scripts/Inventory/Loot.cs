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
    public List<Sprite> equipmentSprites;
    public Sprite lootSprite; // sprite for the loot
    [SerializeField]
    public Rarity Rarity; // item rarity
    public int itemLevel; // the level of the item
    #endregion

    #region tags
    [SerializeField]
    public List<Tags> tags;
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

   

    #region armorData
    public ArmorSlot armorSlot; // mihin slottiin tämä sopii
    #endregion

    #region consumableData
    public float consumablePotency; // consumablen vahvuus
    #endregion

    #region value
    public float buyPrice;
    public float sellPrice;
    #endregion
}

