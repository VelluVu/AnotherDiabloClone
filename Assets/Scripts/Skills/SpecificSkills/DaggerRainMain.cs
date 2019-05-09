using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerRainMain : MonoBehaviour
{
    public GameObject dagger;
    public float damage;
    public DamageType damageType;
    public float velocity;

    public List<Vector3> spawnPositions;
    public List<GameObject> enemies;
    private GameObject player;
    private AbilityINFO abilityInfo;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Start()
    {
        transform.parent = player.transform;
        abilityInfo = GetComponent<AbilityINFO>();
        setAbilityValues();

        getEnemiesAround();
        postionSet();
        setDaggerValues();        
        setDaggers();

    }
    void getEnemiesAround()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.parent.transform.position, new Vector2(8, 2), 0);

        for (int i = 0; i < hitColliders.Length; i++ )
        {
            GameObject obj = hitColliders[i].gameObject;
            if(obj.CompareTag("Enemy"))
            {
                if (!enemies.Contains(obj.transform.root.gameObject))
                    enemies.Add(obj.transform.root.gameObject);
            }
        }
    }
    void setAbilityValues()
    {
        damage = abilityInfo._damage;
        damageType = abilityInfo._damageType;
        velocity = abilityInfo._velocity;
    }
    void setDaggerValues()
    {
        DaggerRainDagger daggerS;
        daggerS = dagger.GetComponent<DaggerRainDagger>();
        daggerS.damage = damage / spawnPositions.Count;
        daggerS.damageType = damageType;
        daggerS.velocity = velocity;
        daggerS.enemies = enemies;
    }
    public void postionSet()
    {
        Vector3 pos = transform.position;
        Vector3 secondRow = new Vector3(0.2f, 0.45f, 0);
        Vector3 thirdRow = new Vector3(0.4f, 0.3f, 0);

        spawnPositions.Add(pos + new Vector3(0, 0.5f, 0));

        spawnPositions.Add(pos + secondRow);
        secondRow.x *= -1;
        spawnPositions.Add(pos + secondRow);

        spawnPositions.Add(pos + thirdRow);
        thirdRow.x *= -1;
        spawnPositions.Add(pos + thirdRow);
    }
    
    public void setDaggers()
    {
        GameObject spawned;        
        for(int i=0; i < spawnPositions.Count; i++)
        {
            spawned = Instantiate(dagger, spawnPositions[i], Quaternion.identity);
            spawned.transform.parent = player.transform;            
        }
    }

}
