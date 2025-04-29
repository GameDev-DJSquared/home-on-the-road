using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] Transform target;
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
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }


        InvokeRepeating("UpdatePath", 0f, 1f);
        //seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer  = Vector3.Distance(rb.position, target.position);
        if (distFromPlayer <= closeDistance)
        {
            Vector3 dir = (target.position - rb.position).normalized;
            dir.y = 0;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
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

        //rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

        
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
