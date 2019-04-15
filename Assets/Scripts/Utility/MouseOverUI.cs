using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Vaihtaa pelaajan mouseOverUI bool trueksi;
/// </summary>
public class MouseOverUI : MonoBehaviour
{

    private void Update ( )
    {
        CheckMouseHover ( );
    }

    public void CheckMouseHover ( )
    {

        if( EventSystem.current.IsPointerOverGameObject()) { 
      
            ReferenceHolder.instance.player.SetMouseOnUI ( true );
            Debug.Log("TRUEEE");
        }
        else
        {
            ReferenceHolder.instance.player.SetMouseOnUI ( false );
        }
    }
}
