using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro

public class StartMenuController : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("The exact name of the first gameplay scene.")]
    public string firstSceneName = "Level1";

    [Header("UI References")]
    [Tooltip("Drag the 'Press Any Key' text here.")]
    public TextMeshProUGUI promptText;

    [Header("Visual Settings")]
    public float blinkSpeed = 2.0f;

    private bool hasStarted = false;

    void Update()
    {
        // 1. Text Blinking Effect
        if (promptText != null && !hasStarted)
        {
            // Creates a smooth fading effect (Alpha 0 to 1)
            float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1.0f) / 2.0f;
            promptText.color = new Color(promptText.color.r, promptText.color.g, promptText.color.b, alpha);
        }

        // 2. Input Detection
        // Input.anyKeyDown detects Keyboard keys or Mouse clicks
        if (Input.anyKeyDown && !hasStarted)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        hasStarted = true;

        // Lock alpha to 1 (fully visible) to indicate selection
        if (promptText != null)
            promptText.color = new Color(promptText.color.r, promptText.color.g, promptText.color.b, 1);

        Debug.Log("Starting Game...");

        // Attempt to find the SceneFader you already have
        GameObject faderObj = GameObject.FindGameObjectWithTag("BlackScreen");

        if (faderObj != null)
        {
            // Use your existing fade logic
            faderObj.GetComponent<SceneFader>().FadeOutAndLoad(firstSceneName);
        }
        else
        {
            // Fallback in case Fader is missing
            SceneManager.LoadScene(firstSceneName);
        }
    }
}