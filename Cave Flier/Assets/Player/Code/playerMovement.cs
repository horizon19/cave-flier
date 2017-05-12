/*-----------------------------------------------------------------------
--  SOURCE FILE:    playerMovement.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  void Start ()
--                  private void FixedUpdate()
--                  void OnCollisionEnter(Collision collision)
--                  void calibrateAccelerometer()
--                  Vector3 getAccelerometer(Vector3 accelerator)
--                  void setPlayerState(PlayerState newState)
--                  PlayerState getPlayerState()
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

/**
* Date:             May 11, 2017
* Author:           Jay Coughlan
* Description:
*                   These represent the Player's available states.
*/
public enum PlayerState
{
    pause,
    active,
    victory,
    dead
}

public class playerMovement : MonoBehaviour
{
    public float speed = 10; //standard speed forward movement
    public float speedMax = 17;
    private float levelDistance;
    private float startTime;
    private Vector3 start;
    private Vector3 end;
    private float levelCompleted;
    private bool speedDelay = false;

    public float xRotationSpeed = 3;
    public float yRotationSpeed = 1;
    public float strafingSpeed = 1 / 3;

    public PlayerState pState = PlayerState.active;
    private Matrix4x4 calibrationMatrix;
    private Vector3 wantedDeadZone = Vector3.zero;

    /**
    * Date:             April 28, 2017
    * Author:           Aing Ragunathan
    * Interface:        void Start ()
    * Description:
    *                   Initializes the player when the object is created.
    *                   
    * Revision:         Aing Ragunathan (May 3, 2017) - Updated to calibrate accelerometer.                    
    */
    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();    //get the physics of the object
        rigidbody.freezeRotation = true;    //stop the object from rotating

        calibrateAccelerometer();   //calibrate the accelerometer to prevent drifiting


        start = transform.position;  //get the starting position
        end = GameObject.Find("wallEnd").transform.position;
        levelDistance = Vector3.Distance(transform.position, GameObject.Find("wallEnd").transform.position);
    }

    public void Update()
    {
        //this switch statement determines the actions the player will take during the update function
        switch (pState)
        {
            case PlayerState.active:
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
        float distanceLeft;
        float currentSpeed;
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
                transform.Translate(Input.acceleration.x * strafingSpeed, Input.acceleration.z * 0.5f, 0);  //move the player according to the accelerometer input

                //setup new speed according to how far the player has moved
                distanceLeft = Vector3.Distance(transform.position, end);
                levelCompleted = distanceLeft / levelDistance; //get the percentage of completion
                currentSpeed = speedMax - ((speedMax - speed) * levelCompleted);
                

                transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);  //move the player forward 

			    transform.Rotate(new Vector3(1, 0, 0), -Input.acceleration.z * xRotationSpeed);	//rotate the player when moving up or down
        		transform.Rotate(new Vector3(0, 1, 0), Input.acceleration.x * yRotationSpeed);	//rotate the player when moving from side to side

                
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
    *                   Aing Ragunathan (May 3, 2017) - Updated to calibrate accelerometer.                    
    */
    void OnCollisionEnter(Collision collision)
    {
        //Reset the level when a collision is detected with a wall or obstacle
        //test placement is the prefab containing our cubes in the test positions
        if (collision.gameObject.name == "TestWalls" || collision.gameObject.name == "Obstacles" ||
            collision.gameObject.name == "TestPlacement")
        {
            //transform.position = new Vector3(0, 0, 0);
            speedMax -= 2;
            calibrateAccelerometer(); //re-calibrate accelerometer after death to prevent drifiting
            speedDelay = true;
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

    /**
    * Date:             May 4, 2017
    * Author:           Aing Ragunathan
    * Interface:        void calibrateAccelerometer ()
    * Description:
    *                   Method for calibrating the accelerometer                
    */
    void calibrateAccelerometer()
    {
        wantedDeadZone = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), wantedDeadZone);  //Get the rotation difference between a downward angle and current angle 
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f)); //Setup matrix with rotation to match up with down vector
        calibrationMatrix = matrix.inverse; //Get the inverse of the matrix to reverse the offset 
    }

    /**
    * Date:             May 4, 2017
    * Author:           Aing Ragunathan
    * Interface:        void getAccelerometer ()
    * Description:
    *                   Gets the calibrated input using the real input
    */
    Vector3 getAccelerometer(Vector3 accelerator)
    {
        Vector3 accel = this.calibrationMatrix.MultiplyVector(accelerator);
        return accel;
    }
}
    