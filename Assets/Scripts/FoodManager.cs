using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] int totalFoodCount = 10;

    List<GameObject> allItems = new List<GameObject>();


    private void Start()
    {
        ShelfScript[] shelfScripts = GameObject.FindObjectsOfType<ShelfScript>();
        foreach(ShelfScript shelfScript in shelfScripts)
        {
            allItems.AddRange(shelfScript.GetItems());
        }


        List<GameObject> shuffledItems = new List<GameObject>(allItems);
        ShuffleList(shuffledItems);

        for (int i = 0; i < Mathf.Min(totalFoodCount, shuffledItems.Count); i++)
        {
            shuffledItems[i].SetActive(true);
        }

    }



    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

}
