using UnityEngine;
using System.Collections;

public class ReturnAlign : MonoBehaviour {
    private GameObject returnUI;
    private GUITexture myTexture;

    void Awake()
    {
        returnUI = gameObject;
        myTexture = returnUI.GetComponent<GUITexture>();
        Rect rect = new Rect(-myTexture.texture.width / 2 - 10 * Screen.width / 2 / 16, -myTexture.texture.height / 2 - 7*Screen.height/2/8, myTexture.texture.width, myTexture.texture.height);
        myTexture.pixelInset = rect;
    }
}
