
using UnityEngine;

public static class ExtensionMethods
{
    public static void SetParent ( this Transform child, Transform parent )
    {
        child.parent = parent;
        child.localRotation = Quaternion.identity;
        child.localScale = new Vector3 ( 1 + 1 /  parent.localScale.x , 1 + 1 / parent.localScale.y, 1 );
    }
}
