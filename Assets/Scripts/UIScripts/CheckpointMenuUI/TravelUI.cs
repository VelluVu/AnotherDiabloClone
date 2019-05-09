using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelUI : MonoBehaviour
{
    public CheckPointUIScript checkPointUI;
    public List<GameObject> checkPoints = new List<GameObject> ( );
    public Button exitButton;
    public Button [ ] buttons;

    private void Awake ( )
    {

        exitButton.onClick.AddListener ( ( ) => ExitTravelMenu ( ) );

        buttons [ 0 ].onClick.AddListener ( ( ) => TravelTo ( checkPoints [ 0 ] ) );
        buttons [ 1 ].onClick.AddListener ( ( ) => TravelTo ( checkPoints [ 1 ] ) );
        buttons [ 2 ].onClick.AddListener ( ( ) => TravelTo ( checkPoints [ 2 ] ) );
        buttons [ 3 ].onClick.AddListener ( ( ) => TravelTo ( checkPoints [ 3 ] ) );

    }

    private void OnEnable ( )
    {
        for ( int i = 0 ; i < checkPoints.Count ; i++ )
        {

            if ( checkPoints [ i ].GetComponent<CheckPoint> ( ).visited )
            {
                buttons [ i ].gameObject.SetActive ( true );
            }
        }
    }

    void ExitTravelMenu ( )
    {
        gameObject.SetActive ( false );
    }

    void TravelTo ( GameObject checkPoint )
    {

        ReferenceHolder.instance.player.transform.position = checkPoint.transform.position;
        checkPointUI.ExitCheckPoint ( );
        
    }

}
