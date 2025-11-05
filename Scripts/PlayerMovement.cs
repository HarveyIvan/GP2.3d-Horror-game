using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed = 6f;
    public float runSpeed  = 12f;

    [Header("Jump/Gravity")]
    public float jumpHeight = 2.0f;   
    public float gravity    = 20f;    

    [Header("Mouse Look")]
    public Camera playerCamera;       
    public float lookSpeed  = 2.0f;   
    public float lookXLimit = 80f;    

    [Header("Cursor")]
    public bool lockCursorOnStart = true;

    private CharacterController controller;
    private float verticalVel;
    private float rotationX;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (lockCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }
    }

    void Update()
    {
        
        if (playerCamera)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX  = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

            transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
        }

        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed    = isRunning ? runSpeed : walkSpeed;

        
        Vector3 camF = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;
        Vector3 camR = Vector3.ProjectOnPlane(playerCamera.transform.right,   Vector3.up).normalized;

        Vector3 moveXZ = (camF * v + camR * h);
        if (moveXZ.sqrMagnitude > 1f) moveXZ.Normalize();
        moveXZ *= speed;

        
        if (controller.isGrounded)
        {
            if (verticalVel < -2f) verticalVel = -2f; 
            if (Input.GetKeyDown(KeyCode.Space))
                verticalVel = Mathf.Sqrt(2f * gravity * jumpHeight); 
        }
        else
        {
            verticalVel -= gravity * Time.deltaTime;
        }

        Vector3 velocity = new Vector3(moveXZ.x, verticalVel, moveXZ.z);
        controller.Move(velocity * Time.deltaTime);
    }
}
