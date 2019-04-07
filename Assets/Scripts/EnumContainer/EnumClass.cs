
using UnityEngine;


public enum ProjectileType
{
    Arrow,
    MagicMissile,
    Rock
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
    Frost,
    Poison
}

public enum EnemyType
{
    GroundEnemy,
    FlyingEnemy,
    EliteEnemy,
    Boss
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
    FireMage,
    IceMage,
    HealerMage,
    Archer,
    Knight,
    ArmoredRat,
    Ghoul,
    PoisonToad,
    ParkRanger
}

public enum FlyingEnemyType
{
    Bat,
    DemonicBird
}

public enum BossEnemyType
{
    SpiderQueen,
    FireLord,
    SummonerKing,
    Medusa
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
