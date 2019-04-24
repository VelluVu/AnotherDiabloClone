
using UnityEngine;

public static class ExtensionMethods
{
    public static void SetParent ( this Transform child, Transform parent )
    {
        child.parent = parent;
        child.localRotation = Quaternion.identity;
       
    }
}
