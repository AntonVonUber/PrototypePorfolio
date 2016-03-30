using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Album : MonoBehaviour
{
    List<Photo> photos;
    System.Random random = new System.Random();
    public int maxNum;

    private Photo defaultImage;
    public Texture2D defaultTexture;

    void Awake()
    {
        photos = new List<Photo>();
        defaultImage = new Photo(defaultTexture);
    }

    public Photo getPhoto(int index)
    {
        return photos[index];
    }

    public void addPhoto(Photo photo)
    {
        photos.Add(photo);
    }

    //according index, delete photo
    public void deletePhoto(int index)
    {
        photos.RemoveAt(index);
    }

    public int getPhotosNum()
    {
        return photos.Count;
    }

    void Update()
    {
        //only for test
        /*
        if(Input.GetKeyDown(KeyCode.F))
        {
            print(photos.Count);
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            print(choosePhoto());
        }
         */
    }

    public List<Photo> choosePhoto()
    {
        List<Photo> tabloidPhotos = new List<Photo>();
        if (photos.Count != 0)
        {
            int minNumPhotos = Mathf.Min(maxNum, 5);
            for(int i = 0; i < minNumPhotos; ++i)
            {
                int index = random.Next(0, minNumPhotos);//6 should not be include;
                tabloidPhotos.Add(photos[index]);   // may need to eliminate already-used choices...
            }
        }
        else
        {
            tabloidPhotos.Add(defaultImage);
        }
        return tabloidPhotos;
    }

}
