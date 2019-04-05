using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMode : MonoBehaviour
{
    public static DevMode instance;

    public bool DevModeOn = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        

    }
}
