using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableSpot : MonoBehaviour
{
    public Image cooldownSprite;
    public bool onCooldown = false;
    public float cooldownTimer = 0;
    private void Start()
    {
        cooldownSprite.fillAmount = 0;
    }
    private void Update()
    {
        if (onCooldown)
        {
            cooldownTimer += Time.deltaTime;
            cooldownSprite.fillAmount = 1-( cooldownTimer / PlayerInventory.instance.consumableCooldown);
            Debug.Log(cooldownTimer / PlayerInventory.instance.consumableCooldown);
            if (cooldownTimer >= PlayerInventory.instance.consumableCooldown)
            {
                onCooldown = false;
                cooldownTimer = 0;
                cooldownSprite.fillAmount = 0;
            }
            
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!GetComponent<LootSlot>().isEmpty)
                {
                    onCooldown = true;
                    foreach(RollAttribute rA in GetComponent<LootSlot>().item.attributes)
                    {
                        rA.Consume();
                    }
                    transform.parent.GetComponent<LootSlot>().stackSize -= 1;
                    if (GetComponent<LootSlot>().stackSize < 1)
                    {
                        GetComponent<LootSlot>().emptySlot();

                    }
                    else
                    {
                        GetComponent<LootSlot>().stackSizeTextEnable(true);
                    }
                }
                
            }
        }
    }

    
}
