using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{

    public Transform camera;
    public Transform light;

    // Connects the camera and the lights rotations
    void Update()
    {
        light.rotation = camera.rotation;
    }
}
