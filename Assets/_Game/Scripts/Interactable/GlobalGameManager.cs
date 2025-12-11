using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    [Header("Configuración")]
    public int totalItemsToWin = 5;
    public string winSceneName = "WinScene";

    // Variable privada para que nadie la modifique por error, solo los métodos
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

    public void CollectSecretItem()
    {
        currentItems++;
        Debug.Log($"Objetos: {currentItems} / {totalItemsToWin}");

        if (currentItems >= totalItemsToWin)
        {
            WinGame();
        }
    }

    // --- ESTA ES LA FUNCIÓN CLAVE ---
    public void ResetProgress()
    {
        currentItems = 0;
        Debug.Log("El contador de objetos se ha reiniciado a 0.");
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