
using UnityEngine;

public class Rope : MonoBehaviour
{

    public Rigidbody2D connectPoint;
    public GameObject rope;
    public int ropes;

    private void Start ( )
    {
        LinkRobe ( );   
    }

    void LinkRobe()
    {
        Rigidbody2D previousRB = connectPoint ;
        for ( int i = 0 ; i < ropes ; i++ )
        {
            GameObject newRope = Instantiate ( rope, transform );
            HingeJoint2D joint = newRope.GetComponent<HingeJoint2D> ( );      
            joint.connectedBody = previousRB;
            if ( i == 0 )
            {
                joint.connectedAnchor = Vector2.zero;
            }
            previousRB = newRope.GetComponent<Rigidbody2D> ( );
        }
        
    }

}
