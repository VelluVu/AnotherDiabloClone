using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tarkoitus ois jos keretään , niin laittaa peliin ääniä ja tämä class hoitais ne.
/// </summary>
public class SoundPlayer : MonoBehaviour
{
    [Header ( "PlayerSounds" )]
    public AudioClip [ ] swordHitSounds;
    public AudioClip [ ] weaponSwingSounds;
    public AudioClip [ ] blockSounds;
    public AudioClip [ ] jumpSounds;
    public AudioClip [ ] dashSounds;
    public AudioClip [ ] takeDamageSounds;
    public AudioClip [ ] deathSounds;
    public AudioClip [ ] walkSounds;
    public AudioClip [ ] restorationSounds;
    public AudioClip [ ] levelUpSounds;

    [Header ( "EnemySounds" )]
    public AudioClip [ ] enemyAttackSounds;
    public AudioClip [ ] enemyPainSounds;
    public AudioClip [ ] enemyDeathSounds;
    public AudioClip [ ] enemyInsectSounds;
    public AudioClip [ ] enemyShootSounds;
    public AudioClip [ ] enemyEggCrackSounds;

    [Header ( "ObjectSounds" )]
    public AudioClip [ ] leverSounds;
    public AudioClip [ ] triggerSounds;
    public AudioClip [ ] doorSounds;
    public AudioClip [ ] checkPointSounds;
    public AudioClip treasureChestSound;

    [Header ( "AbilitySounds" )]
    public AudioClip [ ] BouncingFrostBallsSounds;
    public AudioClip [ ] DaggerRainSounds;
    public AudioClip [ ] ShoutSounds;
    public AudioClip [ ] EyeOfWeaknessSounds;
    public AudioClip [ ] FiveEyesSounds;
    public AudioClip [ ] HammerTimeSounds;
    public AudioClip [ ] LeapSounds;
    public AudioClip [ ] LifeStealDaggerSounds;
    public AudioClip [ ] PoisonBottleSounds;
    public AudioClip [ ] RenewSounds;
    public AudioClip [ ] ShadowOfManaSpritSounds;
    public AudioClip [ ] ShieldOfGhostSounds;
    public AudioClip [ ] ShoutScriptSounds;
    public AudioClip [ ] SmokeCloudSounds;
    public AudioClip [ ] ThorHammerSounds;
    public AudioClip [ ] ThrowingAxeSounds;
    public AudioClip [ ] VoidShieldSounds;
    public AudioClip [ ] FearSounds;
    public AudioClip [ ] IronManSounds;
    public AudioClip [ ] PowerOfBullSounds;
    public AudioClip [ ] PowerOFGodsSounds;
    public AudioClip [ ] PowerOfWolfSounds;
    public AudioClip [ ] RageSounds;
    public AudioClip [ ] SecondChanceSounds;
    public AudioClip [ ] ShadowStepSounds;
    public AudioClip [ ] WitheredPainSounds;
    public AudioClip [ ] TompSounds;
    public AudioClip [ ] HealingShoutSounds;


    private void OnEnable ( )
    {
        StateController.EnemyPlaySoundEvent += EnemyPlaySound;
        EnemyWeaponHolder.EnemyShootSoundEvent += EnemyPlaySound;

        Player.PlayerSoundEvent += PlayerPlaySound;

        TreasureChest.TreasureSoundEvent += ObjectPlaySound;
        TrapDoor.TrapDoorSoundEvent += ObjectPlaySound;
        DoorLeverScript.DoorLeverSoundEvent += ObjectPlaySound;
        ElevatorLever.ElevatorLeverSoundEvent += ObjectPlaySound;
        DoorScript.DoorSoundEvent += ObjectPlaySound;
        Elevator.ElevatorSoundEvent += ObjectPlaySound;

        PrefabAbilityTrigger.AbilitySoundEvent += SkillPlaySound;
        ShoutActivedTrigger.AbilitySoundEventB += SkillPlaySound;
        ProjectileShootTrigger.AbilitySoundEventA += SkillPlaySound;

    }
    private void OnDisable ( )
    {
        StateController.EnemyPlaySoundEvent -= EnemyPlaySound;
        EnemyWeaponHolder.EnemyShootSoundEvent -= EnemyPlaySound;

        Player.PlayerSoundEvent -= PlayerPlaySound;

        TreasureChest.TreasureSoundEvent -= ObjectPlaySound;
        TrapDoor.TrapDoorSoundEvent -= ObjectPlaySound;
        DoorLeverScript.DoorLeverSoundEvent -= ObjectPlaySound;
        ElevatorLever.ElevatorLeverSoundEvent -= ObjectPlaySound;
        DoorScript.DoorSoundEvent -= ObjectPlaySound;
        Elevator.ElevatorSoundEvent -= ObjectPlaySound;

        PrefabAbilityTrigger.AbilitySoundEvent -= SkillPlaySound;
        ShoutActivedTrigger.AbilitySoundEventB -= SkillPlaySound;
        ProjectileShootTrigger.AbilitySoundEventA -= SkillPlaySound;

    }

