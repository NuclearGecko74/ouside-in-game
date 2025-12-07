using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Config")]
    public float walkSpeed = 4.0f;
    public float gravity = -9.81f;

    [Header("Walking Head Bob Config")]
    public float walkBobSpeed = 14f;
    public float walkBobAmountX = 0.05f;
    public float walkBobAmountY = 0.05f;
    public float walkBobSwayAngle = 5f;

    [Header("Idle Head Bob Config")]
    public float idleBobSpeed = 1.5f;
    public float idleBobAmount = 0.02f;

    [Header("Refs")]
    public Camera playerCamera;
    public float mouseSensitivity = 150f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    private float defaultPosY = 0;
    private float defaultPosX = 0;
    private float timer = 0;
    private float currentTilt = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera != null)
        {
            defaultPosY = playerCamera.transform.localPosition.y;
            defaultPosX = playerCamera.transform.localPosition.x;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleHeadBob();
    }

    void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * walkSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, currentTilt);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleHeadBob()
    {
        if (!controller.isGrounded) return;

        float inputMagnitude = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0.1f) // CAMINANDO
        {
            timer += Time.deltaTime * walkBobSpeed;

            // X e Y (Posición)
            // Reducimos un poco la fórmula matemática para que no sea tan agresiva
            float bobOffsetX = Mathf.Cos(timer) * walkBobAmountX;
            float bobOffsetY = Mathf.Sin(timer * 2) * walkBobAmountY;

            // Z (Rotación/Sway)
            // Aquí estaba el problema de lo "tosco". Lo hacemos más sutil.
            // Solo inclinamos basado en el input horizontal, quitamos el vaivén extra del timer que mareaba
            float tiltTarget = -Input.GetAxis("Horizontal") * walkBobSwayAngle;

            // Suavizado DE ROTACIÓN (Bajamos la velocidad de 5f a 3f para que tarde un poco en inclinar)
            currentTilt = Mathf.Lerp(currentTilt, tiltTarget, Time.deltaTime * 3f);

            Vector3 targetPos = new Vector3(
                defaultPosX + bobOffsetX,
                defaultPosY + bobOffsetY,
                playerCamera.transform.localPosition.z
            );

            // Suavizado DE POSICIÓN (Bajamos de 10f a 6f para simular peso en la cabeza)
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetPos, Time.deltaTime * 6f);
        }
        else // IDLE
        {
            timer += Time.deltaTime * idleBobSpeed;

            // Regresar la inclinación a 0 muy suavemente
            currentTilt = Mathf.Lerp(currentTilt, 0, Time.deltaTime * 3f);

            float breathingOffset = Mathf.Sin(timer) * idleBobAmount;

            Vector3 targetPos = new Vector3(
                defaultPosX,
                defaultPosY + breathingOffset,
                playerCamera.transform.localPosition.z
            );

            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetPos, Time.deltaTime * 2f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic) return;

        if (hit.moveDirection.y < -0.3f) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.linearVelocity = pushDir * 2.0f;
    }
}
