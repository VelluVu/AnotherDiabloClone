using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachFromParent : MonoBehaviour
{
  
    void Start()
    {
        gameObject.transform.SetParent ( null );
    }

}
