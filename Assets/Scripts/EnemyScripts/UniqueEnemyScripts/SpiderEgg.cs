using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEgg : MonoBehaviour
{

    GameObject parent;
    public float health;
    public List<GameObject> spiderlings = new List<GameObject> ( );
    public float hatchTime;
    public GroundEnemyType imEaster;
    AudioSource source;

    public delegate void HatchDelegate( GameObject parent );
    public static event HatchDelegate hatchEvent;

    public delegate void EggDestroyedDelegate ( GameObject parent );
    public static event EggDestroyedDelegate eggDestroyedEvent;

    public delegate IEnumerator EggFlashDelegate ( GameObject source, float time, Color color, bool isFlashSpam );
    public static event EggFlashDelegate eggFlashEvent;

    public delegate  void EggSoundDelegate( AudioSource source, EnemySoundType enemySoundType, EnemyTypeForSound enemyTypeForSound );
    public static event EggSoundDelegate EggSoundEvent;

    private void Start ( )
    {
        source = gameObject.GetComponent<AudioSource> ( );
        StartCoroutine ( HatchCoroutine ( hatchTime ) );
    }

    private void OnEnable ( )
    {
        Player.playerDealDamageEvent += TakeDamage;
        CheckPoint.checkPointEvent += Die;
    }

    private void OnDisable ( )
    {
        Player.playerDealDamageEvent -= TakeDamage;
        CheckPoint.checkPointEvent -= Die;
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
            if ( spawn != null || gameObject != null )
            {
                spawn.GetComponent<StateController> ( ).SetWaypoints ( parent, parent.GetComponent<StateController> ( ).positions );
            }
        }
        if( hatchEvent != null)
        {
            hatchEvent ( parent );
        }

        if ( EggSoundEvent != null)
        {
            EggSoundEvent ( source, EnemySoundType.EggCrack, EnemyTypeForSound.Insect );
        }

        gameObject.GetComponent<Collider2D> ( ).enabled = false;
        gameObject.GetComponent<SpriteRenderer> ( ).enabled = false;
        Destroy ( gameObject ,3f );
    }

    public void TakeDamage(GameObject target, float dmg, bool critical, DamageType damageType, bool skill, float skillDmgPercent)
    {
        float skilldmg = 1;
        if ( skill )
        {
            skilldmg = skillDmgPercent * 0.01f;
        }

        if ( target == gameObject )
        {
            health -= dmg * skilldmg;

            if(eggFlashEvent != null)
            {
                StartCoroutine(eggFlashEvent (gameObject, 0.1f, Color.red, false ));
            }

            if ( health <= 0 )
            {
                Die ( );
            }
        }
    }

    public void Die()
    {
        if ( eggDestroyedEvent != null )
        {
            eggDestroyedEvent ( parent );
        }

        if ( EggSoundEvent != null )
        {
            EggSoundEvent ( source, EnemySoundType.EggCrack, EnemyTypeForSound.Insect );
        }

        StopAllCoroutines ( );

        gameObject.GetComponent<Collider2D> ( ).enabled = false;
        gameObject.GetComponent<SpriteRenderer> ( ).enabled = false;
        Destroy ( gameObject, 3f );
    }

    IEnumerator HatchCoroutine(float hatchTime)
    {
        yield return new WaitForSeconds ( hatchTime );
        Hatch ( );
    }
}
