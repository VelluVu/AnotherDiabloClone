using TMPro;
using UnityEngine;

public class StatInfo : MonoBehaviour
{

    TextMeshProUGUI textMesh;

    private void Awake ( )
    {
        textMesh = gameObject.GetComponentInChildren<TextMeshProUGUI> ( );
    }

    public void SetStatInfo(string text, int amount)
    {
        textMesh.text = text + " : " + amount.ToString();
    }

}
