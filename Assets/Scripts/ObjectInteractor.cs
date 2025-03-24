using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    public string interactableTag = "Interactable"; // Change this to whatever tag you want
    public float interactDistance = 3f; // Adjust interaction range

    private Camera cam;

    void Start()
    {
        cam = Camera.main; // Assumes the main camera is used for interaction
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                if (hit.collider.CompareTag(interactableTag))
                {
                    Debug.Log("Interacted with!");
                }
            }
        }
    }
}
