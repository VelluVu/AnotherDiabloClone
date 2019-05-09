using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    public Rarity chestRarity;
    public int chestLevel = 1;
    Animator treasureAnimator;
    Collider2D treasureCollider;
    Collider2D triggerArea;
    AudioSource source;
    bool canOpen;

    public delegate void TreasureChestSoundDelegate ( AudioSource source, ObjectSoundType objSound );
    public static event TreasureChestSoundDelegate TreasureSoundEvent;

    private void OnEnable ( )
    {
        OtherTriggerEvent.TriggerEnterEvent += OnTriggerEnterArea;
        OtherTriggerEvent.TriggerExitEvent += OnTriggerExitArea;
    }
    private void OnDisable ( )
    {
        OtherTriggerEvent.TriggerEnterEvent -= OnTriggerEnterArea;
        OtherTriggerEvent.TriggerExitEvent -= OnTriggerExitArea;
    }

    private void Start ( )
    {
        treasureAnimator = gameObject.GetComponent<Animator> ( );
        treasureCollider = gameObject.GetComponent<Collider2D> ( );
        triggerArea = gameObject.GetComponentInChildren<Collider2D> ( );
        source = gameObject.GetComponent<AudioSource> ( );
    }

    public void OnTriggerEnterArea ( GameObject targetObject )
    {
        if ( gameObject == targetObject )
        {
            canOpen = true;
        }
    }

    public void OnTriggerExitArea ( GameObject targetObject )
    {
        if ( gameObject == targetObject )
        {
            canOpen = false;
        }
    }

    private void Update ( )
    {
        Aukene ( );
    }
    public void Aukene ( )
    {
        if ( canOpen && Input.GetButton("Interaction"))
        {
            if( TreasureSoundEvent != null)
            {
                TreasureSoundEvent ( source, ObjectSoundType.TreasureChest );
            }
            treasureAnimator.SetTrigger ( "Open" );
            triggerArea.enabled = false;
            treasureCollider.enabled = false;
            gameObject.GetComponent<TreasureChest> ( ).enabled = false;
            LootDropGenerator.instance.ChestDrop(transform,chestRarity,chestLevel);
        }
    }
}
