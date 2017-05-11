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


public enum PlayerState
{
    pause,
    active,
    victory,
    dead
}

public class playerMovement : MonoBehaviour
{
    public float speed = 3; //standard speed forward movement
    public PlayerState pState = PlayerState.active;

    /**
    * Date:             April 28, 2017
    * Author:           Aing Ragunathan
    * Interface:        void Start ()
    * Description:
    *                   Initializes the player when the object is created.
    */
    private void Start()
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

        //this switch statement determines the actions the player will take during the update function
        switch (pState)
        {
            case PlayerState.active:
                //Accelerometer Input
                transform.Translate(Input.acceleration.x, Input.acceleration.z * 0.5f, Time.deltaTime * speed);
                break;
            case PlayerState.dead:
                break;
            case PlayerState.pause:
                break;
            case PlayerState.victory:
                break;
        }
    }

    /**
    * Date:             May 2, 2017
    * Author:           Alex Zielinski
    * Interface:        void OnCollisionEnter ()
    * Description:
    *                   Updates the object when it comes into contact with another object.
    *                   
    * Revision:         Aing Ragunathan (May 2, 2017) - Updated to work with parent objects.  
    *                   Jay Coughlan (May 3, 2017) - Updated if statement to reflect changed names
    */
    void OnCollisionEnter(Collision collision)
    {
        //Reset the level when a collision is detected with a wall or obstacle
        //test placement is the prefab containing our cubes in the test positions
        if (collision.gameObject.name == "TestWalls" || collision.gameObject.name == "Obstacles" ||
            collision.gameObject.name == "TestPlacement")
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    /**
    * Date:             May 10, 2017
    * Author:           Jay Coughlan
    * Interface:        void setPlayerState(PlayerState)
    * Description:
    *                   Changes the player state and runs one time functions that come with the new state.
    */
    public void setPlayerState(PlayerState state)
    {
        //we determine which state we're switching to, and call any one-time functions that need to happen when that state is switched.

        switch (state)
        {
            case PlayerState.pause:
                break;
            case PlayerState.active:
                break;
            case PlayerState.dead:
                break;
            case PlayerState.victory:
                break;
        }

        //now that that is run, we switch states
        pState = state;
    }

    /**
    * Date:             May 10, 2017
    * Author:           Jay Coughlan
    * Interface:        PlayerState getPlayerState()
    * Description:
    *                   returns the current player state
    */
    public PlayerState getPlayerState()
    {
        return pState;
    }
}
