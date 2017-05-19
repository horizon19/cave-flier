/*-----------------------------------------------------------------------
--  SOURCE FILE:    HUDController.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  void Start ()
--                  
--                
--  DATE:           May 17, 2017
--
--  DESIGNER:       Jay Coughlan
--
--  NOTES:
--		            This script controls the HUD and it's behaviors, updating it's elements
--                  to display for the player.
----------------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    //this is the canvas, the parent of all the elements
    public Canvas canvas;
    public Text healthText;
    public Text pointText;
    public Text timerText;
    public Image bloodImg;

    //player script for future reference.
    private playerMovement pmScript;

    // Use this for initialization
    void Start()
    {
        //we get the playerMovement script instance
        pmScript = (playerMovement)GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent(typeof(playerMovement));

        //if we don't have an attached canvas, fix that
        if (canvas == null)
        {
            canvas = this.GetComponent<Canvas>();
        }

        //if we don't have an attached healthText, fix that
        if (healthText == null)
        {
            healthText = canvas.transform.GetChild(0).GetComponent<Text>();
        }

        //if we don't have an attached pointText, fix that
        if (pointText == null)
        {
            pointText = canvas.transform.GetChild(1).GetComponent<Text>();
        }

        //if we don't have an attached timerText, fix that
        if (timerText == null)
        {
            timerText = canvas.transform.GetChild(2).GetComponent<Text>();
        }

        //if we don't have an attached bloodImage
        {

        }

        //set the initial points and health
        healthText.text = "Health: " + pmScript.getHealth();
        pointText.text = "Score: " + pmScript.getPoints();
        timerText.text = "Time: 0";
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jay Coughlan
    * Interface:        void updatePlayerHealth(int health)
    * Description:
    *                   this updates the text of the HUD element to the latest player health
    */
    public void updatePlayerHealth(int health)
    {
        healthText.text = "Health: " + health;
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jay Coughlan
    * Interface:        void updatePlayerPoints(int points)
    * Description:
    *                   this updates the text of the HUD element to the latest player score
    */
    public void updatePlayerPoints(int points)
    {
        pointText.text = "Score: " + points;
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jay Coughlan
    * Interface:        void updatePlayerTime(int points)
    * Description:
    *                   this updates the text of the HUD element to the latest player time
    */
    public void updatePlayerTime(float time)
    {
        pointText.text = "Time: " + time;
    }
}
