using UnityEngine;

public class ExtraStatBox : MonoBehaviour
{

    public GameObject statInfo;
    GameObject [ ] newStatInfo;
    bool spawnedStats;

    private void Awake ( )
    {
        newStatInfo = new GameObject [ PlayerClass.instance.listCharacterStats.Count ];
    }

    private void OnEnable ( )
    {
        if ( !spawnedStats )
        {
            FindStats ( );
        }
        else
        {
            UpdateStats ( );
        }
    }

    void FindStats()
    {

        for ( int i = 0 ; i < PlayerClass.instance.listCharacterStats.Count ; i++ )
        {
            newStatInfo[i] = Instantiate ( statInfo, transform );
            newStatInfo[i].GetComponent<StatInfo> ( ).SetStatInfo ( PlayerClass.instance.listCharacterStats[i].statName, ( int ) PlayerClass.instance.listCharacterStats [ i ].Value );

        }
        spawnedStats = true;
    }

    void UpdateStats()
    {
        for ( int i = 0 ; i < newStatInfo.Length ; i++ )
        {
            newStatInfo[i].GetComponent<StatInfo> ( ).SetStatInfo ( PlayerClass.instance.listCharacterStats [ i ].statName, ( int ) PlayerClass.instance.listCharacterStats [ i ].Value );
        }
    }

}
