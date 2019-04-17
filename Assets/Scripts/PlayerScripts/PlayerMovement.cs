
using UnityEngine;

[RequireComponent (typeof(Player))]
public class PlayerMovement : MonoBehaviour
{

    public LayerMask collisionMask;
    public LayerMask collisionDashMask;

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public float maxSlopeAngle = 70;
    public float maxSlopeDescentAngle = 70;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public bool dashing;

    BoxCollider2D col;
    RayCastOrigins rayCastOrigins;
    public CollisionInfo collisions;

    private void Awake ( )
    {
        col = gameObject.GetComponent<BoxCollider2D> ( );
        CalculateRaySpacing ( );
        collisions.faceDirection = 1;
    }
  
    public void Move ( Vector2 velocity )
    {
        UpdateRayCastOrigins ( );
        collisions.Reset ( );
        collisions.velocityOld = velocity;

        if (velocity.x != 0)
        {

            collisions.faceDirection = (int)Mathf.Sign ( velocity.x );
        
        }

        if ( velocity.y < 0 )
        {
            DescendSlope ( ref velocity );
        }
   
        HorizontalCollisions ( ref velocity );
        
        if ( velocity.y != 0 )
        {
            VerticalCollisions ( ref velocity );
        }

        transform.Translate ( velocity );
    }

    void HorizontalCollisions ( ref Vector2 velocity )
    {
        float directionX = collisions.faceDirection;
        float rayLength = Mathf.Abs ( velocity.x ) + skinWidth;

        if ( Mathf.Abs (velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for ( int i = 0 ; i < horizontalRayCount ; i++ )
        {
            Vector2 rayOrigin = ( directionX == -1 ) ? rayCastOrigins.bottomLeft : rayCastOrigins.bottomRight;
            rayOrigin += Vector2.up * ( horizontalRaySpacing * i );
            RaycastHit2D hit; 
            if (!dashing)
            {
                hit = Physics2D.Raycast ( rayOrigin, Vector2.right * directionX, rayLength, collisionMask );
            }
            else
            {
                hit = Physics2D.Raycast ( rayOrigin, Vector2.right * directionX, rayLength, collisionDashMask );
            }

            Debug.DrawRay ( rayOrigin, Vector2.right * directionX * rayLength, Color.red );

            if ( hit )
            {
                float slopeAngle = Vector2.Angle ( hit.normal, Vector2.up );

                if ( i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    if(collisions.descendSlope)
                    {
                        collisions.descendSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimpSlope ( ref velocity, slopeAngle );
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if ( !collisions.climbingSlope || slopeAngle > maxSlopeAngle )
                {
                    velocity.x = ( hit.distance - skinWidth ) * directionX;
                    rayLength = hit.distance;

                    if( collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan ( collisions.slopeAngle * Mathf.Deg2Rad ) * Mathf.Abs ( velocity.x );
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign ( velocity.y );
        float rayLength = Mathf.Abs ( velocity.y ) + skinWidth;

        for ( int i = 0 ; i < verticalRayCount ; i++ )
        {
            Vector2 rayOrigin = ( directionY == -1 ) ? rayCastOrigins.bottomLeft : rayCastOrigins.topLeft;
            rayOrigin += Vector2.right * ( verticalRaySpacing * i + velocity.x );
            RaycastHit2D hit;

            if(!dashing)
            {
                hit = Physics2D.Raycast ( rayOrigin, Vector2.up * directionY, rayLength, collisionMask );
            }
            else
            {
                hit = Physics2D.Raycast ( rayOrigin, Vector2.up * directionY, rayLength, collisionDashMask );
            }

            Debug.DrawRay ( rayOrigin, Vector2.up * directionY *  rayLength, Color.red );

            if(hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if(collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan ( collisions.slopeAngle *  Mathf.Deg2Rad) *  Mathf.Sign(velocity.x );
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
        if(collisions.climbingSlope)
        {
            float directionX = Mathf.Sign ( velocity.x );
            rayLength = Mathf.Abs ( velocity.x ) + skinWidth;
            Vector2 rayOrigin = ( ( directionX == -1 ) ? rayCastOrigins.bottomLeft : rayCastOrigins.bottomRight ) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast ( rayOrigin, Vector2.right * directionX, rayLength, collisionMask );

            if( hit)
            {
                float slopeAngle = Vector2.Angle ( hit.normal, Vector2.up );
                if( slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = ( hit.distance - skinWidth ) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimpSlope(ref Vector2 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs ( velocity.x );
        float climpVelocityY = Mathf.Sin ( slopeAngle * Mathf.Deg2Rad ) * moveDistance;

        if ( velocity.y <= climpVelocityY )
        {
            velocity.y = climpVelocityY;
            velocity.x = Mathf.Cos ( slopeAngle * Mathf.Deg2Rad ) * moveDistance * Mathf.Sign ( velocity.x );
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign ( velocity.x );
        Vector2 rayOrigin = ( directionX == -1 ) ? rayCastOrigins.bottomRight : rayCastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast ( rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask );

        if(hit)
        {
            float slopeAngle = Vector2.Angle ( hit.normal, Vector2.up );
            if( slopeAngle != 0 && slopeAngle <= maxSlopeDescentAngle)
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs ( velocity.x );
                        float descendVelocityY = Mathf.Sin ( slopeAngle * Mathf.Deg2Rad ) * moveDistance;
                        velocity.x = Mathf.Cos ( slopeAngle * Mathf.Deg2Rad ) * moveDistance * Mathf.Sign ( velocity.x );
                        velocity.y -= descendVelocityY;
                        collisions.slopeAngle = slopeAngle;
                        collisions.descendSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void UpdateRayCastOrigins ( )
    {
        Bounds bounds = col.bounds;
        bounds.Expand ( skinWidth * -2 );

        rayCastOrigins.bottomLeft = new Vector2 ( bounds.min.x, bounds.min.y );
        rayCastOrigins.bottomRight = new Vector2 ( bounds.max.x, bounds.min.y );
        rayCastOrigins.topLeft = new Vector2 ( bounds.min.x, bounds.max.y );
        rayCastOrigins.topRight = new Vector2 ( bounds.max.x, bounds.max.y );
    }

    void CalculateRaySpacing ( )
    {
        Bounds bounds = col.bounds;
        bounds.Expand ( skinWidth * -2 );

        horizontalRayCount = Mathf.Clamp ( horizontalRayCount, 2, int.MaxValue );
        horizontalRayCount = Mathf.Clamp ( verticalRayCount, 2, int.MaxValue );

        horizontalRaySpacing = bounds.size.y / ( horizontalRayCount - 1 );
        verticalRaySpacing = bounds.size.x / ( verticalRayCount - 1 );
    }

    struct RayCastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool descendSlope;
        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 velocityOld;
        public int faceDirection;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendSlope = false;        
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}
