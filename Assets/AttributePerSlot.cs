using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributePerSlot : MonoBehaviour
{
    public static AttributePerSlot instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    public List<SlotAttribute> slotAttributes = new List<SlotAttribute>();

  
   public SlotAttribute findSlot(ArmorSlot slot)
   {
        foreach(SlotAttribute sAtt in slotAttributes)
        {
            if(sAtt.slot == slot)
            {
                return sAtt;
            }
        }
        Debug.LogError("Failed to Find SlotAttribute, please fix.");
        return new SlotAttribute();
    }
  
}
[System.Serializable]
public struct SlotAttribute
{
    public ArmorSlot slot;
    public List<Attribute> constantAttributes;
    public List<Attribute> primaryAttributes;
    public List<Attribute> secondaryAttributes;

}

