using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cinematics : MonoBehaviour
{
    public GameObject player;
    public Camera mainCam;
    public Camera introCamera;
    public Camera outroCamera;
    public GameObject Sun;

    private AudioSource audioSource;
    public AudioClip firstRadioConvo;
    public AudioClip helicopters;
    public AudioClip secondRadioConvo;
    public AudioClip carDrivingAway;
    public AudioClip officeNoise;

    public GUITexture fadeScreen;
    public float fadeSpeed;
    public float alpha;
    public float neutralColor;
    public float lighterColor;
    public float logoAlpha;

    public GUITexture start;
    public GUITexture logo;
    public GUITexture fiea;
    public GUITexture title;

    public Vector3 endCamPos;
    public float camMoveSpeed;

    private bool playOutro;

    public GameObject albumManager;
    private Album album;
    private List<Photo> photos;

    public List<GameObject> tabloidPhotos;


    public enum CinematicStatesEnum {   BeforeStart,    AfterStart,
                                        LogoFadeIn,     LogoFadeOut,    FieaFadeIn,
                                        FieaFadeOut,    TitleFadeIn,    TitleFadeOut,
                                        WaitForBegin,   BeginFadeIn,    PlayingGame,
                                        RadioConvo2,    Helicopters,    FadeToCar,
                                        FadeIntoDesk,   AdjustCamera,   FadeOut };

    public CinematicStatesEnum cinemaCurrentState = CinematicStatesEnum.BeforeStart;

    public void setCinemaState(CinematicStatesEnum state)
    {
        cinemaCurrentState = state;
    }

    public CinematicStatesEnum getCinemaState()
    {
        return cinemaCurrentState;
    }


    public bool debugging;

    void Awake()
    {
        album = albumManager.GetComponent<Album>();
        if (!debugging)
        {
            if(cinemaCurrentState == CinematicStatesEnum.BeforeStart)
            {
                alpha = 1.0f;
                player.SetActive(false);
                mainCam.tag = "Untagged";
                introCamera.tag = "MainCamera";
                Sun.SetActive(false);
                RenderSettings.ambientLight = Color.black;

                introCamera.enabled = true;
                outroCamera.enabled = false;
                mainCam.enabled = false;

                fadeScreen.pixelInset = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
            }
            else
            {
                alpha = 0.0f;
                player.SetActive(true);
                mainCam.tag = "MainCamera";
                introCamera.tag = "Untagged";
                introCamera.enabled = false;
                Sun.SetActive(true);

                introCamera.enabled = false;
                outroCamera.enabled = false;
                mainCam.enabled = true;

                fadeScreen.pixelInset = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
            }
            neutralColor = 0.5f;
            lighterColor = 0.3f;
            fadeScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            start.color = new Color(neutralColor, neutralColor, neutralColor, 0.0f);
            logo.color = new Color(neutralColor,neutralColor,neutralColor,0.0f);
            fiea.color = new Color(neutralColor,neutralColor,neutralColor,0.0f);
            title.color = new Color(neutralColor, neutralColor, neutralColor, 0.0f);
        }
        else
        {
            player.SetActive(true);
            Sun.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!debugging)
        {
            switch(cinemaCurrentState)
            {
                case CinematicStatesEnum.BeforeStart:
                    alpha -= fadeSpeed * Time.deltaTime;
                    alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
                    fadeScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                    start.color = new Color(lighterColor, lighterColor, lighterColor, (0.5f - alpha));
                    if (alpha == 0.0f)
                    {
                        if (Input.GetButton("Start"))
                        {
                            cinemaCurrentState = CinematicStatesEnum.AfterStart;
                        }
                    }
                    break;
                case CinematicStatesEnum.AfterStart:
                    alpha += fadeSpeed * Time.deltaTime;
                    alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
                    fadeScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                    start.color = new Color(lighterColor, lighterColor, lighterColor, (0.5f - alpha));
                    if(alpha == 1.0f)
                    {
                        cinemaCurrentState = CinematicStatesEnum.LogoFadeIn;
                        audioSource = introCamera.GetComponent<AudioSource>();
                        audioSource.Stop();
                        audioSource.clip = firstRadioConvo;
                        audioSource.volume = 0.25f;
                        audioSource.PlayDelayed(2.0f);
                    }
                    break;
                case CinematicStatesEnum.LogoFadeIn:
                    logoAlpha += fadeSpeed * Time.deltaTime;
                    logoAlpha = Mathf.Clamp(logoAlpha, 0.0f, 1.0f);
                    logo.color = new Color(neutralColor, neutralColor, neutralColor, logoAlpha);
                    if (logoAlpha == 1.0f)
                    {
                        cinemaCurrentState = CinematicStatesEnum.LogoFadeOut;
                    }
                    break;
                case CinematicStatesEnum.LogoFadeOut:
                    logoAlpha -= fadeSpeed * Time.deltaTime;
                    logoAlpha = Mathf.Clamp(logoAlpha, 0.0f, 1.0f);
                    logo.color = new Color(neutralColor, neutralColor, neutralColor, logoAlpha);
                    if (logoAlpha == 0.0f)
                    {
                        cinemaCurrentState = CinematicStatesEnum.FieaFadeIn;
                    }
                    break;
                case CinematicStatesEnum.FieaFadeIn:
                    logoAlpha += fadeSpeed * Time.deltaTime;
                    logoAlpha = Mathf.Clamp(logoAlpha, 0.0f, 1.0f);
                    fiea.color = new Color(neutralColor, neutralColor, neutralColor, logoAlpha);
                    if (logoAlpha == 1.0f)
                    {
                        cinemaCurrentState = CinematicStatesEnum.FieaFadeOut;
                    }
                    break;
                case CinematicStatesEnum.FieaFadeOut:
                    logoAlpha -= fadeSpeed * Time.deltaTime;
                    logoAlpha = Mathf.Clamp(logoAlpha, 0.0f, 1.0f);
                    fiea.color = new Color(neutralColor, neutralColor, neutralColor, logoAlpha);
                    if (logoAlpha == 0.0f)
                    {
                        cinemaCurrentState = CinematicStatesEnum.TitleFadeIn;
                    }
                    break;
                case CinematicStatesEnum.TitleFadeIn:
                    logoAlpha += fadeSpeed * Time.deltaTime;
                    logoAlpha = Mathf.Clamp(logoAlpha, 0.0f, 1.0f);
                    title.color = new Color(neutralColor, neutralColor, neutralColor, logoAlpha);
                    if (logoAlpha == 1.0f)
                    {
                        cinemaCurrentState = CinematicStatesEnum.TitleFadeOut;
                    }
                    break;
                case CinematicStatesEnum.TitleFadeOut:
                    logoAlpha -= fadeSpeed * Time.deltaTime;
                    logoAlpha = Mathf.Clamp(logoAlpha, 0.0f, 1.0f);
                    title.color = new Color(neutralColor, neutralColor, neutralColor, logoAlpha);
                    if (logoAlpha == 0.0f && !audioSource.isPlaying)
                    {
                        cinemaCurrentState = CinematicStatesEnum.WaitForBegin;
                    }
                    break;
                case CinematicStatesEnum.WaitForBegin:
                    if(!audioSource.isPlaying)
                    {
                        cinemaCurrentState = CinematicStatesEnum.BeginFadeIn;
                        player.SetActive(true);
                        introCamera.GetComponent<AudioListener>().enabled = false;
                        mainCam.enabled = true;
                        mainCam.tag = "MainCamera";
                        introCamera.tag = "Untagged";
                        introCamera.enabled = false;
                    }
                    break;
                case CinematicStatesEnum.BeginFadeIn:
                    alpha -= fadeSpeed * Time.deltaTime;
                    alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
                    fadeScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                    if (alpha == 0.0f)
                    {
                        cinemaCurrentState = CinematicStatesEnum.PlayingGame;
                        Sun.SetActive(true);
                    }
                    break;
                case CinematicStatesEnum.PlayingGame:
                    // NOTHING HERE! HAVE FUN!!
                    break;
                case CinematicStatesEnum.RadioConvo2:
                    StartCoroutine("playSecondConvoClip");
                    break;
                case CinematicStatesEnum.Helicopters:
                    if (audioSource != null && !audioSource.isPlaying)
                    {
                        StartCoroutine("playHelicopterClip");
                    }
                    break;
                case CinematicStatesEnum.FadeToCar:
                    if (audioSource != null && !audioSource.isPlaying)
                    {
                        alpha += (fadeSpeed * Time.deltaTime);
                        alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
                        fadeScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                        if (alpha == 1.0f)
                        {
                            StartCoroutine("playCarEscapeClip");
                        }
                    }
                    break;
                case CinematicStatesEnum.FadeIntoDesk:
                    if (audioSource != null && !audioSource.isPlaying)
                    {
                        alpha -= (fadeSpeed * Time.deltaTime);
                        alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
                        fadeScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                        if (alpha == 0.0f)
                        {
                            cinemaCurrentState = CinematicStatesEnum.AdjustCamera;
                        }
                    }
                    break;
                case CinematicStatesEnum.AdjustCamera:
                    print(Mathf.Abs(Vector3.Distance(outroCamera.transform.position,endCamPos)));
                    if(Mathf.Abs(Vector3.Distance(outroCamera.transform.position,endCamPos)) > 0.01f)
                    {
                        outroCamera.transform.position = Vector3.Lerp(outroCamera.transform.position, endCamPos, camMoveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        cinemaCurrentState = CinematicStatesEnum.FadeOut;
                    }
                    break;
                case CinematicStatesEnum.FadeOut:
                    alpha += (fadeSpeed * Time.deltaTime);
                    alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
                    fadeScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                    if (alpha == 1.0f)
                    {
                        StartCoroutine("quitGame");
                    }
                    break;
            }
        }
    }

    // called by 'GameTimer'
    public void playOutroCinematic()
    {
        cinemaCurrentState = CinematicStatesEnum.RadioConvo2;
    }

    IEnumerator playSecondConvoClip()
    {
        if (audioSource != null)
        {
            audioSource.enabled = false;
        }
        audioSource = player.GetComponent<AudioSource>();
        audioSource.enabled = true;
        audioSource.clip = secondRadioConvo;
        yield return new WaitForSeconds(0.2f);
        audioSource.Play();
        cinemaCurrentState = CinematicStatesEnum.Helicopters;
        alpha = 0.0f;   // *** MAY NOT BE NECESSARY...
    }
    IEnumerator playHelicopterClip()
    {
        if(audioSource != null)
        {
            audioSource.enabled = false;
        }
        audioSource = player.GetComponent<AudioSource>();
        audioSource.enabled = true;
        audioSource.clip = helicopters;
        yield return new WaitForSeconds(0.2f);
        audioSource.Play();
        cinemaCurrentState = CinematicStatesEnum.FadeToCar;
    }

    IEnumerator playCarEscapeClip()
    {
        outroCamera.GetComponent<AudioListener>().enabled = true;
        outroCamera.enabled = true;
        outroCamera.tag = "MainCamera";

        mainCam.GetComponent<AudioListener>().enabled = false;
        mainCam.enabled = false;
        mainCam.tag = "Untagged";

        introCamera.GetComponent<AudioListener>().enabled = false;
        introCamera.enabled = false;
        introCamera.tag = "Untagged";

        player.SetActive(false);

        RenderSettings.ambientLight = Color.gray;
        photos = album.choosePhoto();

        for (int i = 0; i < photos.Count; ++i )
        {
            tabloidPhotos[i].GetComponent<Renderer>().material.mainTexture = photos[i].getContent();
            tabloidPhotos[i].SetActive(true);
        }

        audioSource = outroCamera.GetComponent<AudioSource>();
        yield return new WaitForSeconds(0.2f);
        audioSource.clip = carDrivingAway;
        yield return new WaitForSeconds(0.2f);
        audioSource.Play();
        cinemaCurrentState = CinematicStatesEnum.FadeIntoDesk;
    }
    IEnumerator quitGame()
    {
        yield return new WaitForSeconds(2.0f);
        Application.Quit();
    }
}