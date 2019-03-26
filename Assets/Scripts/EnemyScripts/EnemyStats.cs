using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    public EnemyStatsObject enemy;

    public string myName = "Hirvitys";
    public float moveSpeed;
    public float jumpForce;
    public float attackSpeed;
    public float attackDamage;
    public float health;
    public float mana;
    public float armor;
    public int xpReward;

    private void Awake ( )
    {
        myName = enemy._name;
        moveSpeed = enemy._moveSpeed;
        jumpForce = enemy._jumpForce;
        attackSpeed = enemy._attackSpeed;
        attackDamage = enemy._attackDamage;
        health = enemy._health;
        mana = enemy._mana;
        armor = enemy._armor;
        xpReward = enemy._xpReward;
    }

}
