using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hoitaa Aseen Vaihtamisen kyseiseen paikkaan
/// </summary>
public class WeaponPlaceHolder : MonoBehaviour
{

    BoxCollider2D col;
    SpriteRenderer sr;
    public Item _currentWeapon = null;
    public GameObject bloodSplash;
    public string _weaponName; //aseen nimi
    public SpriteRenderer _weaponSprite;  //Aseen grafiikka
    public float _weaponDamage; //Aseen vahinko
    public float _weaponSpeed; //Aseen nopeus
    public bool isEquipped = false;
    bool hasHit;
    public bool isShield;

    //for new equip
    public RolledLoot equippedWeapon;
    

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
        Debug.Log ( newWeapon );
        if ( newWeapon != null )
        {
            Debug.Log ( newWeapon.weaponDamage );
            Debug.Log ( newWeapon.weaponSpeed );
            Debug.Log ( newWeapon.itemSprite );
            Debug.Log ( newWeapon.itemInventoryImage );
        }

        Debug.Log ( _currentWeapon );
        if ( _currentWeapon != null )
        {
            Debug.Log ( _currentWeapon.weaponDamage );
            Debug.Log ( _currentWeapon.weaponSpeed );
            Debug.Log ( _currentWeapon.itemSprite );
            Debug.Log ( _currentWeapon.itemInventoryImage );
        }

        Item oldWeapon = _currentWeapon;

        Debug.Log ( oldWeapon );
        if ( oldWeapon != null )
        {
            Debug.Log ( oldWeapon.weaponDamage );
            Debug.Log ( oldWeapon.weaponSpeed );
            Debug.Log ( oldWeapon.itemSprite );
            Debug.Log ( oldWeapon.itemInventoryImage );
        }

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

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Enemy" ) && !hasHit && !isShield )
        {
            hasHit = true;
            Destroy ( Instantiate ( bloodSplash, collision.contacts[0].point, Quaternion.identity ), 2f );
            Debug.Log ( gameObject.name );
            gameObject.GetComponentInParent<Player> ( ).DealDamage ( collision.gameObject.GetComponent<StateController> ( ), _weaponDamage );
            StartCoroutine ( HitReset ( ) );

        }
    }

    /*private void OnTriggerEnter2D ( Collision2D collision )
    {

        if ( collision.gameObject.CompareTag ( "Enemy" ) && !hasHit && !isShield )
        {
            hasHit = true;
            Destroy(Instantiate ( bloodSplash, collision.transform) , 2f);
            Debug.Log ( gameObject.name );
            gameObject.GetComponentInParent<Player> ( ).DealDamage ( collider.GetComponent<StateController> ( ), _weaponDamage );
            StartCoroutine ( HitReset ( ) );

        }
    }*/


    IEnumerator HitReset()
    {
        yield return new WaitForSeconds ( _weaponSpeed * gameObject.GetComponentInParent<PlayerClass>().baseAttackSpeed.Value );
        hasHit = false;
    }
}
