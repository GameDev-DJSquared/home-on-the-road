using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    new public string name;
    public GameObject prefab;
    public float weight = 1;
    public int inventorySize = 1;
    public int value = 1;
    public float durability = 100;
    public Type type;
    public Sprite image;

    public enum Type {
        Food,
        Other
    }
}
