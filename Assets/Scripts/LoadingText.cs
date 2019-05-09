using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class LoadingText : MonoBehaviour
{
    TMP_Text loadingText;
    int x = 0;
    int maxDots = 3;
    float timer = 0;
    public float delay = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        loadingText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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
            timer = 0;
        }
        loadingText.text = "Loading" + new string('.',x);
        
    }
}
