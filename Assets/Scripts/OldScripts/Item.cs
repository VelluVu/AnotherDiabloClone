using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Purkaa scriptable objectin tähän class toimii ns. loottina
/// Jos Lisätään tarvaraa Scriptable objectiin tämäkin class kasvaa.
/// </summary>
public class Item : MonoBehaviour
{

    #region ItemInfo
    public string itemName;
    public Sprite itemInventoryImage;
    public Sprite itemSprite;
    #endregion

    #region WeaponStats
    public float weaponDamage;
    public float weaponSpeed;
    #endregion

    #region ArmorStats
    public float armorRating;
    #endregion

    #region ConsumableStats
    public float healthGain;
    #endregion

    #region booleans 
    public bool isArmor;
    public bool isConsumable;
    public bool isRightHand;
    public bool isLeftHand;
    public bool isDualWield;
    public bool isBothHand;
    public bool pickedUp = false;
    #endregion

    SpriteRenderer mySpriteRenderer;
    BoxCollider2D col;

    public ItemObject _pickUpItem;

    private void Awake ( )
    {
        if ( !gameObject.activeSelf || pickedUp == true )
        {
            DontDestroyOnLoad ( this );
        }
    }

    private void Start ( )
    {
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer> ( );
        mySpriteRenderer.sprite = _pickUpItem.itemSprite;
        col = gameObject.AddComponent<BoxCollider2D> ( );
        col.isTrigger = true;
        col.size = new Vector2(mySpriteRenderer.sprite.bounds.size.x, mySpriteRenderer.sprite.bounds.size.y); 
        IdentifyItem ( _pickUpItem );
    }

    public Item GetItem ( )
    {
        if ( this != null)
        {
            pickedUp = true;
            return this;
        }
        return null;

    }

    //Jos lisätään statteja Scriptable objectiin niin tämäkin täytyy päivittää
    public void IdentifyItem ( ItemObject inputItem )
    {

        this.weaponDamage = inputItem.itemDamage;
        this.weaponSpeed = inputItem.itemSpeed;
        this.armorRating = inputItem.armorRating;
        this.healthGain = inputItem.healthGain;
        this.isArmor = inputItem.isArmor;
        this.isConsumable = inputItem.isConsumable;
        this.isRightHand = inputItem.isRightHand;
        this.isLeftHand = inputItem.isLeftHand;
        this.isDualWield = inputItem.isDualWield;
        this.isBothHand = inputItem.isBothHand;
        this.itemName = inputItem.itemName;
        this.itemInventoryImage = inputItem.itemInventoryImage;
        this.itemSprite = inputItem.itemSprite;

    }
}