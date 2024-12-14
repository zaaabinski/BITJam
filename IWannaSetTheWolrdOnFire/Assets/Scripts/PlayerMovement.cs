using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int playerHP=100;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f; // Gravity strength
    public float jumpHeight = 2f;  // Jump height (if you want jumping too)

    [Header("References")]
    public Transform playerCamera;

    private float pitch = 0f;
    private CharacterController characterController;
    private Vector3 velocity; // Used to apply gravity and jumping

    [SerializeField] private FlamethrowerScript flamethowerScript;
    public bool canMove = true;

    void Start()
    {
        // Lock the cursor to the game window and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(canMove)
        {
        HandleMovement();
        HandleMouseLook();
        ApplyGravity();
        }
        if (playerHP <= 0)
        {
            canMove = false;
            Time.timeScale = 0f;
            Debug.Log("You lost");
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Calculate movement direction relative to the player's current orientation
        Vector3 moveDirection = (transform.forward * moveZ + transform.right * moveX).normalized;

        // Apply movement using CharacterController
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * lookSensitivity;

        // Rotate the player (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Adjust the camera's pitch (up/down rotation)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            // If grounded, reset downward velocity (so no continual fall)
            velocity.y = -2f;  // A small value to keep the player grounded, but not floating
        }
        else
        {
            // Apply gravity (falling down)
            velocity.y += gravity * Time.deltaTime;
        }

        // Apply the gravity to the character
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AmmoPickup"))
        {
            flamethowerScript.AddAmmo();
            Destroy(other.gameObject);
        }
    }

    public void DecreseHp(int dmg)
    {
        playerHP -= dmg;
    }

}
