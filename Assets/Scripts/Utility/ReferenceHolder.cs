using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceHolder : MonoBehaviour
{

    public Player player;
    public Transform goreHolder;
    public Transform combatTextHolder;
    public static ReferenceHolder instance { get; private set; }

    private void Awake ( )
    {
        if (instance == null) { instance = this; }
        else { Destroy ( gameObject ); }

        player = GameObject.FindObjectOfType<Player> ( );
    }

}
