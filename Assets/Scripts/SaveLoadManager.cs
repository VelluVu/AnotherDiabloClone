using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveLoadManager : MonoBehaviour
{
    public bool LoadGameOnStart =false;
    public GameObject loadingBackground;
    public TMP_Text loadingText;
    int x = 0;
    int maxDots = 3;
    float timer = 0;
    public float delay = 0.3f;
    // Start is called before the first frame update

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable ( )
    {
        CheckPoint.CheckPointVisitEvent += AutoSave;
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        CheckPoint.CheckPointVisitEvent -= AutoSave;
    }
    public IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        timer = Time.time;
        delay = Time.time + delay;

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            
            Debug.Log(async.progress);
            timer += Time.deltaTime;
            Debug.Log(timer);
            if (timer > delay)
            {
                if (x < 3)
                {
                    x++;
                }
                else
                {
                    x = 0;
                }
                delay = Time.time + delay;
            }
            loadingText.text = "Loading" + new string('.', x);
            yield return null;
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        
        if(scene == SceneManager.GetSceneByBuildIndex(2))
        {

            Destroy(loadingBackground.transform.parent.gameObject);
             
            if (LoadGameOnStart)
            {
                LoadGame();
            }
        }
    }
    public void LoadGame()
    {
        PlayerClass.instance.Load();
        PlayerInventory.instance.LoadInventory();
        Stats.instance.Load();
    }
    public void SaveGame(int checkpointId)
    {
        PlayerClass.instance.Save(checkpointId);
        PlayerInventory.instance.SaveInventory();
        Stats.instance.Save();
    }

    public void AutoSave(Transform checkPointPos, int checkpointId)
    {
        PlayerClass.instance.Save ( checkpointId );
        PlayerInventory.instance.SaveInventory ( );
        Stats.instance.Save();
        Debug.Log ( "Game Saved" );
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
