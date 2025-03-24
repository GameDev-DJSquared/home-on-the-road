using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 7.5f;
    [SerializeField] float runningSpeed = 11.5f;
    [SerializeField] float jumpHeight = 2.0f;
    [SerializeField] float gravity = -20.0f;
    [SerializeField] Camera playerCamera;
    [SerializeField] float lookSpeed = 2.0f;
    [SerializeField] float lookXLimit = 90.0f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    private CharacterController characterController;
    private float rotationX = 0;
    private float rotationY = 0;
    private Vector3 velocity = Vector3.zero;
    private bool canMove = true;
    bool isGrounded = false;
    Vector3 offset;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        offset = playerCamera.transform.localPosition - transform.position;
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
        velocity.x = move.x * speed;
        velocity.z = move.z * speed;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
        playerCamera.transform.position = transform.position + offset;
    }
}
