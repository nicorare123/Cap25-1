using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject virtualCam;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            virtualCam.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            if(virtualCam != null)
            {
                virtualCam.SetActive(false);
            }
        }
    }
}
