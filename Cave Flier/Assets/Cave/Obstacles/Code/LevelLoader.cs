using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

	// constant values for obstacles
	private const int OBSTACLE_TYPES = 3;
	private const int STALAGTITE = 0;
	private const int STALAGMITE = 1;
	private const int COLUMN = 2;

	// constant values for consumables
	private const int CONSUMABLE_TYPES = 2;
	private const int BOOST = 0;
	private const int BRAKE = 1;

	// tunnel dimension variables
	private float lvlCenterX;
	private float lvlCenterY;
	private float lvlCenterZ;
	private float lvlLength;
	private float lvlHeight;
	private float lvlWidth;

	// level coordinate values
	private float leftWallX;
	private float rightWallX;
	private float topWallY;
	private float bottomWallY;
	private float startZ;
	private float endZ;

	// Obstacle and consumable X, Y, Z coordinates
	private float obstacleX;
	private float obstacleY;
	private float obstacleZ;
	private float cnsmbleX;
	private float cnsmbleY;
	private float cnsmbleZ;

	// total amount of obstacles to be rendered in the level
	private int totalObstacles;

	private float sectionStart;
	private float sectionSize;

	// game objects for the level, obstacles
	private GameObject level;
	private GameObject obstacle;
	private GameObject consumable;

	// List to hold array of obstacles and consumables
	private List<GameObject> obstacleList = new List<GameObject>();
	private List<GameObject> consumableList = new List<GameObject>();

	// variables to allow user to set
	public int obstaclesPerSection;
	public int numberOfSections;
	public int numberOfConsumables;
	public int startBuffer; // buffer so obstacles don't spawn immidiatly at start of level
	public int consumableBuffer; // buffer so consumables don't spawn too close to walls


	/**********************************************************************************
	 * Function: start ()
	 * 
	 * Date: May 16, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		Use this for initialization
 	**********************************************************************************/
	void Start () 
	{
		level = GameObject.Find ("LevelBoundingBox"); // set level object
		setLevelProperties(); // instantiate variables
		totalObstacles = obstaclesPerSection * numberOfSections;

		// generate and spawn obstacles
		generateObstacles ();
		spawnObstacles ();

		// generate and spawn consumables
		generateConsumables ();
		spawnConsumable ();
	}


	/**********************************************************************************
	 * Function: start ()
	 * 
	 * Date: May 16, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 	 Update is called once per frame
 	**********************************************************************************/
	void Update () 
	{
		
	}
		

	/**********************************************************************************
	 * Function: setLevelProperties ()
	 * 
	 * Date: May 16, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		Sets tunnel properties regarding the:
	 * 			- center of the level
	 * 			- dimensions of the level
	 * 			- width boundries and height boundries
	 * 			- start and end points for length boundry
 	**********************************************************************************/
	private void setLevelProperties()
	{
		// set center coordinates of level
		lvlCenterX = level.transform.position.x;
		lvlCenterY = level.transform.position.y;
		lvlCenterZ = level.transform.position.z;

		// set dimension of level
		lvlWidth = level.transform.lossyScale.x;
		lvlHeight = level.transform.lossyScale.y;
		lvlLength = level.transform.lossyScale.z;

		// set wall boundry coordinates of level
		leftWallX = lvlCenterX - (lvlWidth / 2);
		rightWallX = lvlCenterX + (lvlWidth / 2);
		topWallY = lvlCenterY + (lvlHeight / 2);
		bottomWallY = lvlCenterY - (lvlHeight / 2);

		// set start and end coordinate of level
		endZ = lvlCenterZ + (lvlLength / 2);
		startZ = lvlCenterZ - (lvlLength / 2);

		// set the section size and the start of the section (start point of tunnel)
		sectionSize = lvlLength / numberOfSections;
		sectionStart = startZ;

		Debug.Log ("Center X: " + lvlCenterX);
		Debug.Log ("Center Y: " + lvlCenterY);
		Debug.Log ("Center Z: " + lvlCenterZ);
		Debug.Log ("Width: " + lvlWidth);
		Debug.Log ("Height: " + lvlHeight);
		Debug.Log ("Length: " + lvlLength);
		Debug.Log ("Left wall: " + leftWallX);
		Debug.Log ("Right wall: " + rightWallX);
		Debug.Log ("Top wall: " + topWallY);
		Debug.Log ("Bottom wall: " + bottomWallY);
		Debug.Log ("End wall: " + endZ);
		Debug.Log ("Start wall: " + startZ);
		Debug.Log ("Section Size: " + sectionSize);
		Debug.Log ("Section Start: " + sectionStart);
	}


	/**********************************************************************************
	 * Function: generateObstacles ()
	 * 
	 * Date: May 12, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		generate a  list of obstacles (randomly generate obstacles).
 	**********************************************************************************/
	private void generateObstacles ()
	{
		int obstacleChooser; // holds random integer to determine obstacle

		for (int i = 0; i < totalObstacles; i++) // loop to create obstacles
		{
			obstacleChooser = Random.Range (0, OBSTACLE_TYPES); // randomly select an obstacle

			// determine what obstacle was generated
			switch (obstacleChooser)
			{
			case STALAGTITE:
				obstacle = GameObject.CreatePrimitive (PrimitiveType.Cube); // create cube
				obstacle.GetComponent<Renderer> ().material.color = Color.red; // set color
				obstacleList.Add (obstacle); // add to array
				break;

			case STALAGMITE:
				obstacle = GameObject.CreatePrimitive (PrimitiveType.Cube); // create cube
				obstacle.GetComponent<Renderer>().material.color = Color.blue; // set color
				obstacleList.Add (obstacle); // add to array
				break; 	

			case COLUMN:
				obstacle = GameObject.CreatePrimitive (PrimitiveType.Cube); // create cube
				obstacle.GetComponent<Renderer>().material.color = Color.green; // set color
				obstacleList.Add (obstacle); // add to array
				break;
			}
		}
	}


	/**********************************************************************************
	 * Function: generateConsumables ()
	 * 
	 * Date: May 14, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		generate an array of consumables (randomly generate consumables)
 	**********************************************************************************/
	private void generateConsumables()
	{
		int consumableChooser; // holds random integer to determine consumable

		for (int i = 0; i < numberOfConsumables; i++) // loop to create consumables
		{
			consumableChooser = Random.Range (0, 2); // random number

			// determine what consumable was generated
			switch (consumableChooser)
			{
			case BOOST:
				consumable = GameObject.CreatePrimitive (PrimitiveType.Sphere); // create sphere
				consumable.GetComponent<Renderer> ().material.color = Color.yellow; // set color
				consumable.transform.localScale = new Vector3 (3,3, 3); // scale object
				consumableList.Add (consumable); // add to array
				break;

			case BRAKE:
				consumable = GameObject.CreatePrimitive (PrimitiveType.Sphere); // create sphere
				consumable.GetComponent<Renderer> ().material.color = Color.cyan; // set color
				consumable.transform.localScale = new Vector3 (3,3, 3); // scale object
				consumableList.Add (consumable); // add to array
				break;
			}
		}
	}


	/**********************************************************************************
	 * Function: void spawnObstacles()
	 * 
	 * Date: May 16, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		 Randomly spawns obstacles in tunnel
	 **********************************************************************************/
	private void spawnObstacles()
	{
		int obstaclePos = 0; // position within the obstacle list
		int currSection = 0; // current section of level

		// iterate thru level sections and spawn obstacles
		while (currSection < numberOfSections)
		{
			// loop to place obstacles per section
			for (int i = 0; i < obstaclesPerSection; i++)
			{	
				// if first section
				if (currSection == 0)
					// include buffer so no obstacles spawn at start of level
					obstacleZ = Random.Range (sectionStart + startBuffer, sectionStart + sectionSize);
				else // spawn normally
					obstacleZ = Random.Range (sectionStart, sectionStart + sectionSize);

				// generate random X and Y values for obstacle
				obstacleX = Random.Range (leftWallX + 1, rightWallX - 1);
				obstacleY = Random.Range (bottomWallY + 1, topWallY - 1);

				// position the cube
				obstacleList[obstaclePos].transform.position = new Vector3 (obstacleX, obstacleY, obstacleZ);
				obstaclePos++;// increment position in obstacle list
			}

			currSection++; // increment section counter
			sectionStart += sectionSize; // increment the start of the next section
		}

		sectionStart = startZ; // reset start section
	}


	/**********************************************************************************
	 * Function: void spawnConsumable()
	 * 
	 * Date: May 16, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		 Randomly spawns consumables in tunnel
	 **********************************************************************************/
	private void spawnConsumable()
	{
		int consumablesLeft = numberOfConsumables;
		int sectionsLeft = numberOfSections - 1; // -1 section so no consumables spawn at the end
		int consumablePos = 0; // position within the consumable list
		int obstaclePos = 0 ; // position within the obstacle list
		int currSection = 0; // current section of level
		int toSpawn = 0; // to determine if consumable spawns in section
		bool collides = true;

		// iterate thru sections and spawn consumables
		while (currSection < numberOfSections)
		{
			toSpawn = Random.Range (0, 2); // random number to determine spawning of consumable

			// check if consumable is to spawn in current section
			if ((toSpawn == 0 || consumablesLeft == sectionsLeft) && consumablesLeft != 0)
			{
				while (collides == true)
				{
					collides = false;
					// generate consumable coords that don't collide with obstacle
					for (int i = obstaclePos; i < (obstaclePos + obstaclesPerSection); i++)
					{
						// if first section
						if (currSection == 0)
							// include buffer so no obstacles spawn at start of level
							cnsmbleZ = Random.Range (sectionStart + startBuffer, sectionStart + sectionSize);
						else // spawn normally
							cnsmbleZ= Random.Range (sectionStart, sectionStart + sectionSize);
						
						// randomly generate X coordinates
						cnsmbleX = Random.Range (leftWallX + consumableBuffer, rightWallX - consumableBuffer);

						// check for collision (x, y coords of consumable and all obstacles in section)
						if (Mathf.Abs(cnsmbleX - obstacleList [i].transform.position.x) <= 2 && 
							Mathf.Abs(cnsmbleZ - obstacleList [i].transform.position.z) <= 2)
						{
							collides = true;
							Debug.Log("----------------HIT");
						}
					}
				}
				Debug.Log ("---GOOD SPAWN");

				// randomly generate Y coordinates
				cnsmbleY = Random.Range (bottomWallY + consumableBuffer, topWallY - consumableBuffer);

				// spawn next consumable from consumable array
				consumableList [consumablePos].transform.position = new Vector3 (cnsmbleX, cnsmbleY, cnsmbleZ);
				consumablesLeft--; // decrement the amount of consumables left
				consumablePos++; // increase the position within consumable array
				collides = true;
			}

			obstaclePos += (obstaclesPerSection - 1);
			sectionStart += sectionSize; // increment the start of the next section
			currSection++; // increment section counter
			sectionsLeft--; // decrement the amount of sections left
		}

		sectionStart = startZ; // reset start section
	}
}