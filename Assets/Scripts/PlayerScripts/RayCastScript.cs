using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent ( typeof ( BoxCollider2D ) )]
public class RayCastScript : MonoBehaviour
{

    public LayerMask collisionMask;
    public LayerMask collisionDashMask;

    public const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    [HideInInspector] public float horizontalRaySpacing;
    [HideInInspector] public float verticalRaySpacing;

    [HideInInspector] public BoxCollider2D col;
    public RayCastOrigins rayCastOrigins;

    public virtual void Start ( )
    {
        col = gameObject.GetComponent<BoxCollider2D> ( );
        CalculateRaySpacing ( );
    }

    public void UpdateRayCastOrigins ( )
    {
        Bounds bounds = col.bounds;
        bounds.Expand ( skinWidth * -2 );

        rayCastOrigins.bottomLeft = new Vector2 ( bounds.min.x, bounds.min.y );
        rayCastOrigins.bottomRight = new Vector2 ( bounds.max.x, bounds.min.y );
        rayCastOrigins.topLeft = new Vector2 ( bounds.min.x, bounds.max.y );
        rayCastOrigins.topRight = new Vector2 ( bounds.max.x, bounds.max.y );
    }

    public void CalculateRaySpacing ( )
    {
        Bounds bounds = col.bounds;
        bounds.Expand ( skinWidth * -2 );

        horizontalRayCount = Mathf.Clamp ( horizontalRayCount, 2, int.MaxValue );
        horizontalRayCount = Mathf.Clamp ( verticalRayCount, 2, int.MaxValue );

        horizontalRaySpacing = bounds.size.y / ( horizontalRayCount - 1 );
        verticalRaySpacing = bounds.size.x / ( verticalRayCount - 1 );
    }

    public struct RayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

}
