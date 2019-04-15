using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosForInvisiblePoints : MonoBehaviour
{

    public Color color;
    public float gizmoRadius;

    private void OnDrawGizmos ( )
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere ( transform.position, gizmoRadius );
    }

}
