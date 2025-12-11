using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretItem : InteractableObject
{
    [Header("Identificación (Sistema de Memoria)")]
    [Tooltip("Deja esto vacío para generar un ID automático.")]
    public string uniqueID;

    [Header("Configuración")]
    [Tooltip("True: El objeto desaparece al leerlo/recogerlo. False: Se queda ahí (útil para notas re-leibles).")]
    public bool destroyOnInteract = true;

    [Header("Contenido de la Nota (Opcional)")]
    [TextArea(1000, 1000)]
    public string noteContent; // <--- NUEVO: Texto de la nota

    [Header("Feedback")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private bool isCollected = false;

    void Start()
    {
        // 1. GENERAR ID ÚNICO
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = SceneManager.GetActiveScene().name + "" + gameObject.name + "" + transform.position.ToString();
        }

        // 2. VERIFICAR MEMORIA (¿Ya lo recogí antes?)
        if (GlobalGameManager.Instance != null)
        {
            if (GlobalGameManager.Instance.CheckIfCollected(uniqueID))
            {
                // Si ya fue recogido anteriormente:
                if (destroyOnInteract)
                {
                    // Si era destructible, lo borramos para que no aparezca
                    Destroy(gameObject);
                }
                else
                {
                    // Si es persistente (ej. una nota en la mesa), marcamos que ya dio puntos
                    isCollected = true;
                }
            }
        }
    }

    public override void Interact()
    {
        // ---------------------------------------------------------
        // 1. MOSTRAR NOTA (Siempre funciona, aunque ya esté recogido)
        // ---------------------------------------------------------
        if (!string.IsNullOrEmpty(noteContent))
        {
            // Verificamos que exista el controlador de notas antes de llamar
            if (NoteController.instance != null)
            {
                NoteController.instance.ShowNote(noteContent);
            }
            else
            {
                Debug.LogWarning("Intentaste mostrar una nota pero no hay 'NoteController' en la escena.");
            }
        }

        // ---------------------------------------------------------
        // 2. LÓGICA DE COLECCIÓN (Solo si NO ha sido recogido aún)
        // ---------------------------------------------------------

        // Si ya cobramos los puntos de este objeto, no hacemos nada más.
        if (isCollected) return;

        // A. Avisar al Manager
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.CollectSecretItem(uniqueID);
        }

        // B. Feedback Visual
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        // C. Feedback Sonoro
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // D. Decisión Final: ¿Destruir o Marcar?
        if (destroyOnInteract)
        {
            Destroy(gameObject);
        }
        else
        {
            // Marcamos como recogido para que la próxima vez solo muestre el texto
            // y no sume puntos ni haga ruido.
            isCollected = true;
        }
    }
}