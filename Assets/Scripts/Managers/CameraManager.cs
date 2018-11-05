using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera MainCam;
    public static CameraManager instance;
    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
        MainCam = GetComponentInChildren<Camera>();
    }
}
