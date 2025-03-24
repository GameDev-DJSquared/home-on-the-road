using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 7.5f;
    [SerializeField] float runningSpeed = 11.5f;
    [SerializeField] float jumpSpeed = 8.0f;
    [SerializeField] float gravity = 20.0f;
    [SerializeField] Camera playerCamera;
    [SerializeField] float lookSpeed = 2.0f;
    [SerializeField] float lookXLimit = 90.0f; // Standard FPS limit

    private CharacterController characterController;
    private float rotationX = 0;
    private float rotationY = 0;
    private Vector3 moveDirection = Vector3.zero;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor to center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    void HandleRotation()
    {
        if (!canMove) return;

        // Mouse look
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        rotationY += Input.GetAxis("Mouse X") * lookSpeed;

        // Apply rotation to camera and player
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    void HandleMovement()
    {
        if (!canMove) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float speed = isRunning ? runningSpeed : walkingSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        moveDirection.x = move.x * speed;
        moveDirection.z = move.z * speed;

        // Jumping
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = -0.1f; // Small downward force to stay grounded
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
        playerCamera.transform.position = transform.position;
    }
}
