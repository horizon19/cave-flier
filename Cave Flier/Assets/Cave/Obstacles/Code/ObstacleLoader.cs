/***********************************************
 * ObstacLoader.cs
 * 
 * Author: Alex Zielinski
 * 
 * Modified:
 * 
 * Description:
 **********************************************/


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

	private List<GameObject> obstacles = new List<GameObject>();


	private void generateObstacles ()
	{
		int obstacleChooser;

		for (int i = 0; i < NUMOFOBSTACLES; i++)
		{
			obstacleChooser = Random.Range (0, OBSTACLES);
			switch (obstacleChooser)
			{
				case STALAGTITE:
					cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.GetComponent<Renderer> ().material.color = Color.red;
					obstacles.Add (cube);
					break;

				case STALAGMITE:
					cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.GetComponent<Renderer>().material.color = Color.blue;
					obstacles.Add (cube);
					break; 	

				case COLUMN:
					cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.GetComponent<Renderer>().material.color = Color.green;
					obstacles.Add (cube);
					break;
			}
		}
	}

	private void setObstacleSize(GameObject obstacle)
	{
		if (obstacle.GetComponent<Renderer> ().material.color == Color.red)
		{
			obstacle.transform.localScale = new Vector3 (1,7, 1);
		} else if (obstacle.GetComponent<Renderer> ().material.color == Color.blue)
		{
			obstacle.transform.localScale = new Vector3 (1,6, 1);
		} else
		{
			obstacle.transform.localScale = new Vector3 (1,11, 1);

		}
	}



	private void setYCoord(GameObject obstacle)
	{
		if (obstacle.GetComponent<Renderer> ().material.color == Color.red)
		{
			yCoor = TOPWALL + 1;
		} else if (obstacle.GetComponent<Renderer> ().material.color == Color.blue)
		{
			yCoor = BOTTOMWALL;
		} else
		{
			yCoor = 0;
		}
	}

	/*********************************************************
	 * Function: void Start()
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		Use this for initialization
	 *********************************************************/
	private void Start () 
	{
		wall = GameObject.Find ("wallTop");
		Debug.Log (wall.transform.lossyScale.z);
		secOffSet = SECSIZE; // set section offset
		currSection = 0; // set the current section
		xCoor = 0;
		yCoor = 0;
		zCoor = 0;
		generateObstacles ();
		spawnObstacles ();
	}

	/*********************************************************
	 * Function: void Update()
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		 Update is called once per frame
	 *********************************************************/
	private void Update () 
	{
		
	}

	/*********************************************************
	 * Function: void spawnObstacles()
	 * 
	 * Programmer: Alex Zielinski
	 * 
	 * Description:
	 * 		 Randomly spawns obstacles in tunnel
	 *********************************************************/
	private void spawnObstacles()
	{
		int obstaclePos = 0;

		// loop through level section by section
		while (currSection < MAXSEC)
		{	// loop to place 5 obstacles per section
			for (int i = 0; i < 5; i++)
			{	
				if (currSection == 0) // if first section
				{
					zCoor = Random.Range (INITSTART, secOffSet); // create random value for z coordinate
				} else // not first section
				{
					zCoor = Random.Range (secOffSet - SECSIZE, secOffSet); // // create random value for z coordinate
				}

				// create random values for x and y coordinates
				xCoor = Random.Range (LEFTWALL, RIGHTWALL);
				setYCoord (obstacles[obstaclePos]);
				setObstacleSize (obstacles [obstaclePos]);


				// position the cube
				obstacles[obstaclePos].transform.position = new Vector3 (xCoor, yCoor, zCoor);

				obstaclePos++;
			}

			secOffSet += SECSIZE; // go to next section
			currSection++; // increment section counter
		}
	}
}