    public void EnemyPlaySound ( AudioSource source, EnemySoundType soundToPlay, EnemyTypeForSound enemyType )
    {

        switch ( soundToPlay )
        {
            case EnemySoundType.EnemyPain:
                if ( enemyType == EnemyTypeForSound.Humanoid )
                {
                    source.PlayOneShot ( enemyPainSounds [ Random.Range ( 0, enemyPainSounds.Length ) ] );
                }
                else if ( enemyType == EnemyTypeForSound.Insect )
                {
                    source.PlayOneShot ( enemyInsectSounds [ Random.Range ( 0, enemyInsectSounds.Length ) ] );
                }
                break;
            case EnemySoundType.EnemyDeath:
                if ( enemyType == EnemyTypeForSound.Humanoid )
                {
                    source.PlayOneShot ( enemyDeathSounds [ Random.Range ( 0, enemyDeathSounds.Length ) ] );
                }
                else if ( enemyType == EnemyTypeForSound.Insect )
                {
                    source.PlayOneShot ( enemyInsectSounds [ Random.Range ( 0, enemyInsectSounds.Length ) ] );
                }
                break;
            case EnemySoundType.EnemyAttack:
                if ( enemyType == EnemyTypeForSound.Humanoid )
                {
                    source.PlayOneShot ( enemyAttackSounds [ Random.Range ( 0, enemyAttackSounds.Length ) ] );
                }
                else if ( enemyType == EnemyTypeForSound.Insect )
                {
                    source.PlayOneShot ( enemyInsectSounds [ Random.Range ( 0, enemyInsectSounds.Length ) ] );
                }
                break;
            case EnemySoundType.EnemyThrowRock:
                if ( enemyType == EnemyTypeForSound.Humanoid )
                {
                    source.PlayOneShot ( enemyShootSounds [ 0 ] );
                }
                else if ( enemyType == EnemyTypeForSound.Insect )
                {
                    source.PlayOneShot ( enemyInsectSounds [ Random.Range ( 0, enemyInsectSounds.Length ) ] );
                }
                break;
            case EnemySoundType.EggCrack:
                if ( enemyType == EnemyTypeForSound.Insect )
                {
                    source.PlayOneShot ( enemyEggCrackSounds [ Random.Range ( 0, enemyEggCrackSounds.Length ) ] );
                }
                break;
            default:
                break;
        }
    }

