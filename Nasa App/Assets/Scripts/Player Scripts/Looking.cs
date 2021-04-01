using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Movement base goes to this video: https://www.youtube.com/watch?v=_QajrabyTJc

public class Looking : MonoBehaviour
{
    float sensitivity = 100f;
    public Transform player;
    private float xRotation = 0f;
    public static bool inLookMode = false;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // Hides the cursor
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))                            /// Katrina Commenting This! This is so that the player can exit Locked-Mouse state and use the GUI.
        {                                                               /// This part in particular switches between look modes, AKA: Whether they are looking as a character, or moving with the mouse for the GUI.
            setLookMode(!inLookMode); // Toggles Look Mode
        }

        if (inLookMode == true)
        {
            // Gets the mouse input
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90); // Stops the player from looking to far up or down

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            player.Rotate(Vector3.up * mouseX); // Rotates around the x axis
        }

        /// There is no code in this part, if inLookMode is false, so that the player can use the cursor freely without moving the screen.
    }

    // Sets if the mouse is locked and invisble or not
    public void setLookMode(bool lookMode)
    {
        if (lookMode) // Locks the mouse
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // Added to make it where it will hide the cusor when not in use
            inLookMode = true;
        }
        else // Unlocks the mouse
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; // Makes cursor visible
            inLookMode = false;
        }
    }

}
