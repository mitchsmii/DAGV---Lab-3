using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isJumping;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement based on input and moveSpeed, but only affect x and z axes
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        moveDirection.x = move.x * moveSpeed;
        moveDirection.z = move.z * moveSpeed;

        if (controller.isGrounded) {
            if (isJumping) {
                // Apply the jump force if jumping
                moveDirection.y = jumpForce;
                isJumping = false; // Reset the jumping flag
            }
        } else {
            // Apply gravity if not grounded
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the player
        controller.Move(moveDirection * Time.deltaTime);

        // Check for jump input after performing movement to ensure jump is processed next frame
        if (Input.GetButtonDown("Jump") && controller.isGrounded) {
            isJumping = true;
        }
    }
}
