using UnityEngine;

/// <summary>
/// Spider Enemy LayEgg Action
/// </summary>
[CreateAssetMenu ( menuName = "PluggableAI/Actions/EggAction" )]
public class SpiderUniqueAction : Action
{

    public GameObject egg;

    public override void Act ( StateController controller )
    {
        LayEgg ( controller );
    }

    void LayEgg ( StateController controller )
    {
        if(controller.chaseTarget == null)
        {
            return;
        }
        if ( !controller.notHatchedEgg )
        {
            controller.notHatchedEgg = true;
            Vector2 position;
            if ( controller.chaseTarget.transform.position.x > controller.transform.position.x )
            {
                controller.dirRight = false;
                position = new Vector2 ( controller.back.position.x  , controller.back.position.y );
                GameObject newEgg = Instantiate ( egg, position, Quaternion.identity ) as GameObject;
                newEgg.GetComponent<SpiderEgg> ( ).SetParent ( controller.gameObject );
                newEgg.GetComponent<Rigidbody2D> ( ).AddForce ( (Vector2.right + Vector2.up) *  5f, ForceMode2D.Impulse );
                controller.rb.velocity = Vector2.left * 6 * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * Time.deltaTime ;
                
            }
            else if ( controller.chaseTarget.transform.position.x < controller.transform.position.x )
            {
                controller.dirRight = true;
                position = new Vector2 ( controller.back.position.x , controller.back.position.y  );
                GameObject newEgg = Instantiate ( egg, position, Quaternion.identity ) as GameObject;
                newEgg.GetComponent<SpiderEgg> ( ).SetParent ( controller.gameObject );
                newEgg.GetComponent<Rigidbody2D> ( ).AddForce ( (Vector2.left + Vector2.up ) * 5f, ForceMode2D.Impulse );
                controller.rb.velocity = Vector2.right * 6 * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * Time.deltaTime ;
                

            }

        }    
    }

    
}
