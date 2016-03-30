using UnityEngine;
using System.Collections;
//using System; //This allows the IComparable Interface

public class Photo
{
    private Texture2D content;
    
    public Photo(Texture2D texture)
    {
        content = texture;
    }

    public void setContent(Texture2D texture)
    {
        content = texture;
    }

    public Texture2D getContent()
    {
        return content;
    }
}