    public void PlayerPlaySound ( AudioSource source, PlayerSoundType playerSoundType )
    {
        switch ( playerSoundType )
        {
            case PlayerSoundType.PlayerAttack:

                source.PlayOneShot ( weaponSwingSounds [ Random.Range ( 0, weaponSwingSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerHit:

                source.PlayOneShot ( swordHitSounds [ Random.Range ( 0, swordHitSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerBlock:

                source.PlayOneShot ( blockSounds [ Random.Range ( 0, blockSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerDeath:

                source.PlayOneShot ( deathSounds [ Random.Range ( 0, deathSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerTakeDamage:

                source.PlayOneShot ( takeDamageSounds [ Random.Range ( 0, takeDamageSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerJump:

                source.PlayOneShot ( jumpSounds [ Random.Range ( 0, jumpSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerDash:

                source.PlayOneShot ( dashSounds [ Random.Range ( 0, dashSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerRestoreHealth:

                source.PlayOneShot ( restorationSounds [ Random.Range ( 0, restorationSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerRestoreMana:

                source.PlayOneShot ( restorationSounds[ Random.Range ( 0, restorationSounds.Length ) ] );
                break;

            case PlayerSoundType.PlayerFootSteps:
               
                 source.clip = walkSounds [ Random.Range ( 0, walkSounds.Length ) ];                 
                        
                break;

            case PlayerSoundType.PlayerLevelUp:

                source.clip = levelUpSounds [ Random.Range ( 0, levelUpSounds.Length ) ];

                break;

            case PlayerSoundType.PlayerFallToDeath:

                source.PlayOneShot ( deathSounds [ Random.Range ( 0, deathSounds.Length ) ] );
                break;

            default:
                break;
        }
    }

    public void ObjectPlaySound ( AudioSource source, ObjectSoundType objSound )
    {
        switch ( objSound )
        {
            case ObjectSoundType.Lever:
                source.PlayOneShot ( leverSounds [ Random.Range ( 0, leverSounds.Length ) ] );
                break;
            case ObjectSoundType.Door:
                source.PlayOneShot ( treasureChestSound );
                break;
            case ObjectSoundType.Elevator:
                source.PlayOneShot ( triggerSounds [ Random.Range ( 0, triggerSounds.Length ) ] );
                break;
            case ObjectSoundType.TreasureChest:
                source.PlayOneShot ( treasureChestSound );
                break;
            case ObjectSoundType.Trigger:
                source.PlayOneShot ( triggerSounds [ Random.Range ( 0, triggerSounds.Length ) ] );
                break;
            case ObjectSoundType.TrapDoor:
                source.PlayOneShot ( triggerSounds [ Random.Range ( 0, triggerSounds.Length ) ] );
                break;
            default:
                break;
        }
    }


    public void SkillPlaySound ( AudioSource source, SkillSoundType skillSound )
    {
        switch (skillSound)
        {
            case SkillSoundType.BouncingFrostBalls:
                source.PlayOneShot(BouncingFrostBallsSounds[Random.Range(0, BouncingFrostBallsSounds.Length)]);
                break;
            case SkillSoundType.DaggerRain:
                break;
            case SkillSoundType.EyeOfWeakness:
                source.PlayOneShot(EyeOfWeaknessSounds[Random.Range(0, EyeOfWeaknessSounds.Length)]);
                break;
            case SkillSoundType.Fear:
                source.PlayOneShot(FearSounds[Random.Range(0, FearSounds.Length)]);
                break;
            case SkillSoundType.FiveEyes:
                source.PlayOneShot(FiveEyesSounds[Random.Range(0, FiveEyesSounds.Length)]);
                break;
            case SkillSoundType.HammerTime:
                source.PlayOneShot(HammerTimeSounds[Random.Range(0, HammerTimeSounds.Length)]);
                break;
            case SkillSoundType.Leap:
                source.PlayOneShot(LeapSounds[Random.Range(0, LeapSounds.Length)]);
                break;
            case SkillSoundType.LifeStealDagger:
                source.PlayOneShot(LifeStealDaggerSounds[Random.Range(0, LifeStealDaggerSounds.Length)]);
                break;
            case SkillSoundType.PoisonBottle:
                source.PlayOneShot(PoisonBottleSounds[Random.Range(0, PoisonBottleSounds.Length)]);
                break;
            case SkillSoundType.Renew:
                source.PlayOneShot(RenewSounds[Random.Range(0, RenewSounds.Length)]);
                break;
            case SkillSoundType.ShadowOfManaSprit:
                source.PlayOneShot(ShadowOfManaSpritSounds[Random.Range(0, ShadowOfManaSpritSounds.Length)]);
                break;
            case SkillSoundType.ShieldOfGhost:
                source.PlayOneShot(ShieldOfGhostSounds[Random.Range(0, ShieldOfGhostSounds.Length)]);
                break;
            case SkillSoundType.ThorHammer:
                source.PlayOneShot(ThorHammerSounds[Random.Range(0, ThorHammerSounds.Length)]);
                break;
            case SkillSoundType.ThrowingAxe:
                source.PlayOneShot(ThrowingAxeSounds[Random.Range(0, ThrowingAxeSounds.Length)]);
                break;
            case SkillSoundType.VoidShield:
                source.PlayOneShot(VoidShieldSounds[Random.Range(0, VoidShieldSounds.Length)]);
                break;
            case SkillSoundType.HealingShout:
                source.PlayOneShot(HealingShoutSounds[Random.Range(0, HealingShoutSounds.Length)]);
                break;
            case SkillSoundType.IronMan:
                source.PlayOneShot(IronManSounds[Random.Range(0, IronManSounds.Length)]);
                break;
            case SkillSoundType.PowerOfBull:
                source.PlayOneShot(PowerOfBullSounds[Random.Range(0, PowerOfBullSounds.Length)]);
                break;
            case SkillSoundType.PowerOfGods:
                source.PlayOneShot(PowerOFGodsSounds[Random.Range(0, PowerOFGodsSounds.Length)]);
                break;
            case SkillSoundType.PowerOFWolf:
                source.PlayOneShot(PowerOfWolfSounds[Random.Range(0, PowerOfWolfSounds.Length)]);
                break;
            case SkillSoundType.Rage:
                source.PlayOneShot(RageSounds[Random.Range(0, RageSounds.Length)]);
                break;
            case SkillSoundType.SecondChance:
                source.PlayOneShot(SecondChanceSounds[Random.Range(0, SecondChanceSounds.Length)]);
                break;
            case SkillSoundType.ShadowStep:
                source.PlayOneShot(ShadowStepSounds[Random.Range(0, ShadowStepSounds.Length)]);
                break;
            case SkillSoundType.Tomp:
                source.PlayOneShot(TompSounds[Random.Range(0, TompSounds.Length)]);
                break;
            case SkillSoundType.WitheredPain:
                source.PlayOneShot(WitheredPainSounds[Random.Range(0, WitheredPainSounds.Length)]);
                break;
            default:
                break;
        }
    }

}
