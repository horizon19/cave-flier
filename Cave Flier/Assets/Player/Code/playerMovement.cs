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
	public int finalScore = 0;
    public float currentSpeed;
    public float speedLevelMax = 17;
    public float speedMin = 10;
    public float speedMax;
    public float maxTurn;
    public GameObject HUDCanvas;


    private Vector3 startPosition;
    private Vector3 startRotation;
    private float levelDistance;
    private Matrix4x4 calibrationMatrix;
    private Vector3 wantedDeadZone = Vector3.zero;
    private HUDController hudScript;
    private collisionDetection cdScript;
	private ScreenManager smScript;
	private String endObjectName = "EndVolume";
	private float totalTime;
    private bool recordTime = false;
	

    [SerializeField] private float invincCounter = 0;

    /**
    * Date:             April 28, 2017
    * Author:           Aing Ragunathan
    * Interface:        void Start ()
    * Description:
    *                   Initializes the player when the object is created.
    *                   
    * Revision:         Aing Ragunathan (May 3, 2017) - Updated to calibrate accelerometer.                    
	*					Aing Ragunathan (May 18, 2017) - Reset points variables
    */
    private void Start()
    {
        if (HUDCanvas == null)
        {
            Debug.LogWarning("Dude, you didn't attach a HUDCanvas. We will try and find one.");
            HUDCanvas = GameObject.Find("HUDCanvas");
            if(HUDCanvas == null)
            {
                Debug.LogError("Dude, I couldn't find it. Please attach one.");
            }
            else
            {
                hudScript = HUDCanvas.GetComponent<HUDController>();
            }
        }
        else
        {
            hudScript = HUDCanvas.GetComponent<HUDController>();
        }

        cdScript = GameObject.Find("Collider").transform.GetChild(0).GetComponent<collisionDetection>();
		smScript = (ScreenManager)GameObject.Find("Screen Manager").GetComponent(typeof(ScreenManager));
        Rigidbody rigidbody = GetComponent<Rigidbody>();    //get the physics of the object
        rigidbody.freezeRotation = true;    //stop the object from rotating
        startPosition = transform.position;  //get the starting position
        startRotation = new Vector3(0, 0, 1);//transform.forward; //get the starting angle of rotation
		levelDistance = Vector3.Distance(transform.position, GameObject.Find(endObjectName).transform.position);
        speedMax = speedLevelMax;   //set the starting max speed

        playerHealth = startHealth;
		totalTime = 0;
		playerPoints = 0;
		finalScore = 0;
        recordTime = false;
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
        getFinalScore();

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
	*					Aing Ragunathan (May 12, 2017) - Update timer when player is active
    */
    private void FixedUpdate()
    {
        currentAcceleration = Input.acceleration;

        //this switch statement determines the actions the player will take during the update function
        switch (pState)
        {

           case PlayerState.active:
                movement(updateSpeed()); //update the player's current position
                if (recordTime)//we only want to record while we're in the level, not approaching or victorying
                {
                    totalTime += Time.deltaTime;    //update the total time
                }
                hudScript.updatePlayerTime(totalTime);
                break;
            case PlayerState.damaged:
                movement(speedMin); //update the player's current position
                if (recordTime)//we only want to record while we're in the level, not approaching or victorying
                {
                    totalTime += Time.deltaTime;
                }
                hudScript.updatePlayerTime(totalTime);
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
        if (deltaRotation < maxTurn)
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
    *
    *                   Turns the object according to the x axis 
	* Revision:			Aing Ragunathan (May 18, 2017) - decreased the deadzone by 0.01f		
    */
    public void moveX()
    {
        float movement = Input.acceleration.x;
        float yRotationSpeed = currentSpeed / 4;
		float deadZone = 0.02f;

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


    public void bounce()
    {
        transform.Translate(new Vector3(0, 1, 0) * -Input.acceleration.z * 20);  //makes up and down turning feel more natural
        transform.Rotate(new Vector3(0, 1, 0), -Input.acceleration.x * 20);  //rotate the player when moving from side to side             
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
		Vector3 endObject = GameObject.Find(endObjectName).transform.position;

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
	*
	* Revision:			Aing Ragunathan (May 18, 2017) - Update speed by same amount as a normal obstacle collision.	
    */
    public void consume(GameObject consumable)
    {
        switch (consumable.name)
        {
            case "boost":
				speedMax = speedMax - ((speedMax - speedMin) / playerHealth);   //assert that playerHealth is greater than 0
                Debug.Log("boost");
                break;
            case "brake":
				speedMax = speedMax - ((speedMax - speedMin) / playerHealth);   //assert that playerHealth is greater than 0
                Debug.Log("brake");
                break;
        }

        consumable.GetComponent<SphereCollider>().enabled = false; //disable the consumable object to avoid repeat collision detections
    }

    /**
    * Date:             May 10, 2017
    * Author:           Jay Coughlan
    * Interface:        void setPlayerState(PlayerState)
    * Description:
    *                   Changes the player state and runs one time functions that come with the new state.
	*
	* Revision:			Aing Ragunathan	(May 18, 2017) - moved speed update when damaged to damaged state
    */
    public void setPlayerState(PlayerState state)
    {
        //we determine which state we're switching to, and call any one-time functions that need to happen when that state is switched.
        switch (state)
        {
            case PlayerState.pause:
                recordTime = false;
                break;
            case PlayerState.active:
                break;
            case PlayerState.damaged:
				speedMax = speedMax - ((speedMax - speedMin) / playerHealth);   //assert that playerHealth is greater than 0
                //activate the HUD's bloodsplatter effect
                hudScript.throwBloodSplatter(invincTimer);
                break;
            case PlayerState.dead:
                smScript.activateScreen(screens.deathScreen);
                smScript.deactivateScreen(screens.gameplayScreen);
                recordTime = false;
                break;
            case PlayerState.victory:
                calcFinalScore();
                smScript.activateScreen(screens.victoryScreen);
                smScript.deactivateScreen(screens.gameplayScreen);
                recordTime = false;
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
        totalTime = 0;//reset total time
        setPlayerState(PlayerState.active); //reset the player to an alive state again
        totalTime = 0;
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
    * Interface:        void setPoints()
    * Description:
    *                   set's the player's points to the specified amount
    */
    public void setPoints(int points)
    {
        playerPoints = points;
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
        //return finalScore;
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
    * Date:             May 18, 2017
    * Author:           Aing Ragunathan
    * Interface:        float getFinalScore()
    * Description:
    *                   Returns the final score
    * Revision:         Aing Ragunathan (May 20, 2017) - 
    */
	public int getFinalScore()
	{
        if ((int) totalTime != 0)
        {
            finalScore = playerPoints * ((int)levelDistance / (int) totalTime);
            //return finalScore;
        }
        else {
            finalScore = -1;
        }


        return finalScore;
	}

    /**
    * Date:             May 19, 2017
    * Author:           Jay Coughlan
    * Interface:        void startTimer()
    * Description:
    *                   it switches the bool recordTimer to true, which will make it begin recording in fixed update.
    *                   it also sets the totalTime to 0 since this should only be run when the start volume is hit.
    */
    public void startTimer()
    {
        totalTime = 0;
        recordTime = true;
    }

    /**
    * Date:             May 19, 2017
    * Author:           Jay Coughlan
    * Interface:        void endTimer()
    * Description:
    *                   this switches the bool recordTime to false, which stops it from recording.
    */
    public void endTimer()
    {
        recordTime = false;
    }
}
