using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

[RequireComponent(typeof(ObjectInteractor))]
public class InventoryScript : MonoBehaviour
{

    [SerializeField] int inventoryCapacity = 5;
    [SerializeField] float spawnDistance = 1f;

    

    Item[] slots;

    int selectedSlot = 0;



    private void Awake()
    {
        slots = new Item[inventoryCapacity];
        GetComponent<ObjectInteractor>().OnObjectGrabbed += OnObjectGrabbed;
    }


    private void Update()
    {
        if(InputManager.instance.GetDropPressed())
        {
            DropItem();
        }



        Vector2 scroll = InputManager.instance.GetScrollDir();
        if(scroll != Vector2.zero)
        {
            if (scroll.y > 0)
                selectedSlot = (selectedSlot + 1) % inventoryCapacity;
            else if (scroll.y < 0)
                selectedSlot = (selectedSlot - 1 + inventoryCapacity) % inventoryCapacity;

            Debug.Log("new slot: " + selectedSlot);
        }
    }
    




    public void DropItem()
    {

        if (slots[selectedSlot] != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
            Quaternion spawnRotation = transform.rotation;

            Instantiate(slots[selectedSlot].prefab, spawnPosition, spawnRotation);

            slots[selectedSlot] = null;
        }




    }


    void OnObjectGrabbed(Item item, GameObject go)
    {
        int remainingCapacity = inventoryCapacity - TotalWeight();
        if(remainingCapacity >= item.inventorySize && OpenSlot())
        {


            // ADD THAT SUCKA

            if (slots[selectedSlot] == null)
            {
                slots[selectedSlot] = item;
            } else
            {
                slots[FindSlot()] = item;
            }

            //go.SetActive(false);
            Destroy(go);
        }
    }

    
    int FindSlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i] == null) return i;
        }
        return -1;
    }

    public float WeightCapacity()
    {
        return Mathf.Clamp((float)TotalWeight() / inventoryCapacity, 0, 1);
    }

    public int TotalWeight()
    {
        int total = 0;
        foreach(Item i in slots)
        {
            if(i != null)
                total += i.inventorySize;
        }
        return total;
    }

    public bool OpenSlot()
    {
        foreach(Item i in slots)
        {
            if (i == null) return true;
        }
        return false;
    }

}
