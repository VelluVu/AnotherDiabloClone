using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "PluggableAI/Actions/Shoot" )]
public class ShootAction : Action
{
    public override void Act ( StateController controller )
    {
        //Testataan millä aseella voidaan ampua
        if ( controller.weaponLeft.weaponType == WeaponType.RangedWeapon )
        {
            ShootAtTarget ( controller, controller.weaponLeft );
        }
        else if ( controller.weaponRight.weaponType == WeaponType.RangedWeapon )
        {
            ShootAtTarget ( controller, controller.weaponRight );
        }
        else if ( controller.weaponRight.weaponType == WeaponType.TwoHandedRangedWeapon )
        {
            ShootAtTarget ( controller, controller.weaponRight );
        }
        else if ( controller.weaponLeft.weaponType == WeaponType.TwoHandedRangedWeapon )
        {
            ShootAtTarget ( controller, controller.weaponLeft );
        }
    }

    /// <summary>
    /// Tiedetään jo kohde ammutaan aseella kohdetta
    /// </summary>
    /// <param name="controller"></param>
    public void ShootAtTarget ( StateController controller, EnemyWeaponHolder weapon )
    {

        if ( controller.transform.position.x < controller.chaseTarget.transform.position.x )
        {
            controller.dirRight = true;
        }
        else
        {
            controller.dirRight = false;
        }

        if ( weapon.GetIsShootRdy ( ) )
        {
            //Tää muuttuu kun ranged weapon asetetaan vihun kouraan
            weapon.Shoot ( controller.enemyStats.attackDamage.Value, controller.enemyStats.attackSpeed.Value, controller.spotDistance, controller.chaseTarget, controller.playerLayer,controller.enemyStats.level );
        }
    }
}
