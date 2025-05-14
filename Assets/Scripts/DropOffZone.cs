using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropOffZone : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalText;

    public static List<Item> items = new List<Item>();

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("New Trigger Enter");

        if(col.gameObject.tag == "Interactable")
        {
            GameObject go = col.gameObject;
            if (go.transform.parent != null && go.transform.parent.tag == "Interactable")
            {
                go = go.transform.parent.gameObject;
            }
            if (go.TryGetComponent(out Interactable interact))
            {
                //Debug.Log("Object is interactable");

                Item item = interact.GetItem();
                if (item != null)
                {
                    if(items.Contains(item))
                    {
                        return;
                    }
                    items.Add(item);
                    //Debug.Log("New Total Value: " + GetTotalValue());

                    if (GetTotalValue() == FoodManager.instance.GetTotalPossibleValue())
                    {
                        GameManager.instance.FinishGame(false);
                    }
                }
                UpdateText();

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
        items.Clear();
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int GetTotalValue()
    {
        string s = "";
        int totalValue = 0;
        foreach(Item item in items)
        {
            totalValue += item.value;
            s += item.value + ", ";
        }
        return totalValue;
    }

    public void UpdateText()
    {
        totalText.text = "Quota: \n" + GetTotalValue().ToString("000") + "/" + GameManager.quota.ToString("000");

    }

}
