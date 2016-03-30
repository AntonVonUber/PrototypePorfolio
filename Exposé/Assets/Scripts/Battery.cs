using UnityEngine;
using System.Collections;

public class Battery : MonoBehaviour {
    //0 for full, 1 for three green, 2 for yellow, 3 for red, 4 for empty
    public Texture2D[] sequence;
    private GameObject battery;
    private GUITexture myTexture;

    //time related part, I don't know whether we should get time 
    //from the sun
    public GameObject timeManager;
    private GameTimer gameTimer;
    private float currentTime;

    public float flashInterval;
    private float passTime = 0.0f;
    private bool startFlash = false;

    void Awake()
    {
        battery = gameObject;
        myTexture = battery.GetComponent<GUITexture>();
        myTexture.texture = sequence[0];
        Rect rect = new Rect(-myTexture.texture.width / 2 + 14 * Screen.width / 2 / 16, -myTexture.texture.height / 2 + 13*Screen.height/2/16, myTexture.texture.width, myTexture.texture.height);
        myTexture.pixelInset = rect;

        //time related part
        gameTimer = timeManager.GetComponent<GameTimer>();
    }

	// Update is called once per frame
	void Update () 
    {
	    chooseState();
	}

    private void chooseState()
    {
        //time related part
        currentTime = gameTimer.getCurrentTime();
        if(currentTime <= 0)
        {
            if(startFlash)
            {
                startFlash = false;
            }
            myTexture.texture = sequence[4];
        }
        else if(currentTime <= 1*gameTimer.totalTime /4)
        {
            if(!startFlash)
            {
                startFlash = true;
                myTexture.texture = sequence[3];
            }

            if(startFlash)
            {
                passTime += Time.deltaTime;
                if(passTime >= flashInterval)
                {
                    if(myTexture.texture == sequence[3])
                    {
                        myTexture.texture = sequence[4];
                    }
                    else if(myTexture.texture == sequence[4])
                    {
                        myTexture.texture = sequence[3];
                    }
                    passTime = 0.0f;
                }
            }
            //myTexture.texture = sequence[3];   
        }
        else if(currentTime <= 2*gameTimer.totalTime /4)
        {
            myTexture.texture = sequence[2];
        }
        else if(currentTime <= 3*gameTimer.totalTime /4)
        {
            myTexture.texture = sequence[1];
        }
    }
}
