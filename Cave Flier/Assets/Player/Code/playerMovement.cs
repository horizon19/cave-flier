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
    damaged,
    victory,
    dead
}

public class playerMovement : MonoBehaviour
{
    public Vector3 currentAcceleration;
    public Vector3 camDirection;

    public PlayerState pState = PlayerState.active;
    public int startHealth = 3;
    public int playerHealth;
    public int invincTimer = 5;
    public int playerPoints = 0;
    public float currentSpeed;
    public float speedLevelMax = 17;
    public float speedMin = 10;
    public float speedMax;
    public float consumableSpeedDif = 5;
    public String endObjectName = "wallEnd";
    public float maxTurn;


    private Vector3 startPosition;
    private Vector3 startRotation;
    private float levelDistance;
    private Vector3 endObject;
    private Matrix4x4 calibrationMatrix;
    private Vector3 wantedDeadZone = Vector3.zero;
    private HUDController hudScript;
    private collisionDetection cdScript;

    [SerializeField] private float invincCounter = 0;

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
		hudScript = this.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<HUDController>();
        cdScript = this.transform.GetChild(3).transform.GetChild(0).GetComponent<collisionDetection>();
        Rigidbody rigidbody = GetComponent<Rigidbody>();    //get the physics of the object
        rigidbody.freezeRotation = true;    //stop the object from rotating

        calibrateAccelerometer();   //calibrate the accelerometer to prevent drifiting


        startPosition = transform.position;  //get the starting position
        startRotation = new Vector3(0, 0, 1);//transform.forward; //get the starting angle of rotation
        endObject = GameObject.Find(endObjectName).transform.position;    //get the 
        levelDistance = Vector3.Distance(transform.position, GameObject.Find("wallEnd").transform.position);
        speedMax = speedLevelMax;

        playerHealth = startHealth;

