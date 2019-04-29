using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplash : MonoBehaviour
{
    public GameObject bloodParticle;

    private void Start ( )
    {
        GameObject createdObject = Instantiate(bloodParticle, transform.position, Quaternion.identity);
        createdObject.transform.SetParent(ReferenceHolder.instance.goreHolder);
        Destroy(createdObject, 3f);
    }
}
