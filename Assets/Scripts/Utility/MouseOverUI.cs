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
        if( EventSystem.current.currentSelectedGameObject != null)
        {
            ReferenceHolder.instance.player.SetMouseOnUI ( true );
        }
        else
        {
            ReferenceHolder.instance.player.SetMouseOnUI ( false );
        }
    }

}
