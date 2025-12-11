using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para saber en qué escena estamos

public class SecretItem : InteractableObject
{
    [Header("Identificación (Sistema de Memoria)")]
    [Tooltip("Deja esto vacío para generar un ID automático basado en posición y escena.")]
    public string uniqueID; 

    [Header("Configuración")]
    public bool destroyOnInteract = true;

    [Header("Feedback")]
    public GameObject pickupEffect; 
    public AudioClip pickupSound; 

    private bool isCollected = false;

    void Start()
    {
        // 1. GENERAR ID ÚNICO (Si está vacío)
        // Creamos un nombre único combinando: NombreEscena + NombreObjeto + Posición(X,Y,Z)
        // Esto asegura que cada objeto sea único en todo el juego.
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = SceneManager.GetActiveScene().name + "_" + gameObject.name + "_" + transform.position.ToString();
        }

        // 2. VERIFICAR MEMORIA
        // Preguntamos al Manager: "¿Ya me recogieron antes?"
        if (GlobalGameManager.Instance != null)
        {
            if (GlobalGameManager.Instance.CheckIfCollected(uniqueID))
            {
                // Si ya fuimos recogidos, nos destruimos o desactivamos al instante
                if (destroyOnInteract)
                {
                    Destroy(gameObject);
                }
                else
                {
                    // Si la configuración es no destruir, marcamos como recogido
                    // y quizás apagamos el render o el collider
                    isCollected = true;
                    // gameObject.SetActive(false); // Descomenta si prefieres que desaparezca totalmente
                }
            }
        }
    }

    public override void Interact()
    {
        if (isCollected) return;

        // 1. Avisar al Manager (Enviando nuestro ID Único)
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.CollectSecretItem(uniqueID);
        }

        // 2. Feedback (Visual y Sonoro)
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // 3. Destrucción
        if (destroyOnInteract)
        {
            Destroy(gameObject);
        }
        else
        {
            isCollected = true;
        }
    }
}