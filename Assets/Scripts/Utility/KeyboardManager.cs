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

    public GameObject secret;
    public GameObject CheckPointOpen;
    public bool onCheckPoint;
    public GameObject pauseMenu;
    public bool openPauseMenu;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu(openPauseMenu);
        }
        if (Input.GetKeyDown(KeyCode.Home))
        {
            secret.SetActive(true);
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
    public void OpenPauseMenu(bool open)
    {
        pauseMenu.SetActive(open);
        if (open)
        {
            Time.timeScale = 0;
            openPauseMenu = false;
        }
        else
        {
            onCheckPoint = CheckPointOpen.activeInHierarchy;
            if (!onCheckPoint)
            {
                Time.timeScale = 1;
            }
            
            openPauseMenu = true;
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
