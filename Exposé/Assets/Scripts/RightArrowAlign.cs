using UnityEngine;
using System.Collections;

public class RightArrowAlign : MonoBehaviour {
    private GameObject rightArrow;
    private GUITexture myTexture;

    void Awake()
    {
        rightArrow = gameObject;
        myTexture = rightArrow.GetComponent<GUITexture>();
        Rect rect = new Rect(-myTexture.texture.width / 2 + 15 * Screen.width / 2 / 16, -myTexture.texture.height / 2, myTexture.texture.width, myTexture.texture.height);
        myTexture.pixelInset = rect;
    }

}
