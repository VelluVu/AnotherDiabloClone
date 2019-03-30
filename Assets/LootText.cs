using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootText : MonoBehaviour
{
    public PickUpLoot pickLoot;
    public GameObject verticalLayoutObject;
    public Transform lootTextCanvas;
    GameObject group;
    

    private void Awake()
    {
        lootTextCanvas = GameObject.Find("LootTextCanvas").transform;
        GetComponent<Button>().onClick.AddListener(() => GainLoot());
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("LootText"))
        {

            if (group == null && collision.transform.parent.GetComponent<GridLayoutGroup>() == null)
            {
                group = Instantiate<GameObject>(verticalLayoutObject, transform.position, Quaternion.identity);
                group.transform.SetParent(lootTextCanvas);
                transform.SetParent(group.transform);
                Debug.Log("Instantiation!");
            }
            if (transform.parent.childCount >= collision.transform.parent.childCount && transform.parent != collision.transform.parent)
            {
                if (transform.parent.GetComponent<GridLayoutGroup>() != null)
                {
                    collision.transform.SetParent(group.transform);

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
        Debug.Log("Pickup");
        pickLoot.pickedUp = true;
        PlayerInventory.instance.AddItem(pickLoot.rLoot, 1, true);
        pickLoot.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
