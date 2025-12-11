using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : InteractableObject
{
    [Header("Configuración de Escena")]
    public string nextSceneName;

    [Header("Progreso")]
    public bool countsTowardsWin = false;

    [Header("Sistema de Bloqueo")]
    public bool isLocked = true;
    public string lockedMessage = "Está cerrada.";
    public string unlockedMessage = "Abrir puerta";

    [Header("Audio")]
    [Tooltip("Sonido al intentar abrir si está cerrada (ej. Pomo forzado).")]
    public AudioClip lockedSound; // <--- NUEVO
    [Tooltip("Sonido al abrir la puerta y salir (ej. Puerta chirriando).")]
    public AudioClip openSound;   // <--- NUEVO

    public new string GetDescription()
    {
        return isLocked ? lockedMessage : unlockedMessage;
    }

    public void Unlock()
    {
        isLocked = false;
        Debug.Log("Puerta desbloqueada.");
    }

    public override void Interact()
    {
        // CASO 1: Está Bloqueada
        if (isLocked)
        {
            Debug.Log(lockedMessage);

            // Reproducir sonido de "Bloqueado"
            if (lockedSound != null)
            {
                AudioSource.PlayClipAtPoint(lockedSound, transform.position);
            }
            return;
        }

        // CASO 2: Está Desbloqueada (Salimos)

        // Reproducir sonido de "Abrir"
        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        }

        // Lógica de progreso
        if (countsTowardsWin && GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.CollectSecretItem();
        }

        // Cargar Escena
        CargarEscena();
    }

    void CargarEscena()
    {
        GameObject faderObj = GameObject.FindGameObjectWithTag("BlackScreen");

        if (faderObj != null)
        {
            faderObj.GetComponent<SceneFader>().FadeOutAndLoad(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}