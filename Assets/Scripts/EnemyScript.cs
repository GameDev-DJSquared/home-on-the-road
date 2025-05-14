using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] Transform target;
    static PlayerHealth playerHealth;
    static LineOfSight lineOfSight;
    static InventoryScript inventoryScript;
    Seeker seeker;
    Rigidbody rb;
    AudioSource audioSource;

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

    [Header("Health")]
    [SerializeField] float healthI = 100;
    float health;
    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            
        }

        if(playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
            lineOfSight = FindObjectOfType<LineOfSight>();
            inventoryScript = FindObjectOfType<InventoryScript>();
        }


        InvokeRepeating("UpdatePath", 0f, 1f);
        seeker.StartPath(rb.position, target.position, OnPathComplete);

        health = healthI;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped)
        {
            audioSource.Stop();
            return;
        }



        float distFromPlayer  = Vector3.Distance(rb.position, target.position);

        if(distFromPlayer > seekingDistance)
        {
            audioSource.Stop();
            return;
        } else if(lineOfSight.InLineOfSight(transform) && inventoryScript.FlashlightOn())
        {
            Vector3 dir = -(target.position - rb.position);
            dir.y = 0;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);

        };



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
                if(!audioSource.isPlaying)
                {
                    audioSource.Play();

                }

            }
            else
            {
                playerHealth.Hurt();
                audioSource.Stop();
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
        if (!audioSource.isPlaying)
        {
            audioSource.Play();

        }

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
            if (!audioSource.isPlaying)
            {
                audioSource.Play();

            }

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

    public bool InRange()
    {
        return (Vector3.Distance(rb.position, target.position) <= seekingDistance);
    }


    public void Hurt(Vector3 source)
    {
        Vector3 dir = transform.position - source;
        dir.y = 0;
        rb.velocity = dir * 2;
        health -= 30;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {

        // Draw sphere at the end of the ray
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, seekingDistance);
    }

}
