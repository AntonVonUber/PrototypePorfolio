using UnityEngine;
using System.Collections;

public class TakePhotos : MonoBehaviour
{
    //public float minDist;
    private GameObject mainCamera;
    private GameObject newsPaper; //maybe only for test
    private PlayerStates playerStates;
    private GameObject player;

    public GameObject camSwitchHud;
    private CamSwitchUI camSwitchUI;

    public GameObject takePhotoCam;

    public GameObject albumManager;
    private Album album;
    public int maxNum;

    public GameObject enterBrowsingHud;
    private EnterBrowsingUI enterBrowsingUI;

    public GameObject SavedPhotos;
    private BrowsePhotos browsePhotos;

    //for disabling the movement
    private MouseLook mouseLookY;
    private MouseLook mouseLookX;
    private CharacterMotor characterMotor;

    private AudioSource shutterSound;
    public AudioClip goodSound;
    public AudioClip badSound;

    public GameObject timeManager;
    private GameTimer gameTimer;

    private bool wasTakingPicture;


    void Awake()
    {
        mainCamera = transform.Find("Main Camera").gameObject;
        newsPaper = GameObject.FindGameObjectWithTag(Tags.photoPart); // maybe only for test
        player = gameObject;
        playerStates = player.GetComponent<PlayerStates>();
        //camSwitchHud = GameObject.Find("CamSwitchHud");
        camSwitchUI = camSwitchHud.GetComponent<CamSwitchUI>();
        album = albumManager.GetComponent<Album>();
        enterBrowsingUI = enterBrowsingHud.GetComponent<EnterBrowsingUI>();
        browsePhotos = SavedPhotos.GetComponent<BrowsePhotos>();

        //for disabling movement
        mouseLookY = mainCamera.GetComponent<MouseLook>();
        mouseLookX = player.GetComponent<MouseLook>();
        characterMotor = player.GetComponent<CharacterMotor>();

        shutterSound = GetComponent<AudioSource>();
        gameTimer = timeManager.GetComponent<GameTimer>();

        wasTakingPicture = false;
    }

    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        print("SwitchCamera:" + Input.GetAxis("SwitchCamera"));
        print("TakePhoto:" + Input.GetAxis("TakePhoto"));
        //should be e
        if ((Input.GetButtonDown("TakePhoto")||((Input.GetAxis("TakePhoto")==1) && !wasTakingPicture)) && playerStates.getPlayerState() == PlayerStates.PlayerStatesEnum.ReadyTakingPhotos && !gameTimer.isTimeUp())
        {
            int num = album.getPhotosNum();
            if (num < album.maxNum )
            {
                //print("newsPaper is "+ newsPaper);
                Texture2D content;
                content = takePhoto();
                //then we need to push it into a album, something like list, hold the reference the photo we just add to the album
                Photo currentPhoto;
                currentPhoto = saveToAlbum(content);
                //if photo is an important clue, then we can do some things fancy, like raise some scores or give special visual effect
                //or audio effect
                ifPhotoImorpant(currentPhoto);
            }
            else
            {
                shutterSound.clip = badSound;
                shutterSound.Play();
            }
            wasTakingPicture = true;
        }
        if(Input.GetAxis("TakePhoto")==0 && wasTakingPicture)
        {
            wasTakingPicture = false;
        }

        if (Input.GetButtonDown("SwitchCamera") || (Input.GetAxis("SwitchCamera") == -1))
        {
            //print("cameraHud is " + cameraHud);
            //print("camSwitchUI is" + camSwitchUI);
            //camSwitchUI.setPlayAnimMode(camSwitchUI.PlayAnim);
            camSwitchUI.setPlayAnimMode(CamSwitchUI.PlayAnimMode.Positive);
        }

        if (Input.GetButtonUp("SwitchCamera") || (Input.GetAxis("SwitchCamera") == 0))
        {
            camSwitchUI.setPlayAnimMode(CamSwitchUI.PlayAnimMode.Reverse);
        }

        //switch to browsing mode
        if (Input.GetButtonDown("EnterBrowse") && playerStates.getPlayerState() == PlayerStates.PlayerStatesEnum.ReadyTakingPhotos)
        {
            enterBrowsingUI.setPlayAnimMode(EnterBrowsingUI.PlayAnimMode.Enter);
        }

        //switch to camera normal mode
        if(Input.GetButtonDown("EnterBrowse") && playerStates.getPlayerState() == PlayerStates.PlayerStatesEnum.Browsing)
        {
            enterBrowsingUI.setPlayAnimMode(EnterBrowsingUI.PlayAnimMode.Exit);
        }

        //for debug
        if(Input.GetKeyDown(KeyCode.J))
        {
            //print("player state is" + playerStates.getPlayerState());
        }

