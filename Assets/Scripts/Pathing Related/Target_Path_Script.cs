using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Path_Script : MonoBehaviour
{
    [SerializeField]
    Transform[] Control_Points;
  
    public bool OtherlineType;

    Vector2 gizmosPOS;
    //This script will draw the line for our bezier curve in scene view

    private void OnDrawGizmos()
    {
        // this line is the paramter of the function of bezier curve
        // where t>0 && t<=1; the for loops allows us to step through the line



        if (!OtherlineType)
        {
           

            for (float t = 0; t <= 1; t += 5 / 100f)
            {
                //same thing as: B(t) = (1-t)^3*P(0) +3*(1-t)^2*t*P(1)+3(1-t)*t^2*P(2)+ t^3*P(3)
                gizmosPOS = Mathf.Pow(1 - t, 3) * Control_Points[0].position + 3 * Mathf.Pow(1 - t, 2) * t * Control_Points[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * Control_Points[2].position + Mathf.Pow(t, 3) * Control_Points[3].position;

                Gizmos.DrawWireSphere(gizmosPOS, 1 / 4f);


            }
            // these lines are to help visualize the editing of said bezier curve
            // first line is 1,2 second is 3/4

            Gizmos.DrawLine(new Vector2(Control_Points[0].position.x, Control_Points[0].position.y), new Vector2(Control_Points[1].position.x, Control_Points[1].position.y));
            Gizmos.DrawLine(new Vector2(Control_Points[2].position.x, Control_Points[2].position.y), new Vector2(Control_Points[3].position.x, Control_Points[3].position.y));
        }
        else
        {
            // for the wafflecunt who doesnt like beziers
            
            
            for (float t = 0; t <= 1; t += 5 / 100f)
            {
               
                
                gizmosPOS = Vector2.Lerp(Control_Points[0].position, Control_Points[1].position, t);

                Gizmos.DrawWireSphere(gizmosPOS, 1 / 4f);





            }
        }
    }


    
}
