using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour {
    public float totalTime;//in second
    private float currentTime; 
    private bool isStart = true;

    public GameObject cinematography;
    private Cinematics cinScript;

    private bool playingOutro;

    void Awake()
    {
        cinScript = cinematography.GetComponent<Cinematics>();
        currentTime = totalTime;

        playingOutro = true;
    }
    
    void Update()
    {
        if(isStart)
        {
            currentTime -= Time.deltaTime;
        }
        isTimeUp();

        /*
        if(Input.GetKeyDown(KeyCode.J))
        {
            print("Current Time is" + currentTime);
        }
        */
    }

    public float getCurrentTime()
    {
        return currentTime;
    }

    public void enableTimer()
    {
        isStart = true;
    }

    public bool isTimeUp()
    {
        if(currentTime <= 0.0f)
        {
            if (playingOutro)
            {
                cinScript.playOutroCinematic();
                playingOutro = false;
            }
            return true;
        }
        return false;
    }
}
