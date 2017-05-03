/*-----------------------------------------------------------------------
--  SOURCE FILE:    obstaclesSetup.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  void Start ()
--                
--  DATE:           May 2, 2017
--
--  DESIGNER:       Aing Ragunathan
--
--  NOTES:
--		            Sets properties of all obstacles on startup.
----------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaclesSetup : MonoBehaviour
{
    
    /**
    * Date:             May 2, 2017
    * Author:           Aing Ragunathan
    * Interface:        void Start ()
    * Description:
    *                   Initializes the obstacles when the objects is created.
    */
    void Start ()
    {
        //change the color of every obstacle in the level
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = Color.grey;
        }
    }
}
