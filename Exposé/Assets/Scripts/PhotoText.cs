using UnityEngine;
using System.Collections;

public class PhotoText : MonoBehaviour {

    private GameObject PhotoIndex;
    private GUIText myText;
    public GameObject savedPhotos;
    private BrowsePhotos browsePhotos;
    public GameObject albumManager;
    private Album album;
    private int num;
    private string text;

    void Awake()
    {
        PhotoIndex = gameObject;
        myText = PhotoIndex.GetComponent<GUIText>();
        Vector2 vector = new Vector2(7*Screen.width/2/8,-7*Screen.height/2/8);
        myText.pixelOffset = vector;

        browsePhotos = savedPhotos.GetComponent<BrowsePhotos>();
        album = albumManager.GetComponent<Album>();
    }

    void Update()
    {
        displayText();
    }

    void displayText()
    {
        if(album.getPhotosNum()==0)
        {
            num = 0;
        }
        else if(album.getPhotosNum() > 0)
        {
            num = browsePhotos.getCurrentIndex();
            num = num + 1;
        }
        string numString = num.ToString();
        text = numString + "/" + album.maxNum;
        myText.text = text;
    }
}
