using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffZone : MonoBehaviour
{
    public static List<Item> items = new List<Item>();

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("New Trigger Enter");
        if(col.TryGetComponent(out Interactable interact))
        {
            Debug.Log("Object is interactable");

            Item item = interact.GetItem();
            if(item != null)
            {
                items.Add(item);
                Debug.Log("New Total Value: " + GetTotalValue());
            }
        }

        
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.TryGetComponent(out Interactable interact))
        {
            Item item = interact.GetItem();
            if (item != null)
            {
                items.Remove(item);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int GetTotalValue()
    {
        int totalValue = 0;
        foreach(Item item in items)
        {
            totalValue += item.value;
        }
        return totalValue;
    }
}
