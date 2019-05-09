using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Käyttää tätä hyökkäys colliderina tällä hetkellä, mutta tietyille vihollisille voisi laittaa weaponplaceholder.
/// </summary>
public class EnemyAttackCollider : MonoBehaviour
{

    public GameObject bloodSplash;
    public DamageType damageType;
    float nexHit;

    private void Awake ( )
    {
        gameObject.GetComponent<Collider2D> ( ).enabled = false;
    }
    
    ///Jos hirviöllä ei ole asetta
    private void OnTriggerStay2D ( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag ( "Player" ) && Time.time > nexHit)
        {
            nexHit = Time.time + 0.4f;      
            GameObject createdObject = Instantiate(bloodSplash, collision.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position), Quaternion.identity);
            createdObject.transform.SetParent(ReferenceHolder.instance.goreHolder);
            Destroy(createdObject, 2f);
            gameObject.GetComponentInParent<StateController> ( ).DealDamage ( collision.gameObject, 0, damageType );
            
        }
    }
}
