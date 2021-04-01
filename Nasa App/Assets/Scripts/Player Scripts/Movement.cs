// Credit to shigsy on dev.to for their basic camera control code which was the bases for the moment

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody body;
    public bool rotation;

    // Base move speed
    public float moveSpeed = 150.0f;

    // vector that calucates movement
    private Vector3 moveVector;

    void Start()
    {

        body.freezeRotation = !rotation;

        // Intitalizes the vector
        moveVector = new Vector3(0, 0, 0);
    }

    void FixedUpdate()
    {
        // Gets the input of a user
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.z = Input.GetAxisRaw("Vertical");

        // Only moves the postion when a button is pressed (helps with smoother movement
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // Moves the object
            transform.position += moveSpeed * moveVector * Time.deltaTime;
        }
    }

}
