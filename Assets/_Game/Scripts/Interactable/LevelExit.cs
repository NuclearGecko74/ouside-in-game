using UnityEngine;

public class LevelExit : MonoBehaviour, IInteractable
{
    public string nextSceneName;

    public string GetDescription()
    {
        return "";
    }

    public void Interact()
    {
        GameObject faderObj = GameObject.FindGameObjectWithTag("BlackScreen");

        if (faderObj != null)
        {
            SceneFader fader = faderObj.GetComponent<SceneFader>();
            fader.FadeOutAndLoad(nextSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}