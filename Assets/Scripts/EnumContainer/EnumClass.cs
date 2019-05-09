
using UnityEngine;


public enum ProjectileType
{
    Arrow,
    MagicMissile,
    Rock,
    ThrowingAxe,
    Spiderweb,
}

public enum WeaponType
{
    MeleeWeapon,
    RangedWeapon,
    TwoHandedRangedWeapon,
    TwoHandedMeleeWeapon,
    Shield
}

public enum DamageType
{
    Physical,
    Fire,
    Cold,
    Poison,
    Raw,
    Lightning,
}

public enum EnemyType
{
    GroundEnemy,
    FlyingEnemy,
    EliteEnemy,
    Boss
}

public enum EnemyTypeForSound
{
    Humanoid,
    Insect,
}

//Vihollislistasta poimitut toteutetaan ne mitä kerkiää

public enum GroundEnemyType
{
    Skeleton,
    DemonicWolf,
    Ogre,
    Rat,
    ShieldWielder,
    Splitter,
    Spider,
    SpiderLing,
    FireMage,
    IceMage,
    HealerMage,
    Archer,
    Knight,
    ArmoredRat,
    Ghoul,
    PoisonToad,
    ParkRanger,
    egg,
    none
}

public enum FlyingEnemyType
{
    Bat,
    DemonicBird,
    none
}

public enum BossEnemyType
{
    SpiderQueen,
    FireLord,
    SummonerKing,
    Medusa,
    none
}

public enum EnemyVariations
{
    ManaLeech,
    IceAura,
    Adapting,
    MagicImmune,
    Suicider
}

public enum ClassType
{
    Warrior,
    Wizard,
    Thief
}

public enum SpellType
{
    Projectile,
    Channel,
    Ray,
    Area,
    Buff
}

public enum DoorType
{
    normalDoor,
    keyLockedDoor,
    leverLockedDoor,
    bossLockedDoor

}

public enum KeyType
{
    silver,
    gold
}

[System.Serializable]
public enum Tags
{
    HealingPotion, ManaPotion
}
[System.Serializable]
public enum Rarity
{
    Common,Uncommon,Rare,Epic,Legendary
}
[System.Serializable]
public enum ArmorSlot
{
    None,Helm,Chest,Pants,Boots,Shoulder,Bracer,Glove,Belt,Mainhand,Offhand,Ring,Necklace,Consumable
}

public enum AreaName
{
    Area1,
    Area2,
    Area3,
    Area4,
}

public enum EnemySoundType
{
    EnemyPain,
    EnemyDeath,
    EnemyAttack,
    EnemyThrowRock,
    EggCrack,
}

public enum PlayerSoundType
{
    PlayerAttack,
    PlayerHit,
    PlayerBlock,
    PlayerDeath,
    PlayerTakeDamage,
    PlayerJump,
    PlayerDash,
    PlayerRestoreHealth,
    PlayerRestoreMana,
    PlayerFootSteps,
    PlayerLevelUp,
    PlayerFallToDeath,
}

public enum ObjectSoundType
{
    Lever,
    Door,
    Elevator,
    TreasureChest,
    Trigger,
    TrapDoor,
}

public enum SkillSoundType
{
    BouncingFrostBalls,
    DaggerRain,
    EyeOfWeakness, 
    Fear,
    FiveEyes,
    HammerTime,
    HealingShout,
    IronMan,
    Leap,
    LifeStealDagger,
    PoisonBottle,
    PowerOfBull,
    PowerOfGods,
    PowerOFWolf,
    Rage,
    Renew,
    SecondChance,
    ShadowOfManaSprit,
    ShadowStep,
    ShieldOfGhost,    
    ThorHammer,
    ThrowingAxe,
    VoidShield,
    Tomp,
    WitheredPain,
    

}

[System.Serializable]
public enum AttributeTag 
{
    Resistance,primary,secondary
}

public enum ClimpObjectType
{
    Robe,
    Ladder,
}

