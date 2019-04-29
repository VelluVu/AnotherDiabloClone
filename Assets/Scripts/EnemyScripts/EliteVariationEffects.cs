using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteVariationEffects : MonoBehaviour
{
    EnemyStats enemyStats;
    public GameObject iceAuraEffect;
    public GameObject manaLeechAuraEffect;
    public GameObject adaptingEffect;
    public GameObject suiciderEffect;
    public GameObject magicImmuneEffect;
    bool auraOn;
    bool aEffectOn;
    bool sEffectOn;
    bool mEffectOn;
    bool manaLeech;
    bool adapting;
    int rng;

    [Tooltip ( "Aura Radius" )] public float auraRadius;
    [Range ( 0.1f, 0.9f )] [Tooltip ( "Less is more" )] public float iceAuraSlowEffect;
    public float manaLeechAmount;
    public LayerMask playerLayer;

    private void Start ( )
    {
        enemyStats = GetComponent<EnemyStats> ( );
        rng = Random.Range ( 0, 4 );
    }

    private void Update ( )
    {
        Variations ( );
    }

    private void OnEnable ( )
    {
        StateController.enemyDealDamageEvent += Leech;
        StateController.enemyTakeDamageEvent += AdaptingToDamageType;
    }

    private void OnDisable ( )
    {
        StateController.enemyDealDamageEvent -= Leech;
        StateController.enemyTakeDamageEvent -= AdaptingToDamageType;
    }

    void Variations ( )
    {

        if ( enemyStats.enemyVariations [ rng ] == EnemyVariations.ManaLeech )
        {
            Debug.Log ( "MANA LEECH" );
            ManaLeech ( );
        }
        if ( enemyStats.enemyVariations [ rng ] == EnemyVariations.IceAura )
        {
            Debug.Log ( "ICE AURA" );
            IceAura ( );
        }
        if ( enemyStats.enemyVariations [ rng ] == EnemyVariations.Adapting )
        {
            Debug.Log ( "ADAPTING" );
            Adapting ( );
        }
        if ( enemyStats.enemyVariations [ rng ] == EnemyVariations.MagicImmune )
        {
            Debug.Log ( "MAGIC IMMUNE" );
            MagicImmune ( );
        }
        if ( enemyStats.enemyVariations [ rng ] == EnemyVariations.Suicider )
        {
            Debug.Log ( "SUICIDER" );
            Suicider ( );
        }


    }

    /// <summary>
    /// Aura joka kuluttaa pelaajan manaa, samalla myös iskut kuluttaa manaa.
    /// </summary>
    void ManaLeech ( )
    {
        manaLeech = true;
        Collider2D col = Physics2D.OverlapCircle ( transform.position, auraRadius, playerLayer );
        if ( col != null )
        {
            col.GetComponent<Player> ( ).OnManaUse ( manaLeechAmount );
        }
    }

    /// <summary>
    /// Aura joka hidastaa pelaajaa, sekä nostaa hirviön frost resitancee
    /// </summary>
    void IceAura ( )
    {
        Collider2D col = Physics2D.OverlapCircle ( transform.position, auraRadius, playerLayer );
        enemyStats.coldResistance.BaseValue += 100f;

        if ( !auraOn )
        {
            auraOn = true;
            GameObject iceAuraEff = Instantiate ( iceAuraEffect, transform.parent );
            iceAuraEff.transform.localScale = new Vector3 ( ( transform.localScale.x - iceAuraEff.transform.localScale.x ) * 1.2f, ( transform.localScale.y - iceAuraEff.transform.localScale.y ) * 1.2f, 1 );
            iceAuraEff.transform.SetParent ( transform );
            iceAuraEff.transform.position = transform.position;
            iceAuraEff.transform.rotation = Quaternion.identity;
        }

        if ( col != null )
        {
            col.GetComponent<Player> ( ).FeelSlow ( iceAuraSlowEffect );
        }
    }

    /// <summary>
    /// Resistance Kasvaa mitä enemmän ottaa osumaa
    /// </summary>
    void Adapting ( )
    {
        adapting = true;
        if ( !aEffectOn )
        {
            aEffectOn = true;
            GameObject adaeff = Instantiate ( adaptingEffect, transform.parent );
            adaeff.transform.localScale = new Vector3 ( ( transform.localScale.x - adaeff.transform.localScale.x ), ( transform.localScale.y - adaeff.transform.localScale.y ), 1 );
            adaeff.transform.SetParent ( transform );
            adaeff.transform.position = transform.position;
            adaeff.transform.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Suuret resistancet
    /// </summary>
    void MagicImmune ( )
    {
        if ( !mEffectOn )
        {
            mEffectOn = true;
            GameObject meff = Instantiate ( magicImmuneEffect, transform.parent );
            meff.transform.localScale = new Vector3 ( ( transform.localScale.x - meff.transform.localScale.x ), ( transform.localScale.y - meff.transform.localScale.y ), 1 );
            meff.transform.SetParent ( transform );
            meff.transform.position = transform.position;
            meff.transform.rotation = Quaternion.identity;
        }
        enemyStats.coldResistance.BaseValue += 100f;
        enemyStats.fireResistance.BaseValue += 100f;
        enemyStats.lightningResistance.BaseValue += 100f;
        enemyStats.poisonResistance.BaseValue += 100f;
    }

    /// <summary>
    /// Kuollessaan instansioi pallon mikä seuraa pelaajaa tietyn ajan ja räjähtää
    /// </summary>
    void Suicider ( )
    {
        if ( !sEffectOn )
        {
            sEffectOn = true;
            GameObject seff = Instantiate ( suiciderEffect, transform.parent );
            seff.transform.localScale = new Vector3 ( ( transform.localScale.x - seff.transform.localScale.x ), ( transform.localScale.y - seff.transform.localScale.y ), 1 );
            seff.transform.SetParent ( transform );
            seff.transform.position = transform.position;
            seff.transform.rotation = Quaternion.identity;
        }
    }

    public void Leech ( GameObject target, float damage, DamageType damageType, int level )
    {
        if ( manaLeech )
        {
            if ( !auraOn )
            {
                auraOn = true;
                GameObject lechAuraEff = Instantiate ( manaLeechAuraEffect, transform.parent );
                lechAuraEff.transform.localScale = new Vector3 ( ( transform.localScale.x - lechAuraEff.transform.localScale.x ) * 1.2f, ( transform.localScale.y - lechAuraEff.transform.localScale.y ) * 1.2f, 1 );
                lechAuraEff.transform.SetParent ( transform );
                lechAuraEff.transform.position = transform.position;
                lechAuraEff.transform.rotation = Quaternion.identity;

            }
            ReferenceHolder.instance.player.OnManaUse ( damage );
            enemyStats.mana.BaseValue += damage;
        }
    }

    public void AdaptingToDamageType ( GameObject origin, float damage, DamageType damageType )
    {
        if ( adapting )
        {
            switch ( damageType )
            {
                case DamageType.Physical:
                    break;
                case DamageType.Fire:
                    enemyStats.fireResistance.BaseValue += damage;
                    break;
                case DamageType.Cold:
                    enemyStats.coldResistance.BaseValue += damage;
                    break;
                case DamageType.Poison:
                    enemyStats.poisonResistance.BaseValue += damage;
                    break;
                case DamageType.Raw:
                    break;
                case DamageType.Lightning:
                    enemyStats.lightningResistance.BaseValue += damage;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere ( transform.position, auraRadius );
    }
}
