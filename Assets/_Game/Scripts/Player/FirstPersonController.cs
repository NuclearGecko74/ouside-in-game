using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Config")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Crouching Config")]
    [SerializeField] private float crouchHeight = 0.3f;
    [SerializeField] private float standHeight = 1f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Camera Config")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float lookXLimit = 85f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;

    private bool isCrouching = false;
    private Vector3 targetCenter;
    private float targetHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        characterController.height = standHeight;
        characterController.center = new Vector3(0, standHeight / 2, 0);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseX, 0);

        rotationX += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        if (Input.GetKey(crouchKey))
        {
            isCrouching = true;
            targetHeight = crouchHeight;
        }
        else
        {
            isCrouching = false;
            targetHeight = standHeight;
        }

        float lastHeight = characterController.height;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, timeToCrouch * 5 * Time.deltaTime);

        Vector3 newCenter = characterController.center;
        newCenter.y = characterController.height / 2f;
        characterController.center = newCenter;

        Vector3 camPos = playerCamera.transform.localPosition;
        camPos.y = characterController.height * 0.9f;
        playerCamera.transform.localPosition = camPos;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrouching;

        float currentSpeedX = 0;
        float currentSpeedY = 0;

        if (canMove)
        {
            float inputSpeed = walkSpeed;
            if (isCrouching) inputSpeed = crouchSpeed;
            else if (isRunning) inputSpeed = runSpeed;

            currentSpeedX = inputSpeed * Input.GetAxis("Vertical");
            currentSpeedY = inputSpeed * Input.GetAxis("Horizontal");
        }

        float movementDirectionY = moveDirection.y;
        moveDirection = (transform.forward * currentSpeedX) + (transform.right * currentSpeedY);

        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump") && canMove && !isCrouching)
            {
                moveDirection.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                moveDirection.y = -1f;
            }
        }
        else
        {
            moveDirection.y = movementDirectionY + (gravity * Time.deltaTime);
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
}
