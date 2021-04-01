using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour {

    public Text textObject;

    public void textUpdate(string info)
    {
        textObject.text = info;
    }
}
