using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalObjective : InteractableObject
{
    [Header("Configuración de Salida")]
    public string winSceneName = "WinScene"; // Nombre de la escena a cargar
    public bool isLocked = true; // ¿Empieza bloqueado?

    [Header("Mensajes")]
    public string lockedMessage = "Necesito encontrar la llave primero...";
    public string unlockedPrompt = "Escapar";

    [Header("Audio")]
    [Tooltip("Sonido si intentas usarlo y está bloqueado.")]
    public AudioClip lockedSound; // <--- NUEVO
    [Tooltip("Sonido al desbloquear/escapar exitosamente.")]
    public AudioClip successSound; // <--- NUEVO

    // Sobrescribimos GetDescription para mostrar mensajes distintos según el estado
    public new string GetDescription()
    {
        if (isLocked)
            return lockedMessage; // Mensaje por defecto (ej: "Cerrado")
        else
            return unlockedPrompt; // Mensaje cuando ya tienes la llave
    }

    // Este método lo llama la llave (UnlockKey)
    public void UnlockExit()
    {
        isLocked = false;
        Debug.Log("¡Salida desbloqueada!");
    }

    public override void Interact()
    {
        // CASO 1: Está Bloqueado
        if (isLocked)
        {
            Debug.Log(lockedMessage);

            // Reproducir sonido de bloqueo
            if (lockedSound != null)
            {
                AudioSource.PlayClipAtPoint(lockedSound, transform.position);
            }
            return;
        }

        // CASO 2: Está Desbloqueado (Ganaste)

        // Reproducir sonido de éxito
        if (successSound != null)
        {
            AudioSource.PlayClipAtPoint(successSound, transform.position);
        }

        // Cargar la escena de victoria
        LoadWinScene();
    }

    void LoadWinScene()
    {
        // Buscamos el SceneFader existente para mantener consistencia visual
        GameObject faderObj = GameObject.FindGameObjectWithTag("BlackScreen");

        if (faderObj != null)
        {
            SceneFader fader = faderObj.GetComponent<SceneFader>();
            fader.FadeOutAndLoad(winSceneName);
        }
        else
        {
            // Fallback por si no hay fader
            SceneManager.LoadScene(winSceneName);
        }
    }
}