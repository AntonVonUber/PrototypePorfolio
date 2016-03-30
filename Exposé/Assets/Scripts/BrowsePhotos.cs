using UnityEngine;
using System.Collections;

public class BrowsePhotos : MonoBehaviour {
    public Texture2D noPhotosTexture;
    private GameObject savedPhotos;
    private GUITexture myTexture;

    public GameObject albumManager;
    private Album album;

    private int currentPhotoIndex = 0;
    //for noPhotoAlign
    Rect noPhotoRect;

    void Awake()
    {
        savedPhotos = gameObject;
        myTexture = savedPhotos.GetComponent<GUITexture>();
        myTexture.texture = noPhotosTexture;
        //x -260, y -130, w 512 H 256

        //Rect rect = new Rect(-myTexture.pixelInset.width / 2, -myTexture.pixelInset.height / 2,
                            //myTexture.pixelInset.width,myTexture.pixelInset.height);
        noPhotoRect = new Rect(-noPhotosTexture.width / 2, -noPhotosTexture.height / 2 - Screen.height/2/128 , noPhotosTexture.width, noPhotosTexture.height);

        myTexture.pixelInset = noPhotoRect;

        album = albumManager.GetComponent<Album>();
    }

    void Update()
    {
        displayPhoto();
    }

    void displayPhoto()
    {
        if(album.getPhotosNum()>0)
        {
            Texture2D picture = album.getPhoto(currentPhotoIndex).getContent();
            if (picture != myTexture.texture)
            {
                //Rect rect = new Rect(-picture.width/2,-picture.height/2,picture.width,picture.height);
                Rect rect = new Rect(-Screen.width/2,-Screen.height/2, Screen.width, Screen.height);
                myTexture.pixelInset = rect;
                myTexture.texture = picture;                
            }
        }
        else
        {
            myTexture.pixelInset = noPhotoRect;
            myTexture.texture = noPhotosTexture;
        }
    }

    public void nextPhoto()
    {
        if (album.getPhotosNum() > 0)
        {
            currentPhotoIndex++;
            currentPhotoIndex = currentPhotoIndex % album.getPhotosNum();
            print("currentPhoto index " + currentPhotoIndex);// for test
        }
    }

    public void prePhoto()
    {
        if( album.getPhotosNum()>0)
        {
            currentPhotoIndex--;
            currentPhotoIndex = (currentPhotoIndex + album.getPhotosNum()) % album.getPhotosNum();
            print("currentPhoto index " + currentPhotoIndex); //for test
        }
    }

    public void deleteCurrentPhoto()
    {
        if(album.getPhotosNum() > 0)
        {
            album.deletePhoto(currentPhotoIndex);
        }
        if(currentPhotoIndex > 0 )
        {            
            currentPhotoIndex--;
        }
    }

    public int getCurrentIndex()
    {
        return currentPhotoIndex;
    }
}
