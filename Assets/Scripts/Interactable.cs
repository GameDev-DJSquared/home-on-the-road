using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Goes on Prefabs of GameObjects
/// </summary>
public class Interactable : MonoBehaviour
{
    static Material outlineMaterial;
    Renderer renderer;

    void Awake()
    {
        if(outlineMaterial == null)
        {
            outlineMaterial = Resources.Load<Material>("OutlineMaterial");
            Debug.Log(outlineMaterial.name);
        }


        renderer = GetComponent<Renderer>();
        List<Material> materials = renderer.materials.ToList();
        materials.Add(outlineMaterial);
        renderer.SetMaterials(materials);
    }

    public void TurnOnOutline()
    {

        renderer.materials[renderer.materials.Length - 1].SetFloat("_Alpha", 1);

    }

    public void TurnOffOutline()
    {
        renderer.materials[renderer.materials.Length - 1].SetFloat("_Alpha", 0);

    }
}
