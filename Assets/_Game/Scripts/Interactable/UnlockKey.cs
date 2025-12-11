using UnityEngine;

public class UnlockKey : InteractableObject
{
    [Header("Objetivo a Desbloquear (Usa UNO de los dos)")]
    [Tooltip("Arrastra aquí si es una puerta normal de cambio de nivel.")]
    public LevelExit levelDoor;

    [Tooltip("Arrastra aquí si es la puerta del FINAL del juego.")]
    public FinalObjective finalDoor;

    [Header("Audio")]
    [Tooltip("Sonido al recoger la llave.")]
    public AudioClip keySound;

    public override void Interact()
    {
        bool used = false;

        // OPCIÓN 1: Es una puerta de nivel normal
        if (levelDoor != null)
        {
            levelDoor.Unlock();
            used = true;
        }
        // OPCIÓN 2: Es la puerta final
        else if (finalDoor != null)
        {
            finalDoor.UnlockExit();
            used = true;
        }

        if (used)
        {
            Debug.Log("Llave utilizada con éxito.");

            // Reproducir sonido (PlayClipAtPoint sobrevive al Destroy)
            if (keySound != null)
            {
                AudioSource.PlayClipAtPoint(keySound, transform.position);
            }

            // Destruir la llave
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("¡ERROR! No has asignado NINGUNA puerta (ni LevelExit ni FinalObjective) en esta llave.");
        }
    }
}