        Debug.Log(transform.forward);
    }

    /**
    * Date:             May 11, 2017
    * Author:           Jay Coughlan
    * Interface:        void Update ()
    * Description:
    *                   Consists of states and function calls for the main game loop
*/
    public void Update()
    {
        //this switch statement determines the actions the player will take during the update function
        switch (pState)
        {
            case PlayerState.active:
                //ALL MOVEMENT IS IN FIXED UPDATE.
                break;
            case PlayerState.damaged:
                //we add the time between update function runs
                invincCounter += Time.deltaTime;
                if (invincCounter >= invincTimer)
                {
                    //once we surpass the counter, we set the player state and allow him to be damaged.
                    invincCounter = 0;
                    setPlayerState(PlayerState.active);
                }
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
    *                   Aing Ragunathan (May 12, 2017) - Moved the player movement to another function, added new speed calculation
*/
    private void FixedUpdate()
    {

        currentAcceleration = Input.acceleration;

        //this switch statement determines the actions the player will take during the update function
        switch (pState)
        {
            case PlayerState.active:
                movement(updateSpeed()); //update the player's current position
                break;
            case PlayerState.damaged:
                movement(speedMin); //update the player's current position
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
    * Date:             May 12, 2017
    * Author:           Aing Ragunathan
    * Interface:        void movement()
    * Description:
    *                   Updates the object's physics and positioning before rendering.
    *

    * Revisions:        Aing Ragunathan (May 15, 2017) - Restricted turning according to input
    *                   Aing Ragunathan (May 16, 2017) - Updated restrictions check forward angle with Vector angles instead
*/
    public void movement(float speed)
    {
        float deltaRotation;
        Vector3 deltaCrossProduct;

        deltaRotation = Vector3.Angle(transform.forward, startRotation);    //get the difference in angle between the current and starting angles
        deltaCrossProduct = Vector3.Cross(transform.forward, startRotation);    //get the direction of turning
        transform.Translate(Vector3.forward * Time.deltaTime * speed);  //move the player forward 

        //normal movement
        if (deltaRotation < maxTurn)// && deltaCrossProduct.y > 0 && Input.acceleration.z < 0)
        {
            moveX();
            moveY();
        }
        //restriction for turning left
        else if (deltaCrossProduct.y > 0 && Input.acceleration.x > 0)
        {
            moveX();
        }
        //restriction for turning right
        else if (deltaCrossProduct.y < 0 && Input.acceleration.x < 0)
        {
            moveX();
        }
        //restriction for turning up
        else if (deltaCrossProduct.x > 0 && Input.acceleration.z < 0)

        {
            moveY();
        }
        //restriction for turning down
        else if (deltaCrossProduct.x < 0 && Input.acceleration.z > 0)
        {
            moveY();
        }
    }

    /**
    * Date:             May 15, 2017
    * Author:           Aing Ragunathan
    * Interface:        void moveX()
    * Description:
    *                   Turns the object according to the x axis 
*/
    public void moveX()
    {
        float movement = Input.acceleration.x;
        float yRotationSpeed = currentSpeed / 4;
        float deadZone = 0.03f;

        if (movement > deadZone || movement < -deadZone)
        {
            transform.Rotate(new Vector3(0, 1, 0), movement * yRotationSpeed);  //rotate the player when moving from side to side             
        }
    }

    /**
    * Date:             May 15, 2017
    * Author:           Aing Ragunathan
    * Interface:        void moveY()
    * Description:
    *                   Turns the object according to the y axis 
*/
    public void moveY()
    {
        float xRotationSpeed = currentSpeed / 25;


        //transform.Translate(0, Input.acceleration.z * 0.5f, 0);  //makes up and down turning feel more natural
        transform.Translate(0, Input.acceleration.z * xRotationSpeed, 0);  //makes up and down turning feel more natural
                                                                           //transform.Rotate(new Vector3(1, 0, 0), -Input.acceleration.z * xRotationSpeed); //rotate the player when moving up or down               
    }

    /**
    * Date:             May 12, 2017
    * Author:           Aing Ragunathan
    * Interface:        void updateSpeed()
    * Description:
    *                   Calculates the new speed of a the player
*/
    public float updateSpeed()
    {
        float distanceLeft;
        float levelCompleted;

        distanceLeft = Vector3.Distance(transform.position, endObject);   //setup new speed according to how far the player has moved
        levelCompleted = distanceLeft / levelDistance; //get the percentage of completion
        currentSpeed = speedMax - ((speedMax - speedMin) * levelCompleted); //calculates a new speed for the player's forward movement

        return currentSpeed;
    }

    /**
    * Date:             May 17, 2017
    * Author:           Aing Ragunathan
    * Interface:        void consume(GameObject consumable)
    * Description:
    *                   Updates the max speed of the player when a boost consumabled is obtained.
*/
    public void consume(GameObject consumable)
    {
        switch (consumable.name)
        {
            case "boost":
                speedMax += consumableSpeedDif;
                Debug.Log("boost");
                break;
            case "brake":
                speedMax -= consumableSpeedDif;
                Debug.Log("brake");
                break;
        }

        consumable.GetComponent<SphereCollider>().enabled = false; //disable the consumable object
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
    *                   Aing Ragunathan (May 12, 2017) - Moved speed changes to lowerHealth()
*/
    void OnCollisionEnter(Collision collision)
    {
        //Reset the level when a collision is detected with a wall or obstacle
        //test placement is the prefab containing our cubes in the test positions

        if (collision.gameObject.name == "TestWalls" || collision.gameObject.name == "Obstacles" ||
            collision.gameObject.name == "TestPlacement")
        {
            calibrateAccelerometer(); //re-calibrate accelerometer after death to prevent 
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
            case PlayerState.damaged:
                break;
            case PlayerState.dead:
                respawn();
                state = PlayerState.active;
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

    /**
    * Date:             May 11, 2017
    * Author:           Jay Coughlan
    * Interface:        void lowerHealth(int)
    * Description:
    *                   Lowers the player's health by the inputted integer, and handles if the player dies or is just damaged.
*/
    public void lowerHealth(int damage)
    {
        //check to make sure we're in a state where damage can be done
        if (pState == PlayerState.active)
        {
            speedMax = speedMax - ((speedMax - speedMin) / playerHealth);   //assert that playerHealth is greater than 0
            playerHealth -= damage;

            //if we have 0 or less, we're dead
            if (playerHealth <= 0)
            {
                playerHealth = 0;
                setPlayerState(PlayerState.dead);
            }
            else
            {
                //otherwise we're just damaged
                setPlayerState(PlayerState.damaged);
            }
        }

        hudScript.updatePlayerHealth(playerHealth);
    }

    /**
    * Date:             May 15, 2017
    * Author:           Jay Coughlan
    * Interface:        int getHealth()
    * Description:
    *                   Simple health getter.
    */
    public int getHealth()
    {
        return playerHealth;
    }

    /**
    * Date:             May 12, 2017
    * Author:           Aing Ragunathan
    * Interface:        void respawn ()
    * Description:
    *                   Resets the player's attributes so it can start a new level.
    */
    public void respawn()
    {
        cdScript.purgeCollisions();
        playerHealth = startHealth; //reset the player's health
        hudScript.updatePlayerHealth(playerHealth);
        setPoints(0);//reset the players points
        hudScript.updatePlayerPoints(getPoints());
        transform.position = startPosition; //reset the player's location
        transform.localEulerAngles = startRotation; //reset the player's rotation angles
        speedMax = speedLevelMax;   //reset the max potential speed
        setPlayerState(PlayerState.active); //reset the player to an alive state again
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jay Coughlan
    * Interface:        void addPoints (int mult = 1)
    * Description:
    *                   Adds points to the player score based on the input multiplyer. The default
    *                   for the multiplyer is 1, giving the player 10 points. This also calls the HUD to update.
    */
    public void addPoints(int mult = 1)
    {
        playerPoints += 10 * mult;
        hudScript.updatePlayerPoints(playerPoints);
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jay Coughlan
    * Interface:        int getPoints()

   * Description:
    *                   Returns the current points the player has.
    */
    public int getPoints()
    {
        return playerPoints;
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jay Coughlan
    * Interface:        void setPoints()
    * Description:
    *                   set's the player's points to the specified amount
    */
    public void setPoints(int points)
    {
        playerPoints = points;
    }
}