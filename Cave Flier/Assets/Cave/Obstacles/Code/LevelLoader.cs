using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

	// constant values for obstacles
	private const int OBSTACLE_TYPES = 3;
	private const int STALACTITE = 0;
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

	// holds the start of a section and the size of the section
	private float sectionStart;
	private float sectionSize;

	// game objects for the level, obstacles
	private GameObject level;
	private GameObject obstacle;

	// List to hold array of obstacles and consumables
	private List<GameObject> obstacleList = new List<GameObject>();
	private List<GameObject> consumableList = new List<GameObject>();

	// prefab for users to specify
	public GameObject column;
	public GameObject stalactite;
	public GameObject stalagmite;
	public GameObject consumable;

	// variables to allow user to set
	public int obstaclesPerSection;
	public int numberOfSections;
	public int numberOfConsumables;
	public int startBuffer; // buffer so obstacles don't spawn immidiatly at start of level
	public int consumableBuffer; // buffer so consumables don't spawn too close to walls

	// scale values user can set for column 
	public float columnScaleX;
	public float columnScaleY;
	public float columnScaleZ;

	// min and max Y range user can set for stalactites/stalagmites
	public float obstMinScale;
	public float obstMaxScale;


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
		level = GameObject.Find ("ObstacleGen"); // set level object
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

		// for testing
		/*Debug.Log ("Center X: " + lvlCenterX);
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
		Debug.Log ("Section Start: " + sectionStart); */
	}


	private void setColumnRotation()
	{
		int rotation; // random number to set rotation degree

		rotation = Random.Range (-10, 10);

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
			case STALACTITE:
				obstacleList.Add (Instantiate (stalactite.gameObject) as GameObject);
				obstacleList [i].name = "StalactiteObst";
				//Debug.Log (stalactite.name);
				break;

			case STALAGMITE:
				obstacleList.Add(Instantiate(stalagmite.gameObject) as GameObject);
				obstacleList [i].name = "StalagmiteObst";
				//Debug.Log (stalagmite.name);
				break; 	

			case COLUMN:
				obstacleList.Add (Instantiate (column.gameObject) as GameObject);
				obstacleList [i].transform.localScale = new Vector3 (columnScaleX, columnScaleY, columnScaleZ);
				obstacleList [i].transform.Rotate (0, 0, 0);
				obstacleList [i].name = "ColumnObst";
				//Debug.Log (column.name);
				break;
			}

			obstacleList [i].tag = "Obstacle";
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
	 * 		generate an array of consumables (randomly generate consumables).
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
				consumableList.Add(Instantiate(consumable.gameObject) as GameObject);
				consumableList[i].transform.GetChild(0).GetComponent<Renderer> ().material.color = Color.yellow; // set color
                consumableList[i].transform.GetChild(0).name = "boost"; // set name
                break;

			case BRAKE:
				consumableList.Add(Instantiate(consumable.gameObject) as GameObject);
				consumableList[i].transform.GetChild(0).GetComponent<Renderer> ().material.color = Color.cyan; // set color
                consumableList[i].transform.GetChild(0).name = "brake"; // set name
                break;
			}
            consumableList[i].transform.GetChild(0).tag = "Consumable"; // set tag
            consumableList[i].transform.GetChild(0).transform.localScale = new Vector3(3, 3, 3);
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
	 * 		 Randomly spawns obstacles in tunnel.
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

				// generate random X coord for obstacle
				obstacleX = Random.Range (leftWallX + 1, rightWallX - 1);

				// generate Y coord for obstacle
				//obstacleY = Random.Range (bottomWallY + 1, topWallY - 1);
				setObstacleYCoord(obstacleList[obstaclePos]);
				setObstacleYScale(obstacleList[obstaclePos]);

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
	 * 		Randomly spawns consumables in tunnel. Checks if generated coordinates
	 * 		collides with an obstacle. If so then coordinates are generated until 
	 * 		they are valid.
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
				do // keep looping until valid coordinates are generated
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
							cnsmbleZ = Random.Range (sectionStart, sectionStart + sectionSize);
						
						// randomly generate X coordinates
						cnsmbleX = Random.Range (leftWallX + consumableBuffer, rightWallX - consumableBuffer);

						// check for collision (x, y coords of consumable and all obstacles in section)
						if (Mathf.Abs (cnsmbleX - obstacleList [i].transform.position.x) <= 6 &&
						    Mathf.Abs (cnsmbleZ - obstacleList [i].transform.position.z) <= 6)
						{
							collides = true; // collision detected
							//Debug.Log ("----------------HIT"); // for testing
						}
					}
				} while (collides == true);

				//Debug.Log ("---GOOD SPAWN"); // for testing

				// randomly generate Y coordinates
				cnsmbleY = Random.Range (bottomWallY + consumableBuffer, topWallY - consumableBuffer);

				// spawn next consumable from consumable array
				consumableList [consumablePos].transform.position = new Vector3 (cnsmbleX, cnsmbleY, cnsmbleZ);
				consumablesLeft--; // decrement the amount of consumables left
				consumablePos++; // increase the position within consumable array
				collides = true;
			}

			obstaclePos += obstaclesPerSection; // increment the position within obstacle list
			sectionStart += sectionSize; // increment the start of the next section
			currSection++; // increment section counter
			sectionsLeft--; // decrement the amount of sections left
		}

		sectionStart = startZ; // reset start section
	}


	/**********************************************************************************
	 * Function: void setObstacleYCoord(GameObject obstacle)
	 *						GameObject obstacle: obstacle to determine Y coordinate
	 *
	 * Date: May 14, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		Based on the obstacle type passed in the Y coordinate of the obstacle is 
	 * 		set accordingly
	 **********************************************************************************/
	// randomly generate Y coordinates
	private void setObstacleYCoord(GameObject obstacle)
	{
		// check what obstacles was passed in
		if (string.Equals(obstacle.name , "StalactiteObst")) // if obstacle is stalagtite
		{
			obstacleY = topWallY; // set y coord to the top wall
		} 
		else if (obstacle.name == "StalagmiteObst") //if obstacle is stalagmite
		{
			obstacleY = bottomWallY; // set y coord to bottom wall
		} 
		else // if obstacle is coloumn
		{
			obstacleY = bottomWallY; // set y coord bottom wall
		}
	}


	/**********************************************************************************
	 * Function: void setObstacleYCoord(GameObject obstacle)
	 *						GameObject obstacle: obstacle to determine Y scale
	 *
	 * Date: May 14, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		Based on the obstacle type passed in the Y scale of the obstacle is 
	 * 		set accordingly
	 **********************************************************************************/
	private void setObstacleYScale(GameObject obstacle)
	{
		// randomly generate a scale value
		float scaleY = Random.Range (obstMinScale, obstMaxScale);

		// check if obstacle is NOT a column (checking for stalactite or stalagmite)
		if (obstacle.name != "ColumnObst") // if obstacle is stalagtite
		{
			obstacle.transform.localScale = new Vector3 (1, scaleY, 1); // scale obstacle
		} 
	}
}