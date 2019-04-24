using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebSpot : MonoBehaviour
{
    [Range (0,100)]public float slowAmountPercent;

    public float GetSlowPercent()
    {
        return slowAmountPercent * 0.01f;
    }
}
