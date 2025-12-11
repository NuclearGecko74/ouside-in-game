using UnityEngine;

public class OffsetFlashlight : MonoBehaviour
{
    [Header("Configuración")]
    public Transform cameraTransform;
    public float rotationSpeed = 5.0f;

    [Header("Posición")]
    public Vector3 offset;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        transform.position = cameraTransform.TransformPoint(offset);
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraTransform.rotation, rotationSpeed * Time.deltaTime);
    }
}
