using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootText : MonoBehaviour
{
    public PickUpLoot pickLoot;
    public GameObject verticalLayoutObject;
    public Transform lootTextCanvas;
    public GameObject group;
    CanvasGroup canvasGroup;
    


    private void Start()
    {
        lootTextCanvas = GameObject.Find("LootTextCanvas").transform;
        GetComponent<Button>().onClick.AddListener(() => GainLoot());
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(TimerHideLootText());
        
    }
    private void OnEnable()
    {
        KeyboardManager.LootTextAppearEvent += ShowLootText;
    }
    private void OnDisable()
    {
        KeyboardManager.LootTextAppearEvent -= ShowLootText;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {

        
        if (collision.CompareTag("LootText") && pickLoot.GetComponent<Rigidbody2D>().velocity.magnitude < 0.01f)
        {
            
            
            if (group == null && collision.transform.parent.GetComponent<GridLayoutGroup>() == null)
            {
                group = Instantiate<GameObject>(verticalLayoutObject, transform.position, Quaternion.identity);
                group.transform.SetParent(lootTextCanvas);
                transform.SetParent(group.transform);
                
            }
            if (transform.parent.childCount >= collision.transform.parent.childCount && transform.parent != collision.transform.parent)
            {
                if (group != null)
                {
                    
                   
                    collision.transform.SetParent(group.transform);

                }
                else 
                {
                    transform.SetParent(collision.transform.parent);
                }
                
            }
            else
            {
                transform.SetParent(collision.transform.parent);
            }
        }
    }
    public void GainLoot()
    {
        if (PlayerInventory.instance.IsLootSlotsFull())
        {
            Debug.LogWarning("Inventory Full");
            return;
        }
        Debug.Log("Pickup");
        pickLoot.pickedUp = true;
        PlayerInventory.instance.AddItem(pickLoot.rLoot, pickLoot.Count, true);
        Destroy(pickLoot.gameObject);
        Destroy(gameObject);
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Vector2.Distance(ReferenceHolder.instance.player.transform.position, transform.position)<3)
        {
            canvasGroup.interactable = true;
        }
        else
        {
            canvasGroup.interactable = false;
            
        }
        if(transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.identity;
        }
    }
    public IEnumerator TimerHideLootText()
    {
        yield return new WaitForSeconds(Settings.instance.lootTextDisappearTimer);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

    }
    public void ShowLootText()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        TimerHideLootText();
    }

}
