using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public CinemachineBrain cinemachineBrain;
    public GameObject decorationCamera;
    public GameObject servingCamera;
    public GameObject lastFocusCamera;

    private void Awake()
    {
        instance = this;
    }
}
