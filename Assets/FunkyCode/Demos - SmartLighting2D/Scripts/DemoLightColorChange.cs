﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoLightColorChange : MonoBehaviour {
    public float speed = 1f;
    public float time = 1f;
    public Color[] colors;

    int currentID = 0;
    TimerHelper timer;
    LightingSource2D lightSource;

    void Start() {
        timer = TimerHelper.Create();

        lightSource = GetComponent<LightingSource2D>();
    }

    void Update() {
        Color color = lightSource.lightColor;
        Color newColor = colors[currentID];

        color.r = Mathf.Lerp(color.r, newColor.r, Time.deltaTime * speed);
        color.g = Mathf.Lerp(color.g, newColor.g, Time.deltaTime * speed);
        color.b = Mathf.Lerp(color.b, newColor.b, Time.deltaTime * speed);

        lightSource.lightColor = color;

        if (timer.Get() > time) {
            timer.Reset();

            currentID += 1;

            if (currentID >= colors.Length) {
                currentID = 0;
            }
        }
    }
}
