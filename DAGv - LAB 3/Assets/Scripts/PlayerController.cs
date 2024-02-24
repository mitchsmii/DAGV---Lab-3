using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float originalMoveSpeed = 5f;
    [SerializeField] private float sprintMoveSpeed = 10f;
    [SerializeField] private float crouchMoveSpeed = 2f;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isJumping;


    // Crouch variables
    private float originalHeight;
    [SerializeField] private float crouchHeight = 0.5f;
    private bool isCrouching = false;
    private float x;
    private float y;
    public float sensitivity = -1f;
    private Vector3 rotate;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height; // Store the original height
        moveSpeed = originalMoveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement based on input and moveSpeed, affecting only x and z axes
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        moveDirection.x = move.x * moveSpeed;
        moveDirection.z = move.z * moveSpeed;

        if (controller.isGrounded) {
            if (isJumping) {
                moveDirection.y = jumpForce; // Apply the jump force
                isJumping = false; // Reset jumping flag
            }
        } else {
            moveDirection.y -= gravity * Time.deltaTime; // Apply gravity
        }

        controller.Move(moveDirection * Time.deltaTime); // Move the player

        // Jump input
        if (Input.GetButtonDown("Jump") && controller.isGrounded && !isCrouching) {
            isJumping = true;
        }

        // Sprint input
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching) {
            moveSpeed = sprintMoveSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            moveSpeed = originalMoveSpeed;
        }

        // Crouch input
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            Crouch();
        } else if (Input.GetKeyUp(KeyCode.LeftControl)) {
            StandUp();
        }

        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotate = new Vector3(x,y * sensitivity, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;

    }
    void OnCollisionEnter(Collision collision){
         if (collision.gameObject.name == "Jumppad"){
            moveDirection.y = 15;
         }
    }
    void Crouch() {
        if (!isCrouching) {
            controller.height = crouchHeight;
            isCrouching = true;
            moveSpeed = crouchMoveSpeed;
            // Optionally, adjust the center of the CharacterController if needed
            // controller.center = new Vector3(controller.center.x, crouchHeight / 2, controller.center.z);
        }
    }

    void StandUp() {
        if (isCrouching) {
            controller.height = originalHeight;
            isCrouching = false;
            moveSpeed = originalMoveSpeed;
            // Reset the center of the CharacterController if it was adjusted during crouch
            // controller.center = new Vector3(controller.center.x, originalHeight / 2, controller.center.z);
        }
    }
}
