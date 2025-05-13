using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolomonManager : MonoBehaviour
{
    public GameObject ghost;
    public GameObject stand;

    public bool seen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (seen == true)
        {
            ghost.SetActive(false);
            stand.SetActive(true);
        }
        else {
            ghost.SetActive(true);
            stand.SetActive(false);
        }
    }
}
