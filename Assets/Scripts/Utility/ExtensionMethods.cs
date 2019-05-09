
using UnityEngine;

public static class ExtensionMethods
{
    public static void SetParent ( this Transform child, Transform parent )
    {
        child.parent = parent;
        child.localRotation = Quaternion.identity;
    }

    public static Vector2 GetX (this Vector2 v)
    {
        return new Vector2(v.x, 0);
    }

    public static float FlatDistanceTo (this Vector2 from, Vector2 to)
    {
        Vector2 a = from.GetX ( );
        Vector2 b = to.GetX ( );
        return Vector2.Distance ( a, b );
    }
   
}


