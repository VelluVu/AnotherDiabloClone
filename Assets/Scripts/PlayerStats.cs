using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public PlayerStatsObject clas;

    public string myName = "jokuUkko";
    public string className = "perusJamppa";

    public float moveSpeed;
    public float jumpForce;
    public float baseDamage;
    public float baseAttackSpeed;
    public float health;
    public float mana;
    public float stamina;

    public int strength;
    public int dexterity;
    public int endurance;
    public int energy;

    private void Awake ( )
    {
        myName = clas._name;
        className = clas._className;

        moveSpeed = clas._moveSpeed;
        jumpForce = clas._jumpForce;
        baseDamage = clas._baseDamage;
        baseAttackSpeed = clas._baseAttackSpeed;
        health = clas._health;
        mana = clas._mana;
        stamina = clas._stamina;

        strength = clas._strength;
        dexterity = clas._dexterity;
        endurance = clas._endurance;
        energy = clas._energy;
    }
}
