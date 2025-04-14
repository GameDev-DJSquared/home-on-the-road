using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ObjectInteractor))]
public class InventoryScript : MonoBehaviour
{

    [SerializeField] int inventoryCapacity = 5;
    [SerializeField] float spawnDistance = 1f;

    List<Item> items = new List<Item>();



    int selectedSlot = -1;



    private void Awake()
    {
        GetComponent<ObjectInteractor>().OnObjectGrabbed += OnObjectGrabbed;
        
    }


    private void Update()
    {
        if(InputManager.instance.GetDropPressed())
        {
            DropItem();
        }
    }

    public void DropItem()
    {
        


        if (items.Count > 0)
        {
            int index = items.Count - 1;

            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
            Quaternion spawnRotation = transform.rotation;

            Instantiate(items[index].prefab, spawnPosition, spawnRotation);

            items.RemoveAt(index);
        }


        //if (selectedSlot == -1)
        //{
        //    Debug.Log(Time.time + ": no item to drop");
        //    return;
        //}




    }


    void OnObjectGrabbed(Item item, GameObject go)
    {
        int remainingCapacity = inventoryCapacity - items.Sum(x => x.inventorySize);
        if(remainingCapacity >= item.inventorySize)
        {


            // ADD THAT SUCKA

            items.Add(item);
            go.SetActive(false);
            
        }
    }

    


    public float WeightCapacity()
    {
        return (float)items.Sum(x => x.inventorySize) / inventoryCapacity;
    }


}
