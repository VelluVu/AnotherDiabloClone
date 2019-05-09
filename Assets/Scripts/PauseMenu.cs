using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Button ResumeButton;
    public Button OptionsButton;
    public Button MainMenuButton;
    public GameObject optionsObject;

    // Start is called before the first frame update
    void Start()
    {
        ResumeButton.onClick.AddListener(() => DoOnClick("Resume"));
        OptionsButton.onClick.AddListener(() => DoOnClick("Options"));
        MainMenuButton.onClick.AddListener(() => DoOnClick("MainMenu"));
    }

    void DoOnClick(string action)
    {
        if(action == "Resume")
        {
            KeyboardManager.instance.openPauseMenu = true;
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
        else if(action == "Options")
        {
            optionsObject.SetActive(true);
            FindObjectOfType<Options>().optionsOpen = true;
            FindObjectOfType<Options>().Load();
        }
        else if(action == "MainMenu")
        {
            foreach(DontDestroyOnLoad dont in FindObjectsOfType<DontDestroyOnLoad>())
            {
                if(dont.gameObject.name != "UICanvas")
                {
                    Destroy(dont.gameObject);
                }
                
            }
            
            
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
