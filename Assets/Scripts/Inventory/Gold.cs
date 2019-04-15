using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{

    

   
    public int amount;
    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(ReferenceHolder.instance.player.transform.position,transform.position)<= PlayerClass.instance.pickupRadius.Value)
        {
            transform.position = Vector2.MoveTowards(transform.position,ReferenceHolder.instance.player.transform.position,4*Time.deltaTime);
            Debug.Log("Closing in on player");
        }
    }
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerInventory.instance.gold+= amount;
            Destroy(gameObject);
        }
    }
}
