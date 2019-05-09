using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeCloud : MonoBehaviour
{
    public float damage;
    public DamageType damageType;
    public float period=1;
    public float durantion;
    BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        Destroy(this.gameObject, durantion);
        InvokeRepeating("enableSmokeCollider", 0, period);
    }

    void enableSmokeCollider()
    {
        if(col.isActiveAndEnabled)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        GameObject go = c.gameObject.transform.root.gameObject;
        if (go.CompareTag("Enemy"))
        {
            if (go.GetComponent<StateController>() != null)
            {
                go.GetComponent<StateController>().TakeDamage(go, damage, false, damageType, true, 100);
            }

        }

    }
}
