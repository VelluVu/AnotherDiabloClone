using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveEyesMain : MonoBehaviour
{
    public GameObject eye;
    public float damage;
    public DamageType damageType;
    public float velocity;
    

    public List<Vector3> spawnPositions;
    public List<GameObject> enemies = new List<GameObject>();
    private GameObject player;
    private Animator anim;
    private AbilityINFO abilityInfo;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponentInChildren<Animator>();

    }

    void Start()
    {
        anim.SetTrigger("FiveEyes");
        transform.parent = player.transform;
        abilityInfo = GetComponent<AbilityINFO>();
        setAbilityValues();

        getEnemiesAround();
        postionSet();
        setEyeValues();
        setEyes();
        Destroy(this.gameObject, 1);

    }
    void getEnemiesAround()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.parent.transform.position, new Vector2(15, 4), 0);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject obj = hitColliders[i].gameObject;
            if (obj.CompareTag("Enemy"))
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
    void setEyeValues()
    {
        FiveEyesEye eyeS;
        eyeS = eye.GetComponent<FiveEyesEye>();
        eyeS.damage = damage / spawnPositions.Count;
        eyeS.damageType = damageType;
        eyeS.velocity = velocity;
        eyeS.enemies = enemies;
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

    public void setEyes()
    {
        GameObject spawned;
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            spawned = Instantiate(eye, spawnPositions[i], Quaternion.identity);
            spawned.transform.parent = player.transform;            
        }
    }

}
