/**********************************************************************************
 * ObstacLoader.cs
 * 
 * Date: May 12, 2017
 * 
 * Author: Alex Zielinski
 * 
 * Description:
 * 		This script is responsible for adding game obstacles to the tunnel.
 * 		It randomly generates a list of obstacles and then places those
 * 		obstacles randomly throughout the tunner
 **********************************************************************************/

using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLoader : MonoBehaviour 
{
	// constant values for tunnel dimension
	private const int WIDTH = 50;
	private const int LENGTH = 100;
	private const int HEIGHT = 10;
	private const int SECSIZE = 20;

	// constant values for coordinates of walls
	private const int LEFTWALL = -10;
	private const int RIGHTWALL = 10;
	private const int TOPWALL = 4;
	private const int BOTTOMWALL = -4;

	// constant value for initial starting region for obstacles in tunnel
	private const int INITSTART = 10;

	// constant value for the max number of sections within tunnel
	private const int MAXSEC = 10;

	// constant values for obstacle generation
	private const int NUMOFOBSTACLES = 50;
	private const int OBSTACLES = 3;
	private const int STALAGTITE = 0;
	private const int STALAGMITE = 1;
	private const int COLUMN = 2;

	// obstacle object
	private GameObject cube;
	private GameObject wall;

	// offset for section
	private int secOffSet;

	// current section
	private int currSection;

	// constant values for X, Y, Z coordinates
	private int xCoor;
	private int yCoor;
	private int zCoor;

	// list to hold randomly generated obstacles
	private List<GameObject> obstacles = new List<GameObject>();


	/**********************************************************************************
	 * Function: void Start()
	 * 
	 * Date: May 12, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		Use this for initialization
	 **********************************************************************************/
	private void Start () 
	{
		// init variables
		secOffSet = SECSIZE; // set section offset
		currSection = 0; // set the current section
		xCoor = 0;
		yCoor = 0;
		zCoor = 0;

		// generate and spawn obstacles
		generateObstacles ();
		spawnObstacles ();
	}


	/**********************************************************************************
	 * Function: void Update()
	 * 
	 * Date: May 12, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		 Update is called once per frame
 	**********************************************************************************/
	private void Update () 
	{
		
	}


	/**********************************************************************************
	 * Function: generateObstacles ()
	 * 
	 * Date: May 12, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		generate an array of obstacles (randomly generate obstacles
 	**********************************************************************************/
	private void generateObstacles ()
	{
		int obstacleChooser; // holds random integer to determine obstacle

		for (int i = 0; i < NUMOFOBSTACLES; i++) // loop to create 50 objects
		{
			obstacleChooser = Random.Range (0, OBSTACLES); // random number

			// determine what obstacle was selected
			switch (obstacleChooser)
			{
			case STALAGTITE:
				cube = GameObject.CreatePrimitive (PrimitiveType.Cube); // create cube
				cube.GetComponent<Renderer> ().material.color = Color.red; // set color
				obstacles.Add (cube); // add to array
				break;

			case STALAGMITE:
				cube = GameObject.CreatePrimitive (PrimitiveType.Cube); // create cube
				cube.GetComponent<Renderer>().material.color = Color.blue; // set color
				obstacles.Add (cube); // add to array
				break; 	

			case COLUMN:
				cube = GameObject.CreatePrimitive (PrimitiveType.Cube); // create cube
				cube.GetComponent<Renderer>().material.color = Color.green; // set color
				obstacles.Add (cube); // add to array
				break;
			}
		}
	}


	/**********************************************************************************
	 * Function: void setObstacleSize(GameObject obstacle)
	 * 					GameObject obstacle: obstacle to determine obstacle size
	 * 
	 * Date: May 12, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		sets obstacle size based on obstacle object passed in
 	**********************************************************************************/
	private void setObstacleSize(GameObject obstacle)
	{
		// check which obstacle was passed in
		if (obstacle.GetComponent<Renderer> ().material.color == Color.red) // if obstacle is stalagtite
		{
			obstacle.transform.localScale = new Vector3 (1,7, 1); // scale object
		} 
		else if (obstacle.GetComponent<Renderer> ().material.color == Color.blue) // if obstacle is stalagmite
		{
			obstacle.transform.localScale = new Vector3 (1,6, 1); // scale object
		} 
		else // if obstacle is column
		{
			obstacle.transform.localScale = new Vector3 (1,11, 1); // scale object

		}
	}


	/**********************************************************************************
	 * Function: void setYCoord(GameObject obstacle)
	 * 					GameObject obstacle: obstacle to determine y coordinate
	 * 
	 * Date: May 12, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		Sets the y coordinate of the obstacle based on the obstacle
	 **********************************************************************************/
	private void setYCoord(GameObject obstacle)
	{
		if (obstacle.GetComponent<Renderer> ().material.color == Color.red) // if obstacle is stalagtite
		{
			yCoor = TOPWALL + 1; // set y coord to the top wall
		} 
		else if (obstacle.GetComponent<Renderer> ().material.color == Color.blue) //if obstacle is stalagmite
		{
			yCoor = BOTTOMWALL; // set y coord to bottom wall
		} 
		else // if obstacle is coloumn
		{
			yCoor = 0; // set y coord to center
		}
	}


	/**********************************************************************************
	 * Function: void spawnObstacles()
	 * 
	 * Date: May 12, 2017
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		 Randomly spawns obstacles in tunnel
	 **********************************************************************************/
	private void spawnObstacles()
	{
		// integer to represent the position within the obstacle list
		int obstaclePos = 0;

		// loop through level section by section
		while (currSection < MAXSEC)
		{	
			// loop to place 5 obstacles per section
			for (int i = 0; i < 5; i++)
			{	
				if (currSection == 0) // if first section
				{
					zCoor = Random.Range (INITSTART, secOffSet); // create random value for z coordinate
				} else // not first section
				{
					zCoor = Random.Range (secOffSet - SECSIZE, secOffSet); // // create random value for z coordinate
				}

				// create random value for x coordinates
				xCoor = Random.Range (LEFTWALL, RIGHTWALL);

				// set the y coordinate based on the obstacle passed in
				setYCoord (obstacles[obstaclePos]);

				// set the size of obstacle based on the obstacle passed in
				setObstacleSize (obstacles [obstaclePos]);


				// position the cube
				obstacles[obstaclePos].transform.position = new Vector3 (xCoor, yCoor, zCoor);

				// increment position in obstacle list
				obstaclePos++;
			}

			secOffSet += SECSIZE; // go to next section
			currSection++; // increment section counter
		}
	}
}
