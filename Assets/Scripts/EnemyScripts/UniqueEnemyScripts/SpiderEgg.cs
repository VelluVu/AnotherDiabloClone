using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEgg : MonoBehaviour
{

    GameObject parent;
    public float health;
    public List<GameObject> spiderlings = new List<GameObject> ( );
    public float hatchTime;

    public delegate void HatchDelegate( GameObject parent );
    public static event HatchDelegate hatchEvent;

    public delegate IEnumerator EggFlashDelegate ( GameObject source, float time, Color color, bool isFlashSpam );
    public static event EggFlashDelegate eggFlashEvent;
    
    private void Start ( )
    {
        StartCoroutine ( HatchCoroutine ( hatchTime ) );
    }

    private void OnEnable ( )
    {
        Player.playerDealDamageEvent += TakeDamage;
    }

    private void OnDisable ( )
    {
        Player.playerDealDamageEvent -= TakeDamage;
    }

    public void SetParent( GameObject parent)
    {
        this.parent = parent;
    }

    public void Hatch()
    {

        int spawns = Random.Range ( 1, spiderlings.Count );

        for ( int i = 0 ; i < spawns ; i++ )
        {

            Vector2 randomX = new Vector2 ( transform.position.x + Random.Range ( -0.2f, 0.2f ), transform.position.y);
            GameObject spawn = Instantiate ( spiderlings[ i ], randomX, Quaternion.identity ) as GameObject;
            
        }
        if( hatchEvent != null)
        {
            hatchEvent ( parent );
        }

        gameObject.GetComponent<Collider2D> ( ).enabled = false;
        gameObject.GetComponent<SpriteRenderer> ( ).enabled = false;
        Destroy ( gameObject ,3f );
    }

    public void TakeDamage(GameObject target, float dmg)
    {

        if ( target == gameObject )
        {
            health -= dmg;

            if(eggFlashEvent != null)
            {
                StartCoroutine(eggFlashEvent (gameObject, 0.1f, Color.red, false ));
            }

            if ( health <= 0 )
            {
                if ( hatchEvent != null )
                {
                    hatchEvent ( parent );
                }
                StopAllCoroutines ( );
                Destroy ( gameObject );
            }
        }
    }

    IEnumerator HatchCoroutine(float hatchTime)
    {
        yield return new WaitForSeconds ( hatchTime );
        Hatch ( );
    }
}
