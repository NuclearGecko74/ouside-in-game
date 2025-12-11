using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Nombre de tu escena de Menú o Nivel 1")]
    public string firstSceneName = "MainMenu";

    [Tooltip("Tiempo de espera antes de poder reiniciar")]
    public float delayBeforeInput = 2.0f;

    private float timer = 0f;
    private bool canInteract = false;

    void Update()
    {
        if (!canInteract)
        {
            timer += Time.deltaTime;
            if (timer >= delayBeforeInput) canInteract = true;
        }

        if (canInteract && Input.anyKeyDown)
        {
            // 1. ORDENAR EL REINICIO DE DATOS
            // Antes de cambiar de escena, limpiamos el contador
            if (GlobalGameManager.Instance != null)
            {
                GlobalGameManager.Instance.ResetProgress();
            }

            // 2. CAMBIAR DE ESCENA
            GameObject faderObj = GameObject.FindGameObjectWithTag("BlackScreen");

            if (faderObj != null)
            {
                // Usamos tu sistema de Fade existente
                faderObj.GetComponent<SceneFader>().FadeOutAndLoad(firstSceneName);
            }
            else
            {
                SceneManager.LoadScene(firstSceneName);
            }
        }
    }
}