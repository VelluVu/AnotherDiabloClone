using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLifeTime : MonoBehaviour
{

    public float lifeTime;

    void Start ( )
    {

        StartCoroutine ( Vanish ( ) );
    }

    IEnumerator Vanish ( )
    {
        yield return new WaitForSeconds ( lifeTime );
        gameObject.SetActive ( false );
        if(gameObject.GetComponent<Item>().pickedUp == false)
        {
            Destroy ( gameObject );
        }

    }

}
