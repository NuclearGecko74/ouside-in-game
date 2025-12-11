using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretItem : InteractableObject
{
    [Header("Identificación")]
    public string uniqueID;

    [Header("Configuración")]
    public bool destroyOnInteract = true;

    [Header("Nota")]
    [TextArea(10, 20)]
    public string noteContent;

    [Tooltip("Deja en 0 para usar el tamaño GLOBAL del NoteController. Pon un número para cambiarlo solo en esta nota.")]
    public float customFontSize = 0;

    [Header("Feedback")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private bool isCollected = false;

    void Start()
    {
        // Generar ID automático
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = SceneManager.GetActiveScene().name + "_" + gameObject.name + "_" + transform.position.ToString();
        }

        // Verificar memoria del GlobalGameManager
        if (GlobalGameManager.Instance != null && GlobalGameManager.Instance.CheckIfCollected(uniqueID))
        {
            if (destroyOnInteract) Destroy(gameObject);
            else isCollected = true;
        }
    }

    public override void Interact()
    {
        // 1. Mostrar Nota
        if (!string.IsNullOrEmpty(noteContent))
        {
            if (NoteController.instance != null)
            {
                // Enviamos el contenido y el tamaño (0 usa el global)
                NoteController.instance.ShowNote(noteContent, customFontSize);
            }
        }

        if (isCollected) return;

        // 2. Lógica de recolección
        if (GlobalGameManager.Instance != null)
            GlobalGameManager.Instance.CollectSecretItem(uniqueID);

        if (pickupEffect != null) Instantiate(pickupEffect, transform.position, Quaternion.identity);
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        if (destroyOnInteract) Destroy(gameObject);
        else isCollected = true;
    }
}