using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    public static FoodManager instance { private set; get; }


    [SerializeField] Slider foodProgress;
    [SerializeField] int totalFoodCount = 17;

    List<GameObject> allPrefabs = new List<GameObject>();
    List<GameObject> enabledPrefabs = new List<GameObject>();

    float totalPossibleValue = 0f;
    


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Two FoodManagers!");
        }
        instance = this;


        ShelfScript[] shelfScripts = GameObject.FindObjectsOfType<ShelfScript>();
        foreach(ShelfScript shelfScript in shelfScripts)
        {
            allPrefabs.AddRange(shelfScript.GetItems());
        }


        List<GameObject> shuffledItems = new List<GameObject>(allPrefabs);
        ShuffleList(shuffledItems);

        for (int i = 0; i < Mathf.Min(totalFoodCount, shuffledItems.Count); i++)
        {
            shuffledItems[i].SetActive(true);
            totalPossibleValue += shuffledItems[i].GetComponent<Interactable>().GetItem().value;
        }





    }

    private void Update()
    {
        foodProgress.value = Mathf.Clamp(DropOffZone.GetTotalValue() / GetTotalPossibleValue(), 0f, 1);
    }

    public float GetTotalPossibleValue()
    {
        return totalPossibleValue;
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
