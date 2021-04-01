using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WaypointCreator : MonoBehaviour
{
    Vector3 oldPos;
    public float totalDistance = 0;
    
    public float shortDist = 1491.556f;
    public float safeDist = 2148.324f;
    public GameObject waypointPrefab;

    public static string fileName = "Assets/Data Files/safeWaypoints.txt";

    // Start is called before the first frame update
    void Start()
    {
        File.CreateText(fileName).Dispose(); // Deletes the file if it already exists
        StreamWriter sw = new StreamWriter(fileName);
        for(int i = 0; i < 10; i++)
            sw.WriteLine(" ");
        sw.Close();
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceVector = transform.position - oldPos;
        float distanceThisFrame = distanceVector.magnitude;
        totalDistance += distanceThisFrame;
        oldPos = transform.position;
        setPoints(safeDist);
    }

    void setPoints(float path) {
        float seperationDist = path / 11;
        for(int i = 1; i <= 10; i++) {
            if(Mathf.Round(totalDistance) == Mathf.Round(seperationDist * i)) {
                string[] arrLine = File.ReadAllLines(fileName);
                arrLine[i-1] = this.transform.position.ToString();
                File.WriteAllLines(fileName, arrLine);
            }
        }
    }
}
