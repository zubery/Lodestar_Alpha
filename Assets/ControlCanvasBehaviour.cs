using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCanvasBehaviour : MonoBehaviour
{

    Camera MainCam;
    private void Start()
    {
        MainCam = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(transform.position + MainCam.transform.rotation * Vector3.forward, MainCam.transform.rotation * Vector3.up);

    }
}
