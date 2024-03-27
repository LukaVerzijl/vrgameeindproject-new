using Fusion;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _controller;
    private ThirdPersonCamera ThirdPersonCamera;
    //player movement variables
    public float moveSpeed = 2f;

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

        MyInput();
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //calc move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
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

            Debug.Log(playerObj.name);
            Debug.Log(orientation.name);
            Debug.Log(player.name);
            Debug.Log(RB);



            camObj = GameObject.Find("FreeLook Camera");
            virtualCamera = camObj.GetComponent<CinemachineFreeLook>();

            //define the third person script
            playerCam = GameObject.Find("PlayerCam");
            ThirdPersonCamera = playerCam.GetComponent<ThirdPersonCamera>();

            //Set targets Third person camera script
            ThirdPersonCamera.playerObj = playerObj.transform;
            ThirdPersonCamera.player = player.transform;
            ThirdPersonCamera.orientation = orientationObj.transform;
            ThirdPersonCamera.rb = RB;

            ThirdPersonCamera.enabled = true;
            //ThirdPersonCamera.playerObj = transform.parent.GetChild(0);

            Debug.Log(playerCam.name);



            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;


        }
    }


}