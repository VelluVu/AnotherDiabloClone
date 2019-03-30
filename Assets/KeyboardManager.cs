using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager instance;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (GameObject.Find("LootTextCanvas").GetComponent<Canvas>().enabled)
            {
                GameObject.Find("LootTextCanvas").GetComponent<Canvas>().enabled = false;
            }
            else
            {
                GameObject.Find("LootTextCanvas").GetComponent<Canvas>().enabled = true;
            }
                
        }
    }
}
