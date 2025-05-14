using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerBurnScript : MonoBehaviour
{
    PlayerMover playerMover;
    EnemyScript enemyScript;
    [SerializeField] float maxDistance = 3f;
    RaycastHit hit;
    public static LineOfSight lineOfSight;

    // Start is called before the first frame update
    void Start()
    {
        playerMover = GameObject.FindObjectOfType<PlayerMover>();
        enemyScript = GetComponent<EnemyScript>();
        if (lineOfSight == null)
        {
            lineOfSight = FindObjectOfType<LineOfSight>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyScript.InRange())
        {
            Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.magenta);
            if(Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    playerMover.SlowDown();

                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        
    }
}
