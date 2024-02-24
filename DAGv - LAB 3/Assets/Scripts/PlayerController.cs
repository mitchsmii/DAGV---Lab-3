using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //defines all the necessary variables

    // these 4 are for the different levels of move speed required within the game
    [SerializeField] private float originalMoveSpeed = 5f;
    [SerializeField] private float sprintMoveSpeed = 10f;
    [SerializeField] private float crouchMoveSpeed = 2f;
    [SerializeField] private float moveSpeed;

    //Determines how high the player will jump 
    [SerializeField] private float jumpForce = 5f;

    // sets the gravity
    [SerializeField] private float gravity = 9.81f;

    // References to the CharacterController component within Unity
    private CharacterController controller;

    // Uses a built-in vector3 to determine the movedirection variable
    public Vector3 moveDirection;

    // Creates a boolean variable to see if the player is jumping
    private bool isJumping;

    // Sets the original and crouch height of the player
    private float originalHeight;
    [SerializeField] private float crouchHeight = 0.5f;

    private bool isCrouching = false;

    // creates a float variable for x and y, along with a rotate and sensitivity variable for looking around
    private float x;
    private float y;
    public float sensitivity = -1f;
    private Vector3 rotate;


// The start function begins
    void Start()
    {
        // connects the controller variable to the component CharacterController within Unity
        controller = GetComponent<CharacterController>();

        // sets the player height to the original height
        originalHeight = controller.height;

        // sets the move speed to the original move speed
        moveSpeed = originalMoveSpeed;

        // this gets rid of the cursor in the game
        Cursor.lockState = CursorLockMode.Locked;
    }

    // The update function, which does this every frame
    void Update()
    {
        // gets the input from the user for Horizontal and Vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // this moves the object forward, backward, left and right using addition and multiplication of the input and transform
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        moveDirection.x = move.x * moveSpeed;
        moveDirection.z = move.z * moveSpeed;

        // checks to see if the player is on the ground
        if (controller.isGrounded) {
            // checks to see if the user is jumping
            if (isJumping) {

                // if they are, moves the player up equal to the amount of jumpForce
                moveDirection.y = jumpForce;

                //resets the isJumping variable to false
                isJumping = false;
            }
        } else {
            // this slowly moves the player down each frame
            moveDirection.y -= gravity * Time.deltaTime;
        }
        // This moves the player
            controller.Move(moveDirection * Time.deltaTime);

        // checks to see if the jump button is pressed, the player is grounded, and it not crouching
        if (Input.GetButtonDown("Jump") && controller.isGrounded && !isCrouching) {
            // sets the isjumping variable to true
            isJumping = true;
        }

        // Checks to see if the Left Shift button is pressed down
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching) {

            //if it is, sets the moveSpeed to the sprintMoveSpeed variable
            moveSpeed = sprintMoveSpeed;
        }

        // checks to see if the LeftShift key is not pressed
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            
            // returns the speed back to normal if it is not pressed
            moveSpeed = originalMoveSpeed;
        }

        // checks to see if they left control key is pressed down
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            // if it is, runs the crouch function
            Crouch();

            // if it's not, it runs the StandUp function
        } else if (Input.GetKeyUp(KeyCode.LeftControl)) {
            StandUp();
        }

        // Allows the user to use their mouse in order to look around
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotate = new Vector3(x,y * sensitivity, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;

    }

    // this allows the user to interact with the Trampoline object and jump higher with it
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trampoline")
        {
            Debug.Log("Enter");
            moveDirection.y = 15;
        }
    }


        // The Crouch function, which changes the height of the player
        void Crouch() 
        {
            if (!isCrouching) {
                controller.height = crouchHeight;
                isCrouching = true;
                moveSpeed = crouchMoveSpeed;
            }
        }
        
        // the StandUp function, which returns the player to the original height
        void StandUp() 
        {
            if (isCrouching) {
                controller.height = originalHeight;
                isCrouching = false;
                moveSpeed = originalMoveSpeed;
            }
        }

     
    }
