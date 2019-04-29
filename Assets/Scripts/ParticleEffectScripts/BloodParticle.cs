using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour
{

    ParticleSystem pSystem;
    List <ParticleCollisionEvent> particleCollisions;
    public GameObject[] bloodDecal;
    

    private void Start ( )
    {
        pSystem = GetComponent<ParticleSystem> ( );
        particleCollisions = new List<ParticleCollisionEvent> ( );
    }

    private void OnParticleCollision ( GameObject other )
    {

        int collisions = pSystem.GetCollisionEvents ( other, particleCollisions );

        for ( int i = 0 ; i < collisions ; i++ )
        {
            GameObject createdObject = Instantiate(bloodDecal[Random.Range(0,bloodDecal.Length - 1)], particleCollisions[i].intersection, Quaternion.identity);
            createdObject.transform.SetParent(ReferenceHolder.instance.goreHolder);
        }
        
    }
}
