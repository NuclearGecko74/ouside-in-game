using UnityEngine;
using TMPro;

public class NoteController : MonoBehaviour
{
    public static NoteController instance;

    [Header("UI References")]
    public GameObject notePanel;
    public TextMeshProUGUI noteText;

    [Header("Configuración de Texto")]
    [Tooltip("Tamaño base para todas las notas. Auméntalo aquí si quieres letra más grande.")]
    public float globalFontSize = 15f; // <--- TAMAÑO GRANDE POR DEFECTO

    private bool isReading = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (notePanel != null) notePanel.SetActive(false);
    }

    void Update()
    {
        if (isReading)
        {
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseNote();
            }
        }
    }

    // Método simplificado
    public void ShowNote(string textContent, float customSize = 0)
    {
        isReading = true;
        notePanel.SetActive(true);

        // --- SEGURIDAD PARA QUE EL TEXTO NO DESAPAREZCA ---
        if (noteText != null)
        {
            // 1. Desactivar AutoSize para tener control manual
            noteText.enableAutoSizing = true;

            // 2. Permitir que el texto se desborde si es muy grande (EVITA QUE SE OCULTE)
            noteText.overflowMode = TextOverflowModes.Overflow;

            // 3. Aplicar el tamaño
            if (customSize > 0)
            {
                noteText.fontSize = customSize; // Tamaño específico de un objeto
            }
            else
            {
                noteText.fontSize = globalFontSize; // Tamaño global grande
            }

            noteText.text = textContent;
        }

        Time.timeScale = 0f;
    }

    public void CloseNote()
    {
        isReading = false;
        notePanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}