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
        if ( !controller.notHatchedEgg )
        {
            controller.notHatchedEgg = true;
            Vector2 position;
            if ( controller.chaseTarget.transform.position.x > controller.transform.position.x )
            {
                controller.dirRight = false;
                position = new Vector2 ( controller.transform.position.x + 0.5f , controller.transform.position.y + 0.2f );
                GameObject newEgg = Instantiate ( egg, position, Quaternion.identity ) as GameObject;
                newEgg.GetComponent<SpiderEgg> ( ).SetParent ( controller.gameObject );
                controller.rb.velocity = Vector2.left * 6 * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * Time.deltaTime ;
                
            }
            else if ( controller.chaseTarget.transform.position.x < controller.transform.position.x )
            {
                controller.dirRight = true;
                position = new Vector2 ( controller.transform.position.x - 0.5f, controller.transform.position.y + 0.2f );
                GameObject newEgg = Instantiate ( egg, position, Quaternion.identity ) as GameObject;
                newEgg.GetComponent<SpiderEgg> ( ).SetParent ( controller.gameObject );
                controller.rb.velocity = Vector2.right * 6 * controller.enemyStats.moveSpeed.Value * controller.moveSpeedScale * Time.deltaTime ;
                

            }

        }    
    }

    
}
