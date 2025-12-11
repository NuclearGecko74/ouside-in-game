using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // Necesario para usar Listas

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    [Header("Configuración")]
    public int totalItemsToWin = 5;
    public string winSceneName = "WinScene";

    // --- NUEVO: MEMORIA DE OBJETOS ---
    // Usamos una lista para recordar QUÉ objetos específicos ya tomamos
    public List<string> collectedObjectIDs = new List<string>();

    private int currentItems = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectSecretItem(string itemID)
    {
        // 1. Guardamos el ID en la lista para que no vuelva a aparecer
        if (!collectedObjectIDs.Contains(itemID))
        {
            collectedObjectIDs.Add(itemID);

            // 2. Sumamos al contador numérico
            currentItems++;
            Debug.Log($"Objetos: {currentItems} / {totalItemsToWin} (ID: {itemID})");

            if (currentItems >= totalItemsToWin)
            {
                WinGame();
            }
        }
    }

    // Método para que el objeto pregunte si ya fue recogido
    public bool CheckIfCollected(string itemID)
    {
        return collectedObjectIDs.Contains(itemID);
    }

    public void ResetProgress()
    {
        currentItems = 0;
        collectedObjectIDs.Clear(); // <-- Borramos la memoria al reiniciar
        Debug.Log("Progreso e historial de objetos reiniciado.");
    }

    void WinGame()
    {
        Debug.Log("¡Victoria!");
        GameObject faderObj = GameObject.FindGameObjectWithTag("BlackScreen");

        if (faderObj != null)
            faderObj.GetComponent<SceneFader>().FadeOutAndLoad(winSceneName);
        else
            SceneManager.LoadScene(winSceneName);
    }
}