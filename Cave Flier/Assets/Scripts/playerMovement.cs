/*-----------------------------------------------------------------------
--  SOURCE FILE:    playerMovement.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  void Start ()
--                  private void FixedUpdate()
--                  void OnCollisionEnter(Collision collision)
--                
--  DATE:           April 28, 2017
--
--  DESIGNER:       Aing Ragunathan
--
--  NOTES:
--		            This script describes how a player flies through a level
--                  in the game using a mobile devie with an accelerometer and 
--                  also what happens when the player collides with an object.
----------------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;
using System.Globalization;


public class playerMovement : MonoBehaviour
{
    public float speed = 3; //standard speed forward movement

    /**
    * Date:             April 28, 2017
    * Author:           Aing Ragunathan
    * Interface:        void Start ()
    * Description:
    *                   Initializes the player when the object is created.
    */
    private void Start ()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();    //get the physics of the object
        rigidbody.freezeRotation = true;    //stop the object from rotating
    }

    /**
    * Date:             April 28, 2017
    * Author:           Aing Ragunathan
    * Interface:        void FixedUpdate ()
    * Description:
    *                   Updates the object's physics and positioning before rendering.
    *                   
    * Revisions:        Aing Ragunathan (May 2, 2017) - Updated to work with accelerometer
    */

    private void FixedUpdate()
    {
        /* Keyboard input
        float transH = Input.GetAxis("Horizontal");
        float transV = Input.GetAxis("Vertical");

        Vector3 playerInput = new Vector3(transH, transV, Time.deltaTime*speed);
        
        transform.Translate(playerInput);
        */

        //Accelerometer Input
        transform.Translate(Input.acceleration.x, Input.acceleration.z * 0.1f,  Time.deltaTime * speed);
    }

    /**
    * Date:             May 2, 2017
    * Author:           Alex Zielinski
    * Interface:        void OnCollisionEnter ()
    * Description:
    *                   Updates the object when it comes into contact with another object.
    *                   
    * Revision:         Aing Ragunathan (May 2, 2017) - Updated to work with parent objects.  
    */
    void OnCollisionEnter(Collision collision)
    {
        //Reset the level when a collision is detected with a wall or obstacle
        if (collision.gameObject.name == "Walls" || collision.gameObject.name == "Obstacles")
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

}
