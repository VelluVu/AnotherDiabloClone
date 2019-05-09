using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputInfoScript : MonoBehaviour
{
    TextMeshProUGUI textM;

    private void Start ( )
    {
        textM = gameObject.GetComponentInChildren<TextMeshProUGUI> ( );
    }

    public void SetText(string inputKey)
    {
        textM.text = inputKey;
    }



}
