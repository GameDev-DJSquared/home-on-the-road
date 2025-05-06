using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName ="Item")]
public class Item : ScriptableObject
{
    new public string name;
    public GameObject prefab;
    public float weight = 1;
    public int inventorySize = 1;
    public int value = 1;
    public Type type;
    public Sprite image;

    public enum Type {
        Food,
        Other
    }
}
