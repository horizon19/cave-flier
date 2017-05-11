/***********************************************
 * ObstacLoader.cs
 * 
 * Author: Alex Zielinski
 * 
 * Modified:
 * 
 * Description:
 **********************************************/

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
		spawnObstacles();
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
				yCoor = Random.Range (TOPWALL, BOTTOMWALL);

				// create cube object
				cube = GameObject.CreatePrimitive (PrimitiveType.Cube);

				// position the cube
				cube.transform.position = new Vector3 (xCoor, yCoor, zCoor);

				cube.transform.localScale = new Vector3 (2, 2, 2);
			}

			secOffSet += SECSIZE; // go to next section
			currSection++; // increment section counter
		}
	}
}
