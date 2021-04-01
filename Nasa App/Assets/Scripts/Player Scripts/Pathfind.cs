using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public CoordinateConverter startingPoint = new CoordinateConverter(-89.232, 54.794);
    public CoordinateConverter endPoint = new CoordinateConverter(-89.200, 120.690);
    double currentX, currentY = 250, currentZ;
    double finalX, finalZ;

    public GameObject player;
    public GameObject endObject;

    public LineRenderer line;
    public Camera cam;

    float distanceFromGround;
    int i = 1;

    // Start is called before the first frame update
    void Start()
    {
        distanceFromGround = transform.position.y;

        currentX = startingPoint.x;
        currentZ = startingPoint.z;
        finalX = endPoint.x;
        finalZ = endPoint.z;

        player.transform.position = new Vector3((float)currentX, (float)currentY, (float)currentZ);
        endObject.transform.position = new Vector3((float)finalX, 326, (float)finalZ);
    }

    // Update is called once per frame
    void Update()
    {
        //line.SetPosition(0, transform.position); // Sets the lines starting position

        RaycastHit hit;
        // Basically checks the ground 10m in front of the player and gets the distance of that from the ground
        if (Physics.Raycast(new Vector3(transform.position.x + 10f, transform.position.y, transform.position.z), -Vector3.up, out hit))
        {
            distanceFromGround = hit.distance;
            Debug.Log(distanceFromGround);

            // When the slope changes too much
            if(distanceFromGround > .8 || distanceFromGround < .7)
            {

            }
        }
    }
}
