
using UnityEngine;

public class PlayerMovement : RayCastScript
{

    public float maxSlopeAngle = 70;
    public float maxSlopeDescentAngle = 70;

    public CollisionInfo collisions;

    public bool dashing;

    public override void Start ( )
    {
        base.Start ( );
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

        if ( !collisions.onElevator )
        {
            transform.SetParent ( null );
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
                //Debug.DrawRay ( rayOrigin, Vector2.right * directionX );
            }
            else
            {
                hit = Physics2D.Raycast ( rayOrigin, Vector2.right * directionX, rayLength, collisionDashMask );
                //Debug.DrawRay ( rayOrigin, Vector2.right * directionX );
            }

            //Debug.DrawRay ( rayOrigin, Vector2.right * directionX * rayLength, Color.red );

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
                    ClimbSlope ( ref velocity, slopeAngle );
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
                if ( hit.collider.gameObject.CompareTag ( "Enemy" ) && (collisions.right || collisions.left) )
                {
                    velocity.x = 0;
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

                if (hit.collider.gameObject.CompareTag("Ground") && collisions.below)
                {
                    collisions.hitsGround = true;
                }

                if ( hit.collider.gameObject.CompareTag ( "MovingPlatform" ) && collisions.below )
                {
                    collisions.onElevator = true;

                    if ( collisions.onElevator )
                    {
                        ExtensionMethods.SetParent ( transform, hit.collider.transform );                
                    }              
                }
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

    void ClimbSlope(ref Vector2 velocity, float slopeAngle)
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

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool descendSlope;
        public bool climbingSlope;
        public bool onElevator;
        public bool hitsGround;
        public bool falling;
        public float slopeAngle, slopeAngleOld;      
        public Vector2 velocityOld;
        public int faceDirection;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendSlope = false;
            hitsGround = false;
            falling = false;
            onElevator = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}
