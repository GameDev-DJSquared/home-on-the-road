using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    public string interactableTag = "Interactable"; // Change this to whatever tag you want
    public float interactDistance = 3f; // Adjust interaction range

    Camera cam;
    GameObject highlightedObj;

    void Start()
    {
        cam = Camera.main; // Assumes the main camera is used for interaction
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                highlightedObj = hit.collider.gameObject;

                if(highlightedObj.TryGetComponent<Interactable>(out Interactable i))
                {
                    i.TurnOnOutline();
                }


                if (InputManager.instance.GetInteractPressed())
                {
                    Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.green, 1.5f);
                    
                    Debug.Log(Time.time + ": Interacted with!");

                }
            }
        } else
        {
            if(highlightedObj != null)
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
