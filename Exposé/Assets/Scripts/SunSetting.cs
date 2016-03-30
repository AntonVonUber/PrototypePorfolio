using UnityEngine;
using System.Collections;

public class SunSetting : MonoBehaviour
{
    public GameObject mainCam;

    public Color startBackgroundColor;
    public Color endBackgroundColor;

    public float colorChangeDuration;
    public float settingSpeed;

    public float xEuler;
    private float yEuler;
    private float zEuler;


	// Use this for initialization
	void Start ()
    {
        yEuler = transform.rotation.y;
        zEuler = transform.rotation.z;

        RenderSettings.skybox.SetColor("_Tint", startBackgroundColor);
        RenderSettings.ambientLight = startBackgroundColor;
        colorChangeDuration = (xEuler / settingSpeed);
    }



    // Update is called once per frame
    void Update ()
    {
        if (xEuler > 0)
        {
            float lerp = Mathf.PingPong(Time.time, colorChangeDuration) / colorChangeDuration;
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(startBackgroundColor, endBackgroundColor, lerp));
            RenderSettings.ambientLight = Color.Lerp(startBackgroundColor, endBackgroundColor, lerp);

            xEuler -= (Time.deltaTime * settingSpeed);
            transform.rotation = Quaternion.Euler(xEuler, yEuler, zEuler);

            if(xEuler < 13)
            {
                mainCam.GetComponent<GlobalFog>().globalDensity = Mathf.Lerp(0.0f, 0.2f, lerp);
            }
        }
        else
        {
            this.enabled = false;
        }
    }
}
