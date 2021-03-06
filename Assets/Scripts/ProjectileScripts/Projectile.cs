﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Projectile : MonoBehaviour
{

    public ProjectileType _projectiletype;
    public DamageType _damageType;

    public Rigidbody2D rb;
    public LayerMask playerLayer; //näit ei välttämättä tarvi ellei haluu jotain bounce juttuja ehkä?
    public LayerMask enemyLayer;
    public GameObject web;

    public delegate void ProjectileHitDelegate ( GameObject target, float damage, DamageType damageType, int level );
    public static event ProjectileHitDelegate projectileHitEvent;

    Vector2 _velocity;
    public float _damage;
    bool collided;
    public bool isPlayersProjectile;
    string targetTag;
    [HideInInspector]
    public int enemyLevel;

    //public float _speed;

    private void Awake ( )
    {
        rb = gameObject.GetComponent<Rigidbody2D> ( );

        if ( gameObject.CompareTag ( "Enemy" ) )
        {
            isPlayersProjectile = false;
        }
        if ( gameObject.CompareTag ( "Player" ) )
        {
            isPlayersProjectile = true;
        }
        if ( isPlayersProjectile )
        {
            targetTag = "Enemy";
        }
        if ( !isPlayersProjectile )
        {
            targetTag = "Player";
        }
    }

    private void Start ( )
    {

        Destroy ( gameObject, 10f );
    }

    private void Update ( )
    {
        if ( _projectiletype == ProjectileType.Arrow )
        {
            transform.LookAt ( Vector3.forward, Vector3.Cross ( Vector3.forward, rb.velocity ) );
        }
    }

    public void LaunchProjectile ( float damage, Vector2 velocity, DamageType damageType, ProjectileType projectileType )
    {

        _projectiletype = projectileType;
        _damageType = damageType;
        _damage = damage;
        _velocity = velocity;

        switch ( _projectiletype )
        {
            case ProjectileType.Arrow:

                ArrowProjectile ( );

                break;
            case ProjectileType.MagicMissile:

                MagicProjectile ( );

                break;
            case ProjectileType.Rock:

                ArrowProjectile ( );
                break;

            case ProjectileType.Spiderweb:

                ArrowProjectile ( );

                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Nuolifysiikkaa??
    /// </summary>
    public void ArrowProjectile ( )
    {
        rb.velocity = _velocity;
    }

    /// <summary>
    /// MagicFysiikkaa Erinlaisia taikaProjectileja?
    /// </summary>
    public void MagicProjectile ( )
    {
        rb.velocity = _velocity;
    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag ( targetTag ) && !collided )
        {
            collided = true;
            if ( _projectiletype == ProjectileType.Arrow )
            {
                //arrow impact stuff...
                rb.isKinematic = true;
                rb.simulated = false;
                transform.SetParent ( collision.contacts [ 0 ].rigidbody.transform );


            }
            else if ( _projectiletype == ProjectileType.MagicMissile )
            {
                //Magic effect
                //collision.rigidbody.AddForce (  , ForceMode2D.Impulse );

                Destroy ( gameObject );
            }
            else if ( _projectiletype == ProjectileType.Rock )
            {
                //Kivi osuupelaajaan ei tartu pelaajaan

            }
            else if(_projectiletype == ProjectileType.Spiderweb)
            {
                rb.isKinematic = true;
                rb.simulated = false;
                transform.SetParent ( collision.contacts [ 0 ].rigidbody.transform );
                Destroy ( Instantiate ( web, collision.GetContact ( 0 ).point, Quaternion.identity ), 2.5f );
            }

            if ( projectileHitEvent != null )
            {
                projectileHitEvent ( collision.gameObject, _damage, _damageType, enemyLevel );
            }

        }
        else
        {
            collided = true;
            if ( _projectiletype == ProjectileType.Arrow )
            {
                //disable colliders and simulation
                rb.simulated = false;
                rb.isKinematic = true;
                gameObject.GetComponent<Collider2D> ( ).enabled = false;
            }
            else if ( _projectiletype == ProjectileType.Rock )
            {
                Destroy ( gameObject, 2f );
            }
            else if(_projectiletype == ProjectileType.Spiderweb)
            {
                //Instansioi hidastava alue
                Destroy(Instantiate ( web, collision.GetContact(0).point, Quaternion.identity ), 2.5f);
                Destroy ( gameObject, 1f );
            }
            else
            {
                Destroy ( gameObject );
            }

        }
    }



}
