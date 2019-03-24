using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simppeli Kameraa seuraa vaan pelaajaa..
/// </summary>
public class CameraControl : MonoBehaviour {

    public Transform player;

    private void LateUpdate ( )
    {
        transform.position = new Vector3 ( player.position.x, player.position.y, -1);
    }
}
