using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CreatePath : MonoBehaviour
{
    public LineRenderer lr;

    void Start()
    {
        string fileName;
        if(TogglePath.shortestPath == false) 
			fileName = "Assets/Data Files/safePath.txt";
        else
            fileName = "Assets/Data Files/shortPath.txt";
        StreamReader reader = new StreamReader(fileName);

        string line;
        int lineCount = 0;
        string[] transforms;

        while((line = reader.ReadLine()) != null) { 
            lr.positionCount += 1;
            transforms = line.Split(',');
            lr.SetPosition(lineCount, new Vector3(float.Parse(transforms[0]), float.Parse(transforms[1]) + 1, float.Parse(transforms[2])));
            lineCount++;
        }

        reader.Close();
    }
}