        if (Input.GetButtonDown("NextPhoto") && playerStates.getPlayerState() == PlayerStates.PlayerStatesEnum.Browsing)
        {
            browsePhotos.nextPhoto();
        }

        if (Input.GetButtonDown("PrePhoto") && playerStates.getPlayerState() == PlayerStates.PlayerStatesEnum.Browsing)
        {
            browsePhotos.prePhoto();
        }

        if (Input.GetButtonDown("DeletePhoto") && playerStates.getPlayerState() == PlayerStates.PlayerStatesEnum.Browsing)
        {
            browsePhotos.deleteCurrentPhoto();
        }

        //diable or enable player movement
        if (playerStates.getPlayerState() == PlayerStates.PlayerStatesEnum.Browsing || 
            playerStates.getPlayerState() == PlayerStates.PlayerStatesEnum.Waiting)
        {
            mouseLookX.enabled = false;
            mouseLookY.enabled = false;
            characterMotor.enabled = false;
        }
        else
        {
            mouseLookX.enabled = true;
            mouseLookY.enabled = true;
            characterMotor.enabled = true;
        }
    }

    void ifPhotoImorpant(Photo currentPhoto)
    {
        RaycastHit hit;
        //cast very far ray first, then check dist later
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 1000.0f))
        {
            if (hit.collider.gameObject.tag == Tags.photoZone)
            {
                GameObject photoZone = hit.collider.gameObject;
                ZoneConfig zoneConfig = photoZone.GetComponent<ZoneConfig>();
                float minDist = zoneConfig.minDist;
                float maxDist = zoneConfig.maxDIst;
                float dist = Vector3.Distance(mainCamera.transform.position, photoZone.transform.position);
                if (dist < maxDist && dist > minDist)
                {
                    float angle = Vector3.Angle(-mainCamera.transform.forward, photoZone.transform.forward);
                    float maxAngle = zoneConfig.maxAngle;
                    if (angle < maxAngle * 0.5f)
                    {
                        print(angle);
                        print("This is an important Photo!");
                        //we can design score related stuff here
                        //if(Input.GetButton("Fire1"))
                        //{
                        //print("take photo!");
                        //}
                    }
                }
            }
        }
        else
        {

        }
    }

    Photo saveToAlbum(Texture2D content)
    {
        Photo photo = new Photo(content);
        album.addPhoto(photo);
        return photo;
    }


    Texture2D takePhoto()
    {
        int width = Screen.width;
        int height = Screen.height;
        Rect rect = new Rect(0, 0, width, height);
        //here maybe we can instantiate a new camera 

        //Camera camera = mainCamera.camera; //maybe shor for getcomponent camera
        GameObject tempCamObj = Instantiate(takePhotoCam, mainCamera.transform.position, mainCamera.transform.rotation) as GameObject;
        Camera tempCam = tempCamObj.GetComponent<Camera>();
        tempCam.GetComponent<GlobalFog>().globalDensity = mainCamera.GetComponent<GlobalFog>().globalDensity;

        Texture2D screenShot = CaptureCamera(tempCam, rect);//camera should be a component, not a kind of gameobject
        if(shutterSound.clip != goodSound)
            shutterSound.clip = goodSound;
        shutterSound.Play();

        GameObject.Destroy(tempCamObj);

        //below is not necessary in this funcion, just for test
        //MeshRenderer meshRenderer = newsPaper.GetComponent<MeshRenderer>();
        //meshRenderer.material.mainTexture = screenShot;

        return screenShot;
    }

    

    //return a black and white picture from a camera
    Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        //Unity Pro only, we're luck!
        camera.targetTexture = rt;
        camera.Render();
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);//this funtino only workds on ARGB32 and RGB24
        //we can use screenShot setpixel method to make it black and white maybe
        /*
        int y = 0;
        while (y < screenShot.height)
        {
            int x = 0;
            while (x < screenShot.width)
            {
                Color oldColor = screenShot.GetPixel(x, y);
                Color newColor = new Color(0.0f, 0.0f, 0.0f);
                //gray = red * 0.299 + green * 0.587 + blue * 0.114 
                //more efficient version: (UINT)(p->Blue * 29 + p->Green * 150 + p->Red * 77 + 128) >> 8;
                //newColor.r = newColor.g = newColor.b = (int)(oldColor.b * 29 + oldColor.g * 150 + oldColor.r * 77) >> 8; 
                newColor.r = newColor.g = newColor.b = oldColor.r * 0.299f + oldColor.g * 0.587f + oldColor.b * 0.114f;
                screenShot.SetPixel(x, y, newColor);
                ++x;
            }
            ++y;
        }
         */

        screenShot.Apply();
        //clean up 
        camera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);

        return screenShot;
    }
}
