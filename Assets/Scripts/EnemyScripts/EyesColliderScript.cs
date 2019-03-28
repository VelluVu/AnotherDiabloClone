using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Käyttää tätä hyökkäys colliderina tällä hetkellä, mutta vihollisellekkin voisi laittaa weaponplaceholderin, mutta hämähäkillä pitäisi silloin olla se kans vaikkei asetta olisikaan.
/// </summary>
public class EyesColliderScript : MonoBehaviour
{

    bool hasDealtDmg;

    private void Update ( )
    {
        if(gameObject.GetComponent<BoxCollider2D>().isActiveAndEnabled)
        {
            hasDealtDmg = false;
        }
    }

    /// <summary>
    /// Osuuko collider pelaajaan ja onko tehnyt vahinkoa vielä, jos ei niin ilmoittaa statemachinelle käyttää dealdamage functiota
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if(collision.gameObject.CompareTag("Player") && !hasDealtDmg)
        {
            Debug.Log ( gameObject.GetComponentInParent<StateController>().enemyStats.name + " Hits you" );
            hasDealtDmg = true;
            gameObject.GetComponentInParent<StateController> ( ).DealDamage ( collision.GetComponentInParent<Player>() );
            StartCoroutine ( DmgCooldown ( ) );
        }
    }

    /// <summary>
    /// Mahdollistaa uuden vahingon teon resettaamalla booleanin
    /// </summary>
    /// <returns></returns>
    IEnumerator DmgCooldown()
    {
        yield return new WaitForSeconds ( gameObject.GetComponentInParent<StateController>().enemyStats.attackSpeed );
        hasDealtDmg = false;
    }



}
