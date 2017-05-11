/*-----------------------------------------------------------------------
--  SOURCE FILE:    gameManager.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  void Start ()
--                  private void Update()
--                  void setGameState(GameState newState)
--                  GameState getGameState()
--                
--  DATE:           May 11, 2017
--
--  DESIGNER:       Jay Coughlan
--
--  NOTES:
--		            This script controls the Game's state, the behavior it has during these states, 
--                  and other functions relevant to those behaviors.
----------------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Date:             May 11, 2017
* Author:           Jay Coughlan
* Description:
*                   These represent the Game's available states.
*/
public enum GameState
{
    loading,
    paused,
    running,
    death
}

public class gameManager : MonoBehaviour
{

    public GameState gState = GameState.running;

    /**
    * Date:             May 11, 2017
    * Author:           Jay Coughlan
    * Interface:        void Start()
    * Description:
    *                   Initializes Game Behavior                
    */
    void Start()
    {

    }

    /**
    * Date:             May 11, 2017
    * Author:           Jay Coughlan
    * Interface:        void Update()
    * Description:
    *                   runs game behavior logic                   
    */
    void Update()
    {
        //this is the behavior we want the game to run every Update depending on it's state.
        switch (gState)
        {
            case GameState.loading:
                break;
            case GameState.paused:
                break;
            case GameState.running:
                break;
            case GameState.death:
                break;
        }
    }

    /**
    * Date:             May 11, 2017
    * Author:           Jay Coughlan
    * Interface:        void setGameState(GameState newState)
    * Description:
    *                   Update's the game master's state and runs one-time behavior based on that state.
    */
    public void setGameState(GameState newState)
    {
        //this is the behavior we want the game to run when it has switched, but not every update
        switch (newState)
        {
            case GameState.loading:
                break;
            case GameState.paused:
                break;
            case GameState.running:
                break;
            case GameState.death:
                break;
        }
        //the reason we change the gameState here and not in the beginning is in case one of the states has
        //behviors we want it to do dependant on what state we're changing from.
        gState = newState;
    }

    /**
    * Date:             May 11, 2017
    * Author:           Jay Coughlan
    * Interface:        void setGameState(GameState newState)
    * Description:
    *                   Returns the game master's current state.
    */
    public GameState getGameState()
    {
        return gState;
    }
}
