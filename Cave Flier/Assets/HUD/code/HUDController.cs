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
            Debug.LogWarning("You have no HUDHealth object. We will try and find one.");
            healthText = canvas.transform.GetChild(0).GetComponent<Text>();

            if (timerText == null)
            {
                Debug.LogWarning("No healthText could be found. Please attach a HUDHealth object to this component");
            }
        }

        //if we don't have an attached pointText, fix that
        if (pointText == null)
        {
            Debug.LogWarning("You have no HUDPoint object. We will try and find one.");
            pointText = canvas.transform.GetChild(1).GetComponent<Text>();
            if (timerText == null)
            {
                Debug.LogWarning("No pointText could be found. Please attach a HUDPoint object to this component");
            }
        }

        //if we don't have an attached timerText, fix that
        if (timerText == null)
        {
            Debug.LogWarning("You have no HUDTime object. We will try and find one.");
            timerText = canvas.transform.GetChild(2).GetComponent<Text>();

            if (timerText == null)
            {
                Debug.LogWarning("No timerText could be found. Please attach a HUDText object to this component");
            }
        }

        //if we don't have an attached bloodImage
        if(bloodImg == null)
        {
            Debug.LogWarning("You have no HUDBlood object. We will try and find one.");
            bloodImg = canvas.transform.GetChild(3).GetComponent<Image>();

            if(bloodImg == null)
            {
                Debug.LogWarning("No bloodImg could be found. Please attach a HUDBlood object to this component");
            }
        }

        //set the initial points and health
        healthText.text = "Health: " + pmScript.getHealth();
        pointText.text = "Score: " + pmScript.getPoints();
        timerText.text = "Time: 0";
        bloodImg.GetComponent<CanvasRenderer>().SetAlpha(0);
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

    /**
    * Date:             May 17, 2017
    * Author:           Jay Coughlan
    * Interface:        void throwBloodSplatter(float time)
    * Description:
    *                   This tells the hud bloodsplatter Image to lerp between 1 and 0 fr it's alpha channel.
    *                   Time is how long we want it to take. generall invinciTimer
    */
    public void throwBloodSplatter(float time)
    {
        //set alpha to 1 for that sudden "AH!"
        bloodImg.GetComponent<CanvasRenderer>().SetAlpha(1f);
        //now we make it lerp. really it's that simple
        bloodImg.CrossFadeAlpha(0, time, true);
    }
}
