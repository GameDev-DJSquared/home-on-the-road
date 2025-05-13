using UnityEngine;

public class StepClimb : MonoBehaviour
{
    public float stepHeight = 0.3f;             // Max step height
    public float stepSmooth = 0.1f;             // Smoothing factor for stepping up
    public float rayDistance = 0.5f;            // Distance in front of player to check
    public LayerMask groundLayer;               // The layer of the ground/steps

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * -1f;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * rayDistance, Color.cyan);
        // Lower ray detects obstacle
        if (Physics.Raycast(origin, direction, rayDistance, groundLayer))
        {
            Debug.Log("yipee");
            // Upper ray checks if space above obstacle is clear
            Vector3 upperOrigin = transform.position + Vector3.up * stepHeight;
            if (!Physics.Raycast(upperOrigin, direction, rayDistance, groundLayer))
            {
                // Apply a small upward movement to climb the step
                rb.position += new Vector3(0f, stepSmooth, 0f);
            }
        }
    }
}
