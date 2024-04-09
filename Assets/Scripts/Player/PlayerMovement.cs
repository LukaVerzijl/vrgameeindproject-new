using Fusion;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _controller;
    private ThirdPersonCamera ThirdPersonCamera;
    private Camera mainCam;
    //player movement variables
    [Header("Movement")]
    public float moveSpeed = 2f;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;





    private GameObject camObj;
    CinemachineFreeLook virtualCamera;

    private GameObject playerCam;

    private GameObject playerObj;
    private GameObject orientationObj;
    private GameObject player;
    private Rigidbody RB;
    private GameObject combatLookAt;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    public override void FixedUpdateNetwork()
    {
        // Only move own player and not every other player. Each player controls its own player object.
        if (HasStateAuthority == false)
        {
            return;
        }
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //Input functions
        MyInput();
        SpeedControl();
        MovePlayer();


        //handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }   
        else
        {
            rb.drag = 0;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Debug.Log(readyToJump);
        Debug.Log(grounded);
        //when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Debug.Log("we can jump");


            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        //calc move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground
        if (grounded)
        {
            Debug.Log("move on ground");
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        } else if (!grounded)
        {
            Debug.Log("move in air");
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
            
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        Debug.Log("JUMP!");
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            //define player
            player = this.gameObject;

            //Define playerChildren
            playerObj = player.transform.GetChild(0).gameObject;
            orientationObj = player.transform.GetChild(1).gameObject;
            RB = player.transform.GetComponent<Rigidbody>();
            combatLookAt = orientationObj.transform.GetChild(0).gameObject;

            Debug.Log(playerObj.name);
            Debug.Log(orientationObj.name);
            Debug.Log(player.name);
            Debug.Log(RB);



            camObj = GameObject.Find("FreeLook Camera");
            virtualCamera = camObj.GetComponent<CinemachineFreeLook>();

            //define the third person script
            playerCam = GameObject.Find("PlayerCam");
            ThirdPersonCamera = playerCam.GetComponent<ThirdPersonCamera>();
            mainCam = ThirdPersonCamera.GetComponent<Camera>();
            mainCam.enabled = true;


            //Set targets Third person camera script
            ThirdPersonCamera.playerObj = playerObj.transform;
            ThirdPersonCamera.player = player.transform;
            ThirdPersonCamera.orientation = orientationObj.transform;
            ThirdPersonCamera.rb = RB;

            //Set targets this movement script
            orientation = orientationObj.transform;

            ThirdPersonCamera.enabled = true;
            //ThirdPersonCamera.playerObj = transform.parent.GetChild(0);

            Debug.Log(playerCam.name);



            virtualCamera.Follow = transform;
            virtualCamera.LookAt = combatLookAt.transform;


        }
    }


}