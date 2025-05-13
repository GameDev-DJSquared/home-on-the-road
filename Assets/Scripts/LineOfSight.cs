using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    // public GameObject target; // Assign the target object in the Unity editor
    private SolomonManager solMan;
    private EnemyScript enemyScript;

    void Start()
    {
        solMan = FindObjectOfType<SolomonManager>();
        enemyScript = FindObjectOfType<EnemyScript>();
    }

    // void Update()
    // {
    //     Debug.Log(IsTargetVisible());
    //     if (IsTargetVisible())
    //     {
    //         solMan.seen = true;
    //     }
    //     else
    //     {
    //         solMan.seen = false;
    //     }
    // }

    // public bool IsTargetVisible()
    // {
    //     // Create a ray from the player to the target
    //     RaycastHit hit;
    //     Vector3 direction = target.transform.position - transform.position;
    //     Ray ray = new Ray(transform.position, direction.normalized);

    //     // Perform the raycast
    //     if (Physics.Raycast(ray, out hit, direction.magnitude))
    //     {
    //         // Check if the hit object is the target
    //         if (hit.transform == target.transform)
    //         {
    //             return true; // The target is visible
    //         }
    //         else
    //         {
    //             return false; // The target is blocked by an obstacle
    //         }
    //     }

    //     return false; // The raycast did not hit anything
    // }

   
    public Transform playerCamera;      // Where vision originates (e.g., camera or head)
    public Transform target;            // Object to detect
    public float viewRadius = 10f;      // Vision distance
    [Range(0, 360)] public float viewAngle = 90f; // Vision angle (FOV)
    public LayerMask obstructionMask;   // What counts as blocking vision

    private float timeSeen = 0.3f;

    void Update()
    {
        Vector3 directionToTarget = (target.position - playerCamera.position).normalized;
        float distanceToTarget = Vector3.Distance(playerCamera.position, target.position);

        
            // Check if within angle
            float angleToTarget = Vector3.Angle(playerCamera.forward, directionToTarget);

            if (angleToTarget <= viewAngle / 2f)
            {
                // Check if target is visible via raycast
                if (!Physics.Raycast(playerCamera.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    timeSeen -= Time.deltaTime;
                    if (timeSeen <= 0)
                    {
                        solMan.seen = true;
                        enemyScript.speed = 0f; 
                        timeSeen = 0.3f;
                    }
                    Debug.Log("Target is in FOV and visible!");
                }
                else
                {
                    solMan.seen = false;
                    enemyScript.speed = 4.5f;
                    Debug.Log("Target is in FOV but obstructed.");
                }
            }
            else
            {
                solMan.seen = false;
                enemyScript.speed = 4.5f;
                Debug.Log("Target is outside FOV angle.");
            }
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
