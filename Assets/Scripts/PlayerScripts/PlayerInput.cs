using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;

    private void Awake ( )
    {
        player = GetComponent<Player> ( );
    }

    private void Update ( )
    {
        Vector2 directionalInput = new Vector2 ( Input.GetAxisRaw ( "Horizontal" ), Input.GetAxisRaw ( "Vertical" ) );
        player.SetDirectionalInput ( directionalInput );

        if(Input.GetButtonDown("Jump"))
        {
            player.OnJumpInputDown ( );
        }
        if ( Input.GetButtonUp ( "Jump" ) )
        {
            player.OnJumpInputUp ( );
        }
        if(Input.GetMouseButton(1))
        {
            player.GetSecondMouseButton ( );
        }
        if(Input.GetMouseButtonUp(1))
        {
            player.GetSecondMouseButtonUp ( );
        }
        if( Input.GetButtonDown ( "OpenInventory" ) )
        {
            player.OpenInventory ( );
        }
        if( Input.GetMouseButton ( 0 ) )
        {
            player.PlayerBasicAttack ( );
        }
        if( Input.GetButtonDown ( "Dash" ) )
        {
            player.DashInputDown ( );
        }
        if( Input.GetButton ( "Dash" ) )
        {
            player.DashInputHold ( );
        }
        if ( Input.GetButtonUp ( "Dash" ) )
        {
            player.DashInputUp ( );
        }  
    }
}
