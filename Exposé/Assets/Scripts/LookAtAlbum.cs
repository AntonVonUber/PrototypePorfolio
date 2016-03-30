using UnityEngine;
using System.Collections;

public class LookAtAlbum : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject album;

    public float angleCanSeeAlbum;
    private bool isSeeingAlbum = false;


    public float albumMoveSpeed;
    public float albumMoveDist;

    //they are local to parent player
    private Vector3 targetAlbumPos;
    private Vector3 oldAlbumPos;
    private float deactivateMargin = 0.1f;

    void Awake()
    {
        mainCamera = transform.Find("Main Camera").gameObject;
        album = transform.Find("Album").gameObject;

        oldAlbumPos = album.transform.localPosition;
        targetAlbumPos = oldAlbumPos + Vector3.forward * albumMoveDist;

        album.SetActive(false);
    }

    void Update()
    {
        float angle = Vector3.Angle(-transform.up, mainCamera.transform.forward);
        //print("angle"+ angle);
        if (angle < angleCanSeeAlbum)
        {
            if (!isSeeingAlbum)
            {
                isSeeingAlbum = true;
                //print(isSeeingAlbum);
            }
        }
        else
        {
            if (isSeeingAlbum)
            {
                isSeeingAlbum = false;
                //print(isSeeingAlbum);
            }
        }

        showAlbum();
    }


    void showAlbum()
    {

        if (isSeeingAlbum)
        {
            album.SetActive(true);
            album.transform.localPosition = Vector3.Lerp(album.transform.localPosition, targetAlbumPos, Time.deltaTime * albumMoveSpeed);
        }
        else
        {
            album.transform.localPosition = Vector3.Lerp(album.transform.localPosition, oldAlbumPos, Time.deltaTime * albumMoveSpeed);
            if (Vector3.Distance(album.transform.localPosition, oldAlbumPos) < deactivateMargin)
            {
                album.SetActive(false);
            }
        }
    }
}
