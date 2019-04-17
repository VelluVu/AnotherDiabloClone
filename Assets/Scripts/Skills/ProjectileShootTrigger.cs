using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShootTrigger : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public float projectileForce;
    [HideInInspector] public float damage;
    [HideInInspector] public float mass;
    [HideInInspector] public float healingEnemy;
    [HideInInspector] public float healingPlayer;
    [HideInInspector] public float manaUsage;
   
    private Transform spawn;
    private Vector2 dir;


    public void setValues(ProjectileAbility skill)
    {
        rb2D = skill._rb2D;
        projectileForce = skill._protectileForce;
        damage = skill._damange;
        mass = skill._mass;
        healingPlayer = skill._healingPlayer;
        healingEnemy = skill._healingEnemy;
        manaUsage = skill.manaUsage;
        

    }

    public void Launch(Ability skill)
    {
        if(skill is ProjectileAbility)
        {
            setValues(skill as ProjectileAbility);
        }
        else
        {
            Debug.Log("ABILITY wasn't protectile ability");
            return;
        }

        if(this.gameObject.GetComponent<Player> ()  != null)
        {
            this.gameObject.GetComponent<Player>().OnManaUse(manaUsage);
        }
            

        spawn = transform;
        // tästä saadaan hiiren osoittama suunta ja laukstaan se siihe suuntaan
        dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - spawn.position;
        Rigidbody2D clonedProjectile = Instantiate(rb2D, spawn.position, Quaternion.identity) as Rigidbody2D;
        
        AbilityINFO info = clonedProjectile.gameObject.AddComponent<AbilityINFO>();
        clonedProjectile.mass = mass;
        info._damage = damage;
        info._direction = dir;
        info._spawnPos = spawn.position;
        info._healingEnemy = healingEnemy;
        info._healingPlayer = healingPlayer;
       
        

        clonedProjectile.AddForce(dir.normalized * projectileForce);
    }
}
