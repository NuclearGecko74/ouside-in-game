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
    [Tooltip("Sonido al intentar abrir si está cerrada.")]
    public AudioClip lockedSound;
    [Tooltip("Sonido al abrir la puerta y salir.")]
    public AudioClip openSound;

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

            if (lockedSound != null)
            {
                AudioSource.PlayClipAtPoint(lockedSound, transform.position);
            }
            return;
        }

        // CASO 2: Está Desbloqueada (Salimos)

        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        }

        // --- CORRECCIÓN DEL ERROR ---
        if (countsTowardsWin && GlobalGameManager.Instance != null)
        {
            // Generamos un ID único para esta puerta usando el nombre de la escena y el objeto.
            // Esto cumple con el requisito de "string itemID" que pide el Manager.
            string exitID = SceneManager.GetActiveScene().name + "_Exit_" + gameObject.name;

            GlobalGameManager.Instance.CollectSecretItem(exitID);
        }
        // -----------------------------

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