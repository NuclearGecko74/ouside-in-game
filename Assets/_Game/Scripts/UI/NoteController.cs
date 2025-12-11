using UnityEngine;
using TMPro;

public class NoteController : MonoBehaviour
{
    public static NoteController instance;

    [Header("UI References")]
    public GameObject notePanel;
    public TextMeshProUGUI noteText;

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
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseNote();
            }
        }
    }

    public void ShowNote(string textContent)
    {

        isReading = true;

        noteText.text = textContent;
        notePanel.SetActive(true);

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