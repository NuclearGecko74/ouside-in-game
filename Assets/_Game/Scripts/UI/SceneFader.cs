using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    private Image blackScreen;

    void Start()
    {
        blackScreen = GetComponent<Image>();

        blackScreen.color = new Color(0, 0, 0, 1);

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        blackScreen.color = new Color(0, 0, 0, 0);
        blackScreen.raycastTarget = false;
    }

    public void FadeOutAndLoad(string sceneName)
    {
        StartCoroutine(FadeOutRoutine(sceneName));
    }

    IEnumerator FadeOutRoutine(string sceneName)
    {
        blackScreen.raycastTarget = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Lerp de 0 (Transparente) a 1 (Negro)
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        blackScreen.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene(sceneName);
    }
}