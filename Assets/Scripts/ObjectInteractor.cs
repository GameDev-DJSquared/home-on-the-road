using System;
using UnityEngine;

/// <summary>
/// This script handles the actual ability to pick up objects
/// </summary>
public class ObjectInteractor : MonoBehaviour
{
    public string interactableTag = "Interactable"; // Change this to whatever tag you want
    public float interactDistance = 3f; // Adjust interaction range

    Camera cam;
    GameObject highlightedObj;
    public Action<Item, GameObject> OnObjectGrabbed;
    Ray ray;

    void Start()
    {
        cam = Camera.main; // Assumes the main camera is used for interaction
    }

    void Update()
    {
        ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.magenta, 1.5f);


        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag(interactableTag))
            {

                GameObject go = hit.collider.gameObject;
                if (go.transform.parent != null && go.transform.parent.tag == interactableTag)
                {
                    go = go.transform.parent.gameObject;
                }

                if (go.TryGetComponent(out Interactable interact))
                {
                    
                    if(highlightedObj != null && highlightedObj != go)
                    {
                        highlightedObj.GetComponent<Interactable>().TurnOffOutline();
                    }
                    interact.TurnOnOutline();
                    highlightedObj = interact.gameObject;

                    if (InputManager.instance.GetInteractPressed())
                    {
                        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.green, 1.5f);

                        OnObjectGrabbed?.Invoke(interact.GetItem(), interact.gameObject);

                    }
                }



                
            }
            else
            {
                if (highlightedObj != null)
                {
                    if (highlightedObj.TryGetComponent<Interactable>(out Interactable i))
                    {
                        i.TurnOffOutline();

                    }
                    highlightedObj = null;
                }

            }
        }
        else
        {
            if (highlightedObj != null)
            {
                if (highlightedObj.TryGetComponent<Interactable>(out Interactable i))
                {
                    i.TurnOffOutline();

                }
                highlightedObj = null;
            }

        }



    }



}
