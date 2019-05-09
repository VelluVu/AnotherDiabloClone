using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.LoadScene(2,LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
