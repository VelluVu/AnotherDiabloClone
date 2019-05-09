using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Button newGameButton;
    public Button loadGameButton;
    public Button optionsButton;
    public Button statsButton;
    public Button exitButton;
    public GameObject loadingBackground;
    public GameObject options;
    public GameObject stats;
   
    // Start is called before the first frame update
    void Start()
    {
        newGameButton.onClick.AddListener(() => DoOnClick(ButtonPress.newGame));
        loadGameButton.onClick.AddListener(() => DoOnClick(ButtonPress.loadGame));
        optionsButton.onClick.AddListener(() => DoOnClick(ButtonPress.options));
        statsButton.onClick.AddListener(() => DoOnClick(ButtonPress.stats));
        exitButton.onClick.AddListener(() => DoOnClick(ButtonPress.exit));
        if (GameObject.Find("UICanvas"))
        {
            Destroy(GameObject.Find("UICanvas"));
        }
        
    }
    void DoOnClick(ButtonPress press)
    {
        if(press == ButtonPress.newGame)
        {
            StartGame(false);
            Debug.Log("Start Game");
        }
        if (press == ButtonPress.loadGame)
        {
            StartGame(true);
            Debug.Log("Load Game");
        }
        if (press == ButtonPress.options)
        {
            options.SetActive(true);
            FindObjectOfType<Options>().optionsOpen = true;
            FindObjectOfType<Options>().Load();
            Debug.Log("Options Opened");
        }
        if (press == ButtonPress.stats)
        {
            stats.SetActive(true);
            FindObjectOfType<StatsMenu>().statsOpen = true;
            FindObjectOfType<StatsMenu>().UpdateStats();
            Stats.instance.Load();
            Debug.Log("Stats Opened");

        }
        if (press == ButtonPress.exit)
        {
            Application.Quit();
        }
    }
    void StartGame(bool load)
    {
        
        if (load)
        {
            // load the game
            FindObjectOfType<SaveLoadManager>().LoadGameOnStart = true;
        }
        else
        {
            FindObjectOfType<SaveLoadManager>().LoadGameOnStart = false;
            // start a new game
        }
        loadingBackground.SetActive(true);
        FindObjectOfType<SaveLoadManager>().StartCoroutine("LoadNewScene");


    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public enum ButtonPress
    {
        newGame,loadGame,options,stats,exit
    }
}
