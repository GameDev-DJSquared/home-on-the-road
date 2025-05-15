using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightAnimation : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;
    public LayerMask targetLayerMask;
    bool saveLight = false;
    [SerializeField] Light flashlightLight;
    InventoryScript inventoryScript;

    public void TurnOnHitbox()
    {
        //boxCollider.enabled = true;
        if(flashlightLight.enabled)
        {
            flashlightLight.enabled = false;
            saveLight = true;
        }


        Collider[] hits = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size, transform.rotation, targetLayerMask);
        foreach (var hit in hits)
        {
            //Debug.Log(Time.time + ": Hit " + hit.name);
            if(hit.TryGetComponent(out EnemyScript enemy))
            {
                inventoryScript.DamageWeapon();
                enemy.Hurt(transform.position);
            }
        }
    }

    public void TurnOffHitbox()
    {
        //boxCollider.enabled = false;

        //if(saveLight)
        //{
        //    flashlightLight.enabled = true;

        //}
    }


    // Start is called before the first frame update
    void Start()
    {
        inventoryScript = FindObjectOfType<InventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
