using UnityEngine;
using System.Collections;

public class UI_Manager
    : MonoBehaviour
{
	// Use this for initialization
    void Start()
    {
        Cursor.visible = false;
    }

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetAxis("Fire2") == 1)
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Fire3") == 1)
        {
            Time.timeScale = 10;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}