using UnityEngine;

public class SecretItem : InteractableObject
{
    [Header("Configuración")]
    [Tooltip("Si es True, el objeto desaparece al recogerlo. Si es False, se queda visible.")]
    public bool destroyOnInteract = true;

    [Header("Feedback Visual")]
    public GameObject pickupEffect; // Partículas (Opcional)

    [Header("Audio")]
    [Tooltip("Sonido al recoger el objeto secreto.")]
    public AudioClip pickupSound; // <--- NUEVO

    // Variable interna para evitar sumar puntos infinitos si no destruimos el objeto
    private bool isCollected = false;

    public override void Interact()
    {
        // Si ya lo recogimos y decidimos no destruirlo, no hacemos nada
        if (isCollected) return;

        // 1. Sumar al contador global
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.CollectSecretItem();
        }
        else
        {
            Debug.LogWarning("¡Falta el GlobalGameManager en la escena!");
        }

        // 2. Efecto visual (si tienes asignado uno)
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        // 3. REPRODUCIR SONIDO (NUEVO)
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // 4. Lógica de destrucción vs. permanencia
        if (destroyOnInteract)
        {
            Destroy(gameObject);
        }
        else
        {
            // Si no se destruye, marcamos como recogido
            isCollected = true;
        }
    }
}