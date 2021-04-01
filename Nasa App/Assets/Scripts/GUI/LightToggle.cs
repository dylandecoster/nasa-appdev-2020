/* Example level loader */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightToggle : MonoBehaviour
{

    bool On = false;
    public Transform light;

    public void OnClick()
    {
            if (On)
            {
                transform.localRotation = Quaternion.Euler(0, light.localRotation.y, 0);
                On = false;
            }

            else
            {
                transform.localRotation = Quaternion.Euler(90, light.localRotation.y, 0);
                On = true;
            }
    }
}