using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager instance;
    public delegate void LootTextAppear();
    public static event LootTextAppear LootTextAppearEvent;

    public bool holdingShift; // if holding shift
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
        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKey(KeyCode.L))
        {
            ShowLoot();
        }
        ShiftKey();
    }
    public void ShowLoot()
    {
        if(LootTextAppearEvent != null)
        {
            LootTextAppearEvent();
        }
        
    }
    public void ShiftKey()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            holdingShift = true;
        }
        else
        {
            holdingShift = false;
        }
    }
}
