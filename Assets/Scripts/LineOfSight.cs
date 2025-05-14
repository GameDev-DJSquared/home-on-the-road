using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public static LineOfSight instance { get; private set; }




    void Start()
    {
        if(instance != null)
        {
            Debug.LogWarning("Two LineOfSight Scripts Detected!");

        }
        instance = this;

    }

    
    public Transform playerCamera;      // Where vision originates (e.g., camera or head)
    public float viewRadius = 10f;      // Vision distance
    [Range(0, 360)] public float viewAngle = 90f; // Vision angle (FOV)
    public LayerMask obstructionMask;   // What counts as blocking vision

    private float timeSeen = 0.3f;

    //void Update()
    //{
            


    //    Vector3 directionToTarget = (target.position - playerCamera.position).normalized;
    //    float distanceToTarget = Vector3.Distance(playerCamera.position, target.position);

        
    //    // Check if within angle
    //    float angleToTarget = Vector3.Angle(playerCamera.forward, directionToTarget);

    //    if (angleToTarget <= viewAngle / 2f)
    //    {
    //        // Check if target is visible via raycast
    //        if (!Physics.Raycast(playerCamera.position, directionToTarget, distanceToTarget, obstructionMask))
    //        {
    //            timeSeen -= Time.deltaTime;
    //            if (timeSeen <= 0)
    //            {
    //                solMan.StopMoving();    
    //                //solMan.seen = true;
    //                //enemyScript.speed = 0f; 
    //                timeSeen = 0.3f;
    //            }
    //            //Debug.Log("Target is in FOV and visible!");
    //        }
    //        else
    //        {
    //            solMan.ResumeMoving();
    //            //solMan.seen = false;
    //            //enemyScript.speed = 4.5f;
    //            //Debug.Log("Target is in FOV but obstructed.");
    //        }
    //    }
    //    else
    //    {
    //        solMan.ResumeMoving();
    //        //solMan.seen = false;
    //        //enemyScript.speed = 4.5f;
    //        //Debug.Log("Target is outside FOV angle.");
    //    }
    //}


    public bool InLineOfSight(Transform target)
    {
        Vector3 directionToTarget = (target.position - playerCamera.position).normalized;
        float distanceToTarget = Vector3.Distance(playerCamera.position, target.position);

        // Check if within angle
        float angleToTarget = Vector3.Angle(playerCamera.forward, directionToTarget);

        if (angleToTarget <= viewAngle / 2f)
        {
            
            if (!Physics.Raycast(playerCamera.position, directionToTarget, distanceToTarget, obstructionMask)) {
                //Debug.DrawRay(playerCamera.position, directionToTarget * distanceToTarget, Color.magenta, 1f);
                return true;
            } else
            {
                //Debug.DrawRay(playerCamera.position, directionToTarget * distanceToTarget, Color.red, 1f);

                //Debug.Log("thing inbetween the player and " + name);
            }
        }
        return false;
    }



    void OnDrawGizmosSelected()
    {
        if (playerCamera == null) return;

    
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * playerCamera.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * playerCamera.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCamera.position, playerCamera.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(playerCamera.position, playerCamera.position + rightBoundary * viewRadius);
    }



    

}
