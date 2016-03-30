using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    public bool isRapidFlicker;
    public float minFlickerSpeed;   // 0.01f;
    public float midFlickerSpeed;   // 0.1f;
    public float maxFlickerSpeed;   // 0.4f;

    // for the 'soft flicker' effect, if 'isRapidFlicker' == false
    public float minIntensity;      // 0.25f;
    public float maxIntensity;      // 2.0f;

    private float random;

    void Start()
    {
        random = Random.Range(0.0f, 65535.0f);
        StartCoroutine("Flicker");
    }

    private IEnumerator Flicker()
    {
        if(isRapidFlicker)
        {
            GetComponent<Light>().enabled = true;
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
            GetComponent<Light>().enabled = false;
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, midFlickerSpeed));
        }
        else
        {
            // for a 'soft flicker'... like a random pulsing...:
            float noise = Mathf.PerlinNoise(random, Time.time);
            GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        }
        SendMessage("Flicker");
    }
}
