using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vihollisen AsePaikka
/// </summary>
public class EnemyWeaponHolder : MonoBehaviour
{

    #region Necessary Components
    SpriteRenderer weaponGraphic;
    BoxCollider2D weaponCollider;
    #endregion

    #region Ranged
    public GameObject projectile;
    public Transform rotatingPoint;
    public Transform launchPosition;
    #endregion

    #region Melee Effects
    public GameObject bloodSplash;
    #endregion

    #region Weapon
    public float _weaponDamage;
    #endregion

    #region Bools
    bool isShootCDrdy = true;
    bool isTargetLocked;
    bool hasHit;
    #endregion

    #region Enums Types
    public WeaponType weaponType;
    public DamageType damageType;
    public ProjectileType projectileType;
    #endregion

    //Näihin otetaan talteen itemeistä tulleet muuttujat , esim. varsijousessa force riittää ampuun kauemmas
    public float _damage, _force, _weaponSpeed;

    private void Start ( )
    {

        weaponGraphic = gameObject.GetComponent<SpriteRenderer> ( );

        if ( weaponType != WeaponType.RangedWeapon )
        {
            weaponCollider = gameObject.GetComponent<BoxCollider2D> ( );
            weaponCollider.offset = new Vector2 ( 0, -0.2f );
            weaponCollider.size = new Vector2 ( weaponGraphic.sprite.bounds.size.x, weaponGraphic.sprite.bounds.size.y );
        }
    }

    /// <summary>
    ///  Ampuu projectilen Otetaan parametrit tällähetkellä stateista
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="force"></param>
    /// <param name="speed"></param>
    public void Shoot ( float damage, float weaponSpeed, float range, Transform target, LayerMask playerLayer )
    {

        Vector2 velocity = AimWithRangedWeapon ( target, range, playerLayer );
    
        if ( isTargetLocked && isShootCDrdy )
        {

            isShootCDrdy = false;
            GameObject newProjectile = Instantiate ( projectile, launchPosition.position, launchPosition.rotation ) as GameObject;
            newProjectile.GetComponent<Projectile> ( ).LaunchProjectile ( damage, velocity, damageType, projectileType );

            StartCoroutine ( ShootCD ( weaponSpeed ) );

        }
    }

    public bool GetIsShootRdy ( )
    {
        return isShootCDrdy;
    }

    /// <summary>
    /// Ampuu projectilen heittoliikkeellä tietystä positiosta.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public Vector2 AimWithRangedWeapon ( Transform target, float range, LayerMask playerLayer )
    {

        RaycastHit2D hit = Physics2D.CircleCast ( rotatingPoint.position, 0.1f, launchPosition.transform.right, range, playerLayer );
   
        Vector2 distance = new Vector2 ( target.position.x - launchPosition.position.x, target.position.y - launchPosition.position.y );
        Vector3 curGravity = Physics2D.gravity;
     
        Debug.DrawRay ( launchPosition.position, distance, Color.black, 10f ); // vaaka vektori pelaajaan

        float angle = Mathf.Atan ( ( distance.y + curGravity.y * 0.5f ) / distance.x ) * -1;
        float velocity = distance.x / Mathf.Cos ( angle );
        Vector2 calculatedVelocity = new Vector2 ( Mathf.Cos ( angle ) * velocity, Mathf.Sin ( angle ) * velocity );

        if ( hit != false )
        {
            if ( hit.collider.gameObject.CompareTag ( "Player" ) )
            {
                if ( hit.distance <= ( range * 0.5f ) )
                {
                    //Debug.Log ( "hitDistance" + hit.distance );

                    //Debug.Log ( "Etäisyys " + range * 0.5f );

                    //Debug.Log ( "kohde lähellä" );

                    calculatedVelocity = new Vector2 ( Mathf.Cos ( angle * 0.3f ) * velocity, Mathf.Sin ( angle * 0.3f ) * velocity );

                    isTargetLocked = true;

                    return calculatedVelocity;
                }
                else
                {

                    //Debug.Log ( "Etäisyys " + range );

                    //Debug.Log ( "kohde kaukana" );

                    isTargetLocked = true;

                    return calculatedVelocity;
                }
            }
        }

        return calculatedVelocity;
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Enemy" ) )
        {
            if ( !hasHit && (weaponType != WeaponType.Shield || weaponType != WeaponType.RangedWeapon || weaponType != WeaponType.TwoHandedRangedWeapon))
            {

                hasHit = true;
                Destroy ( Instantiate ( bloodSplash, collision.gameObject.GetComponent<Collider2D> ( ).bounds.ClosestPoint ( transform.position ), Quaternion.identity ), 2f );
                Debug.Log ( gameObject.name );
                gameObject.GetComponentInParent<Player> ( ).DealDamage ( collision.gameObject, _weaponDamage );
                //col.enabled = false; // kun osuu ottaa colliderin pois ettei iske kaikkialle
                StartCoroutine ( HitReset ( ) );
            }
        }
    }

    IEnumerator HitReset ( )
    {
        yield return new WaitForSeconds ( _weaponSpeed * gameObject.GetComponentInParent<PlayerClass> ( ).baseAttackSpeed.Value );
        hasHit = false;
    }

    IEnumerator ShootCD ( float speed )
    {
        yield return new WaitForSeconds ( speed );
        isShootCDrdy = true;
    }
}
