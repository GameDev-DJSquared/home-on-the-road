using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] Transform target;
    PlayerHealth playerHealth;
    Seeker seeker;
    Rigidbody rb;

    public float speed = 200f;
    public float seekingDistance = 5f;
    public float closeDistance = 1f;
    public float nextWaypointDistance = 3f;
    public bool stopped = false;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    public float rotationSpeed = 5f;
    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerHealth = FindObjectOfType<PlayerHealth>();
        }


        InvokeRepeating("UpdatePath", 0f, 1f);
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped)
            return;


        float distFromPlayer  = Vector3.Distance(rb.position, target.position);
        if (distFromPlayer <= closeDistance)
        {
            


            Vector3 dir = (target.position - rb.position);
            dir.y = 0;

            if (dir.magnitude > 0.01f)
            {
                Vector3 move = dir.normalized * speed * Time.deltaTime;
                if (move.magnitude > dir.magnitude)
                    move = dir; // don't overshoot

                rb.MovePosition(rb.position + move);
            } else
            {
                playerHealth.Hurt();
            }
            return;
        }


        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }


        Vector3 direction = (path.vectorPath[currentWaypoint] - rb.position).normalized;
        direction.y = 0;

        float distance = Vector3.Distance(rb.position, path.vectorPath[currentWaypoint]);

        //Debug.Log("distance: " + distance + "\npath: " + path);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        
        // Calculate movement direction (difference in position)
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;

        if (velocity.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        lastPosition = transform.position;

        
        if(distFromPlayer <= seekingDistance)
        {

            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
    }




    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }

    }


}
