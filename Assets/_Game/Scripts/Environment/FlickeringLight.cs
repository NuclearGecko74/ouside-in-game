using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    private Light myLight;

    [Header("Configuración")]
    public float minWaitTime = 0.1f;
    public float maxWaitTime = 0.5f;

    void Start()
    {
        myLight = GetComponent<Light>();
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            myLight.enabled = !myLight.enabled;

            // if(myLight.enabled) audioSource.PlayOneShot(sparkSound);
        }
    }
}