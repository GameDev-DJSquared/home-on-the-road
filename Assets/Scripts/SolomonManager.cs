using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SolomonManager : MonoBehaviour
{
    public GameObject ghost;
    public GameObject stand;

    public static LineOfSight lineOfSight;
    EnemyScript enemyScript;
    float normalSpeed = 0;

    public bool seen = false;
    public float updateTimeI = 0.2f;
    float updateTime;
    // Start is called before the first frame update
    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        if(lineOfSight == null)
        {
            lineOfSight = FindObjectOfType<LineOfSight>();
        }
        normalSpeed = enemyScript.speed;
        updateTime = updateTimeI;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(updateTime > 0)
        {
            updateTime -= Time.deltaTime;
            if(updateTime <= 0)
            {

                updateTime = updateTimeI;
                //Check if still seen by player

                if(enemyScript.InRange())
                {


                    if (lineOfSight.InLineOfSight(transform))
                    {
                        StopMoving();
                    }
                    else
                    {
                        ResumeMoving();
                    }
                } else
                {
                    if(!seen)
                    {
                        StopMoving();
                    }
                }


                
            }
        }
    }



    public void StopMoving()
    {
        seen = true;
        ghost.SetActive(false);
        stand.SetActive(true);
        enemyScript.stopped = true;
    }

    public void ResumeMoving()
    {
        seen = false;
        ghost.SetActive(true);
        stand.SetActive(false);
        enemyScript.stopped = false;
    }

    
}
