using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StorePath : MonoBehaviour
{
    public static string fileName = "Assets/Data Files/shortPath.txt";
    private double currentPosX, currentPosY, currentPosZ;
    
    // Start is called before the first frame update
    void Start()
    {
        File.CreateText(fileName).Dispose(); // Deletes the file if it already exists
        InvokeRepeating("getPosition", 0f, .25f);
    }

    void getPosition() {
        currentPosX = this.gameObject.transform.position.x;
        currentPosY = this.gameObject.transform.position.y;
        currentPosZ = this.gameObject.transform.position.z;

        StreamWriter writer = File.AppendText(fileName);
        writer.WriteLine(currentPosX + ", " + currentPosY + ", " + currentPosZ);
        writer.Close();
    }
}
