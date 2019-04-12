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
    BoxCollider2D col;
    SpriteRenderer sr;
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
    #endregion

    #region Bools
    bool hasHit;
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
        col = gameObject.GetComponent<BoxCollider2D> ( );
        sr = gameObject.GetComponent<SpriteRenderer> ( );
 
    }

    public RolledLoot EquipWeapon(RolledLoot newWeapon)
    {
        RolledLoot tempWeapon = equippedWeapon;
        Debug.Log("Equipped " + newWeapon.itemName + "!");
        _weaponSprite.sprite = newWeapon.equipmentSprites[0];
        equippedWeapon = newWeapon;
        isEquipped = true;
        col.offset = new Vector2 ( 0, -0.2f ); //asettaa aseen collider kohilleen
        col.size = new Vector2 ( sr.sprite.bounds.size.x, sr.sprite.bounds.size.y ); //asettaa aseen collider reunat kohillee spriten reunojen perusteella

        if (tempWeapon != null)
        {
            return tempWeapon;
        }
        
        return null;
        
    }
    public void UnEquipWeapon()
    {
        _weaponSprite.sprite = null;
        _weaponDamage = 0;
        _weaponSpeed = 0;
        _weaponName = "hand";
        _currentWeapon = null;
        isEquipped = false;
    }

    //Asettaa Esineen käyttöön
    public Item NewWeapon ( Item newWeapon )
    {
        //Debug.Log ( newWeapon );
        //if ( newWeapon != null )
        //{
        //    Debug.Log ( newWeapon.weaponDamage );
        //    Debug.Log ( newWeapon.weaponSpeed );
        //    Debug.Log ( newWeapon.itemSprite );
        //    Debug.Log ( newWeapon.itemInventoryImage );
        //}

        //Debug.Log ( _currentWeapon );
        //if ( _currentWeapon != null )
        //{
        //    Debug.Log ( _currentWeapon.weaponDamage );
        //    Debug.Log ( _currentWeapon.weaponSpeed );
        //    Debug.Log ( _currentWeapon.itemSprite );
        //    Debug.Log ( _currentWeapon.itemInventoryImage );
        //}

        Item oldWeapon = _currentWeapon;

        //Debug.Log ( oldWeapon );
        //if ( oldWeapon != null )
        //{
        //    Debug.Log ( oldWeapon.weaponDamage );
        //    Debug.Log ( oldWeapon.weaponSpeed );
        //    Debug.Log ( oldWeapon.itemSprite );
        //    Debug.Log ( oldWeapon.itemInventoryImage );
        //}

        _weaponSprite.sprite = newWeapon.itemSprite;
        _weaponName = newWeapon.itemName;
        _weaponDamage = newWeapon.weaponDamage;
        _weaponSpeed = newWeapon.weaponSpeed;
        _currentWeapon = newWeapon;
        isEquipped = true;
        col.offset = new Vector2 ( 0, -0.2f );
        col.size = new Vector2 ( sr.sprite.bounds.size.x, sr.sprite.bounds.size.y);
        return oldWeapon;
    }

    //Saattaa tarvita tyhjää käsi
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

    //private void OnCollisionEnter2D ( Collision2D collision )
    //{
    //    if ( collision.gameObject.CompareTag ( "Enemy" ) )
    //    {
    //        if ( !hasHit && weaponType != WeaponType.Shield )
    //        {
    //            hasHit = true;
    //            Destroy ( Instantiate ( bloodSplash, collision.contacts [ 0 ].point, Quaternion.identity ), 2f );
    //            Debug.Log ( gameObject.name );
    //            gameObject.GetComponentInParent<Player> ( ).DealDamage ( collision.gameObject.GetComponent<StateController> ( ), _weaponDamage );
    //            col.enabled = false; // kun osuu ottaa colliderin pois
    //            StartCoroutine ( HitReset ( ) );
    //        }
    //    }
    //}

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Enemy" ) )
        {
            if ( !hasHit && weaponType != WeaponType.Shield )
            {
                
                hasHit = true;
                Destroy ( Instantiate ( bloodSplash, collision.gameObject.GetComponent<Collider2D> ( ).bounds.ClosestPoint ( transform.position ), Quaternion.identity ), 2f );              
                gameObject.GetComponentInParent<Player> ( ).DealDamage ( collision.gameObject , _weaponDamage );
                StartCoroutine ( HitReset ( ) );
            }
        }
    }

    IEnumerator HitReset ()
    {
        yield return new WaitForSeconds ( _weaponSpeed * gameObject.GetComponentInParent<PlayerClass>().baseAttackSpeed.Value );
        hasHit = false;
    }
}
