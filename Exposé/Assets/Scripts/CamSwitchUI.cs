using UnityEngine;
using System.Collections;

public class CamSwitchUI : MonoBehaviour
{
    public Texture2D[] sequence;
    private GameObject camSwitchHud;
    private GUITexture myTexture;
    private float startTime = 0.0f;
    private int index = 0;

    public enum PlayAnimMode { Default, Positive, Reverse };
    private PlayAnimMode playAnimMode = PlayAnimMode.Default;

    //private bool playPositiveDone = false;
    //private bool playpReverseDone = false;

    public int fps;

    private GameObject player;
    private PlayerStates playerStates;

    public GameObject camNormalHud;
    public GameObject camBrowsingHud;

    public GameObject enterBrowsingHud;
    //private EnterBrowsingUI enterBrowsingUI;

    void Awake()
    {
        camSwitchHud = gameObject;
        myTexture = camSwitchHud.GetComponent<GUITexture>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerStates = player.GetComponent<PlayerStates>();
        //enterBrowsingUI = enterBrowsingHud.GetComponent<EnterBrowsingUI>();//maybe we don't need it. 
    }


    void Update()
    {
        //for debug
        if(Input.GetKeyDown(KeyCode.J))
        {
            print("what is playerStates" + playerStates);
        }
        playAnim();
    }

    public void playAnim()
    {
        switch (playAnimMode)
        {
            case PlayAnimMode.Positive:
                startTime += Time.deltaTime;
                //more exactly we can use time to judge:
                //startTime >=  1/fps * sequence.Length 
                int n = sequence.Length;
                float temp = (1.0f / (float)fps) * (float) n;
                if (startTime >= temp)
                {
                    startTime = temp;
                    setPlayAnimMode(PlayAnimMode.Default);
                    if (camNormalHud.activeSelf)
                    {
                        playerStates.setPlayerState(PlayerStates.PlayerStatesEnum.ReadyTakingPhotos);
                    }
                    if(camBrowsingHud.activeSelf)
                    {
                        playerStates.setPlayerState(PlayerStates.PlayerStatesEnum.Browsing);
                    }
                }
                else
                {
                    index = (int)(startTime * fps);
                    index = index % sequence.Length;
                    //print("index "+ index);
                    myTexture.texture = sequence[index];
                    //activate the normal camera hud
                    if(index >= (sequence.Length /2) && !camNormalHud.activeSelf )
                    {
                        if(!camBrowsingHud.activeSelf)
                        {
                            camNormalHud.SetActive(true);
                        }
                    }
                }
                break;
            case PlayAnimMode.Reverse:
                startTime -= Time.deltaTime;
                if(playerStates.getPlayerState() != PlayerStates.PlayerStatesEnum.Default)
                {
                    //print("helloooooo!");
                    playerStates.setPlayerState(PlayerStates.PlayerStatesEnum.Default);
                }
                if (startTime <= 0)
                {
                    startTime = 0;
                    setPlayAnimMode(PlayAnimMode.Default);
                }
                else
                {
                    index = (int)(startTime * fps);
                    index = index % sequence.Length;
                    myTexture.texture = sequence[index];
                    if(index<=(sequence.Length/2) )
                    {
                        if (camNormalHud.activeSelf)
                        {
                            camNormalHud.SetActive(false);
                        }
                        if (camBrowsingHud.activeSelf)
                        {
                            camBrowsingHud.SetActive(false);
                        }
                        
                    }
                }
                break;
            case PlayAnimMode.Default:
                break;
            default:
                break;
        }
    }

    public void setPlayAnimMode(PlayAnimMode mode)
    {
        playAnimMode = mode;
    }
}
