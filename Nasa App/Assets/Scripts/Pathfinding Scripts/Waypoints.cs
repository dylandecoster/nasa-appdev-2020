using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Azmuth;
using static TextUpdater;
using System.IO;

public class Waypoints : MonoBehaviour
{

    // Public varibles 
    public Transform player;
    public Camera cam;
    public TextUpdater info;

    // Private varibles
    private Image iconImg;
    private Text distanceText;
    public Transform[] targets = new Transform[10];
    public Vector3[] fileTarget = new Vector3[10];
    private int currentPoint = 0;
    private float closeEnoughDist = 10f;
    GameObject checkpointHolder;
    private Azmuth az;

    // Upon running the game
    private void Start()
    {
        string fileName = "";
        if(TogglePath.shortestPath == false) {
            fileName = "Assets/Data Files/safeWaypoints.txt";
            checkpointHolder = GameObject.Find("SafestPath");
        }
        else if(TogglePath.shortestPath == true) {
            fileName = "Assets/Data Files/shortWaypoints.txt";
            checkpointHolder = GameObject.Find("ShortestPath");
        }
        StreamReader reader = new StreamReader(fileName);

        string[] t;
        string line = "";
        int current = 0;
        while((line = reader.ReadLine()) != null) {
            t = line.Split(',');
            fileTarget[current] = new Vector3(float.Parse(t[0]), float.Parse(t[1]) + 8, float.Parse(t[2]));
            checkpointHolder.transform.Find(current.ToString()).transform.position = fileTarget[current];
            targets[current] = checkpointHolder.transform.Find(current.ToString());
            current++;
        }

        reader.Close();

        iconImg = GetComponent<Image>();
        distanceText = GetComponentInChildren<Text>();
    }

    // Every frame 
    private void Update()
    {
        // checks to see if the target isn't empty
        if (targets[currentPoint] != null)
        {
            GetDistance();
            CheckOnScreen();
        }
    }

    // Updates the distance text and changes to the next checkpoint when arriving at one
    private void GetDistance()
    {
        float dist = Vector3.Distance(player.position, targets[currentPoint].position);
        distanceText.text = dist.ToString("f1") + "m";

        if (dist < closeEnoughDist && currentPoint < 10)
        {
            // Displays info of the checkpoint
            az = new Azmuth(targets[currentPoint++]);
            info.textUpdate(az.checkpointString());

            // Changes to the new one
            transform.position = cam.WorldToScreenPoint(targets[currentPoint].position);
            if(currentPoint == 9) Destroy(gameObject);
        }
    }

    // Checks to see if the waypoint is on schreen
    private void CheckOnScreen()
    {
        float thing = Vector3.Dot((targets[currentPoint].position - cam.transform.position).normalized, cam.transform.forward);

        if(thing <= 0)
        {
            ToggleUI(false);
        }
        else
        {
            ToggleUI(true);
            transform.position = cam.WorldToScreenPoint(targets[currentPoint].position);
        }
    }

    private void ToggleUI(bool val)
    {
        iconImg.enabled = val;
        distanceText.enabled = val;
    }
}