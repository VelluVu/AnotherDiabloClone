using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributePerSlot : MonoBehaviour
{
    public Attribute [] allAttributes;
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
        
        allAttributes = Resources.FindObjectsOfTypeAll<Attribute>();
        int tempId = 0;
        foreach(Attribute att in allAttributes){
            att.id = tempId;
            tempId++;
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

