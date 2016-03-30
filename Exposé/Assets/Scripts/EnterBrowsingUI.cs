using UnityEngine;
using System.Collections;

public class EnterBrowsingUI : MonoBehaviour {
    public Texture2D[] sequence;
    private GameObject enterBrowsingHud;
    private GUITexture myTexture;
    private float passTime = 0.0f;
    private int index = 0;

    public enum PlayAnimMode { Default, Enter, Exit, Stop };
    private PlayAnimMode playAnimMode = PlayAnimMode.Default;

    public int fps;

    public GameObject player;
    private PlayerStates playerStates;

    public GameObject camNormalHud;
    public GameObject camBrowsingHud;

    private float totalTime;
    void Awake()
    {
        enterBrowsingHud = gameObject;
        myTexture = enterBrowsingHud.GetComponent<GUITexture>();
        playerStates = player.GetComponent<PlayerStates>();

        int n = sequence.Length;
        totalTime = (1.0f / (float)fps) * (float)n;
    }


    void Update()
    {
        playAnim();
    }

    void playAnim()
    {
        switch(playAnimMode)
        {
            case PlayAnimMode.Enter:
                if (playerStates.getPlayerState() != PlayerStates.PlayerStatesEnum.Waiting)
                {
                    playerStates.setPlayerState(PlayerStates.PlayerStatesEnum.Waiting);
                }
                passTime += Time.deltaTime;
                if (passTime >= totalTime)
                {
                    passTime = 0.0f;
                    setPlayAnimMode(PlayAnimMode.Default);
                    playerStates.setPlayerState(PlayerStates.PlayerStatesEnum.Browsing);
                }
                else
                {
                    index = (int)(passTime * fps);
                    index = index % sequence.Length;
                    myTexture.texture = sequence[index];
                    if (index >= (sequence.Length / 2) )
                    {
                       if(!camBrowsingHud.activeSelf)
                       {
                           camBrowsingHud.SetActive(true);
                       }
                       if (camNormalHud.activeSelf)
                       {
                           camNormalHud.SetActive(false);
                       }
                    }
                }
                break;
            case PlayAnimMode.Exit:
                if(playerStates.getPlayerState()!= PlayerStates.PlayerStatesEnum.Waiting)
                {
                    playerStates.setPlayerState(PlayerStates.PlayerStatesEnum.Waiting);
                }
                passTime += Time.deltaTime;
                if (passTime >= totalTime)
                {
                    passTime = 0.0f;
                    setPlayAnimMode(PlayAnimMode.Default);
                    playerStates.setPlayerState(PlayerStates.PlayerStatesEnum.ReadyTakingPhotos);
                }
                else
                {
                    index = (int)(passTime * fps);
                    index = index % sequence.Length;
                    myTexture.texture = sequence[index];
                    if (index >= (sequence.Length / 2))
                    {
                        if (camBrowsingHud.activeSelf)
                        {
                            camBrowsingHud.SetActive(false);
                        }
                        if (!camNormalHud.activeSelf)
                        {
                            camNormalHud.SetActive(true);
                        }
                    }
                }
                break;
            case PlayAnimMode.Default:
                break;
            case PlayAnimMode.Stop:
                myTexture.texture = null;
                setPlayAnimMode(PlayAnimMode.Default);
                break;
            default:
                break;
        }
    }

    public void setPlayAnimMode (PlayAnimMode mode )
    {
        playAnimMode = mode;
    }

    //we need function that we can get playAnimMode
     
    public PlayAnimMode getPlayAnimMode()
    {
        return playAnimMode;
    }
}
