using Unity.VisualScripting;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 7.5f;
    float speed = 0;
    [SerializeField] float runningSpeed = 11.5f;
    [SerializeField] float speedPickUpRate = 0.001f;
    [SerializeField]
    [Range(0f, 1f)] float weightMaxMult = 0.25f;
    [SerializeField] float jumpHeight = 2.0f;
    [SerializeField] float gravity = -20.0f;
    [SerializeField] Camera playerCamera;
    [SerializeField] float lookSpeed = 2.0f;
    [SerializeField] float lookXLimit = 90.0f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    CharacterController characterController;
    InventoryScript inventoryScript;
    private float rotationX = 0;
    private float rotationY = 0;
    private Vector3 velocity = Vector3.zero;
    private bool canMove = true;
    bool isGrounded = false;
    Vector3 offset;

    float moveX, moveZ;
    bool isRunning = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inventoryScript = GetComponent<InventoryScript>();
        offset = playerCamera.transform.localPosition - transform.position;
        // Lock cursor to center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(GameManager.instance.paused)
        {
            return;
        }
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

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        //speed = isRunning ? Mathf.Clamp(speed, speed + 0.1f runningSpeed) : walkingSpeed;
        



        float weightEffect = 1 - (1 - weightMaxMult) * inventoryScript.WeightCapacity();
        float changedSpeed = speed * weightEffect;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        velocity.x = move.x * changedSpeed;
        velocity.z = move.z * changedSpeed;

        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            speed = runningSpeed;
        }

        velocity.y += gravity * Time.deltaTime;


        //rb.velocity = velocity;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if(isRunning)
        {
            speed = Mathf.Clamp(speed + speedPickUpRate, 0, runningSpeed);
        } else
        {
            speed = walkingSpeed;
        }
    }


    private void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
        playerCamera.transform.position = transform.position + offset;
    }

    
}
