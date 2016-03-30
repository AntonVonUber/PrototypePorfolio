using UnityEngine;
using System.Collections;

public class TriggerZone : MonoBehaviour
{
    public GameObject triggeredObject;

    void OnTriggerEnter(Collider collider)
    {
        triggeredObject.SetActive(true);
    }
}
