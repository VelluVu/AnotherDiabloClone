using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyWeaponHolder : MonoBehaviour
{

    public Transform rotatingPoint;
    public Transform launchPosition;
    SpriteRenderer weaponGraphic;
    BoxCollider2D weaponCollider;
    public GameObject projectile;
    bool isShootCDrdy = true;
    bool isTargetLocked;

    public WeaponType weaponType;
    public DamageType damageType;
    public ProjectileType projectileType;

    public float damage, force, speed;

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
    public void Shoot ( float damage, float speed, float range, Transform target, LayerMask playerLayer )
    {
        Vector2 velocity = AimWithRangedWeapon ( target, range, playerLayer );

        if ( isTargetLocked && isShootCDrdy )
        {

            isShootCDrdy = false;
            GameObject newProjectile = Instantiate ( projectile, launchPosition.position, launchPosition.rotation ) as GameObject;
            newProjectile.GetComponent<Projectile> ( ).LaunchProjectile ( damage, velocity, damageType, projectileType );


            StartCoroutine ( ShootCD ( speed ) );
        }
    }

    public bool GetIsShootRdy ( )
    {
        return isShootCDrdy;
    }

    /// <summary>
    /// Ampuu projectilen heittoliikkeellä, jos lopullisella vihollisella on jokin rotatepoint niin tätä pitää muokata päivittämään anglen rotatepointtiin ja projectilelle annetaan vaan lähtövoima/nopeus.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public Vector2 AimWithRangedWeapon ( Transform target, float range, LayerMask playerLayer )
    {

        RaycastHit2D hit = Physics2D.CircleCast ( rotatingPoint.position, 0.1f, target.transform.position, range, playerLayer );
        //RaycastHit2D up = Physics2D.Raycast ( rotatingPoint.position, launchPosition.up, 1f );

        //Vector3 towardsTarget = -Vector3.Cross ( Vector3.Cross ( launchPosition.position, target.position ), launchPosition.up ).normalized;
        //Vector3 upVector = Vector3.Project ( launchPosition.up, Vector3.up ).normalized;
        Vector2 distance = new Vector2 ( target.position.x - launchPosition.position.x, target.position.y - launchPosition.position.y );
        Vector3 curGravity = Physics2D.gravity;
        //Vector3 diagonal = new Vector2 ( towardsTarget.x, upVector.y );

        //vektorit visualisointi
        //Debug.DrawRay ( launchPosition.position, diagonal, Color.blue, 10f );
        //Debug.DrawRay ( launchPosition.position, towardsTarget, Color.red, 10f );
        //Debug.DrawRay ( launchPosition.position, upVector, Color.green, 10f );
        Debug.DrawRay ( launchPosition.position, distance, Color.black, 10f ); // vaaka vektori pelaajaan

        float angle = Mathf.Atan ( ( distance.y + curGravity.y * 0.5f ) / distance.x ) * -1;
        float velocity = distance.x / Mathf.Cos ( angle );
        Vector2 calculatedVelocity = new Vector2 ( Mathf.Cos ( angle ) * velocity, Mathf.Sin ( angle ) * velocity );

        Debug.Log ( "Etäisyys " + range );
        Debug.Log ( "EtäisyysLähelle " + range * 0.5f );
        Debug.Log ( "Säteen " + hit.distance );

        if ( hit != false && hit.collider.gameObject.CompareTag ( "Player" ) )
        {

            if ( hit.distance <= ( range * 0.5f ) )
            {

                Debug.Log ( "Etäisyys " + range * 0.5f );

                Debug.Log ( "kohde lähellä" );

                calculatedVelocity = new Vector2 ( Mathf.Cos ( angle * 0.3f ) * velocity, Mathf.Sin ( angle * 0.3f ) * velocity );

                isTargetLocked = true;

                return calculatedVelocity;
            }
            else
            {

                Debug.Log ( "Etäisyys " + range );

                Debug.Log ( "kohde kaukana" );

                isTargetLocked = true;

                return calculatedVelocity;
            }

        }

        return calculatedVelocity;
    }

    IEnumerator ShootCD ( float speed )
    {
        yield return new WaitForSeconds ( speed );
        isShootCDrdy = true;
    }
}
