using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hoitaa Aseen Vaihtamisen kyseiseen paikkaan
/// </summary>
public class WeaponPlaceHolder : MonoBehaviour
{

    public string _weaponName; //aseen nimi

    public Item _currentWeapon = null;

    #region Necessary Components
    public BoxCollider2D _weaponCol;
    public SpriteRenderer _weaponSprite;  //Aseen grafiikka
    //for new equip
    public RolledLoot equippedWeapon;
    #endregion

    #region MeleeEffects
    public GameObject bloodSplash;
    #endregion

    #region Weapon Stats
    public float _weaponDamage; //Aseen vahinko
    public float _weaponSpeed; //Aseen nopeus
    public float nextHit;
    #endregion

    #region Bools
    bool hasHit;
    bool hitcd;
    public bool weaponSwing;
    public bool isEquipped = false;
    #endregion

    #region Enum Types
    public WeaponType weaponType;
    public DamageType damageType;
    public ProjectileType projectileType;
    #endregion

    private void Start ( )
    {
        _weaponSprite = gameObject.GetComponent<SpriteRenderer> ( );
        _weaponCol = gameObject.GetComponent<BoxCollider2D> ( );
    }

    private void Update ( )
    {
        if ( _weaponSprite.sprite != null )
        {
            SetColliderBounds ( );
        }
    }

    public RolledLoot EquipWeapon ( RolledLoot newWeapon )
    {
        RolledLoot tempWeapon = equippedWeapon;
        Debug.Log ( "Equipped " + newWeapon.itemName + "!" );
        _weaponSprite.sprite = newWeapon.equipmentSprites [ 0 ];
        equippedWeapon = newWeapon;
        isEquipped = true;

        SetColliderBounds ( );

        if ( tempWeapon != null )
        {
            return tempWeapon;
        }

        return null;

    }

    public void SetColliderBounds ( )
    {

        if ( weaponType == WeaponType.Shield )
        {
            _weaponCol.offset = new Vector2 ( 0, 0 );
        }
        else
        {
            _weaponCol.offset = new Vector2 ( 0, 0.2f );
        }
        _weaponCol.size = new Vector2 ( _weaponSprite.sprite.bounds.size.x, _weaponSprite.sprite.bounds.size.y );

    }

    public void UnEquipWeapon ( )
    {
        _weaponSprite.sprite = null;
        _weaponDamage = 0;
        _weaponSpeed = 0;
        _weaponName = "hand";
        _currentWeapon = null;
        isEquipped = false;
    }

    public Item NewWeapon ( Item newWeapon )
    {

        Item oldWeapon = _currentWeapon;

        _weaponSprite.sprite = newWeapon.itemSprite;
        _weaponName = newWeapon.itemName;
        _weaponDamage = newWeapon.weaponDamage;
        _weaponSpeed = newWeapon.weaponSpeed;
        _currentWeapon = newWeapon;
        isEquipped = true;
        _weaponCol.size = new Vector2 ( _weaponSprite.sprite.bounds.size.x, _weaponSprite.sprite.bounds.size.y );
        return oldWeapon;
    }

    public Item EmptyHand ( )
    {
        //Käsi ei ole tyhjä palauta nykyinen ase
        if ( isEquipped == false )
        {
            Item returnWeapon = _currentWeapon;
            _weaponSprite = null;
            _weaponDamage = 0;
            _weaponSpeed = 0;
            _weaponName = "hand";
            _currentWeapon = null;


            return returnWeapon;
        }

        //Käsi on tyhjä
        return null;

    }

    public void UseWeapon ( )
    {
        
        weaponSwing = true;
        _weaponCol.enabled = true;
    }
    public void HaltWeapon ( )
    {

        weaponSwing = false;
        _weaponCol.enabled = false;
    }

    private void OnTriggerStay2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Enemy" ) )
        {
            if ( weaponType != WeaponType.Shield )
            {
                if ( weaponSwing && Time.time > nextHit )
                {
                    nextHit = Time.time + PlayerClass.instance.baseAttackSpeed.Value;
                    Destroy ( Instantiate ( bloodSplash, collision.gameObject.GetComponent<Collider2D> ( ).bounds.ClosestPoint ( transform.position ), Quaternion.identity ), 2f );
                    ReferenceHolder.instance.player.DealDamage ( collision.gameObject, _weaponDamage, damageType );
                }
            }
        }
    }
}
