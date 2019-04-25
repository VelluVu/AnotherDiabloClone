using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteVariationEffects : MonoBehaviour
{
    EnemyStats enemyStats;
    bool manaLeech;
    bool adapting;

    [Tooltip ( "Aura Radius" )] public float auraRadius;
    [Range ( 0.1f, 0.9f )] [Tooltip ( "Less is more" )] public float iceAuraSlowEffect;
    public float manaLeechAmount;
    public LayerMask playerLayer;

    private void Start ( )
    {
        enemyStats = GetComponent<EnemyStats> ( );
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
        foreach ( var variation in enemyStats.enemyVariations )
        {
            if ( variation == EnemyVariations.ManaLeech )
            {
                Debug.Log ( "MANA LEECH" );
                ManaLeech ( );
            }
            if ( variation == EnemyVariations.IceAura )
            {
                Debug.Log ( "ICE AURA" );
                IceAura ( );
            }
            if ( variation == EnemyVariations.Adapting )
            {
                Debug.Log ( "ADAPTING" );
                Adapting ( );
            }
            if ( variation == EnemyVariations.MagicImmune )
            {
                Debug.Log ( "MAGIC IMMUNE" );
                MagicImmune ( );
            }
            if ( variation == EnemyVariations.Suicider )
            {
                Debug.Log ( "SUICIDER" );
                Suicider ( );
            }
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
            col.GetComponent<Player>().OnManaUse ( manaLeechAmount );
        }
    }

    /// <summary>
    /// Aura joka hidastaa pelaajaa, sekä nostaa hirviön frost resitancee
    /// </summary>
    void IceAura ( )
    {   
        Collider2D col = Physics2D.OverlapCircle ( transform.position, auraRadius, playerLayer );
        enemyStats.frostResistance.BaseValue += 100f;
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
    }

    /// <summary>
    /// Suuret resistancet
    /// </summary>
    void MagicImmune ( )
    {
        enemyStats.frostResistance.BaseValue += 100f;
        enemyStats.fireResistance.BaseValue += 100f;
        enemyStats.lightningResistance.BaseValue += 100f;
        enemyStats.poisonResistance.BaseValue += 100f;
    }

    /// <summary>
    /// Kuollessaan instansioi pallon mikä seuraa pelaajaa tietyn ajan ja räjähtää
    /// </summary>
    void Suicider ( )
    {

    }

    public void Leech ( GameObject target, float damage, DamageType damageType, int level )
    {
        if ( manaLeech )
        {
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
                    enemyStats.frostResistance.BaseValue += damage;
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
