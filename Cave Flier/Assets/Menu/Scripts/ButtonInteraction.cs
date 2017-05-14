/*-----------------------------------------------------------------------
--  SOURCE FILE:    ButtonInteraction.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  void Start ()
--                  void Update ()
--                  public void PointerEnter()
--                  public void PointerExit()
--                  public void PointerDown()
--                
--  DATE:           May 13, 2017
--
--  DESIGNER:       Jacob Frank
--
--  NOTES:
--		            This script defines the behaviour of the buttons
--                  prefab that are used in Scenes for menu navigation
--                  using Gaze as the input method.
----------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/**
* Date:             May 13, 2017
* Author:           Jacob Frank
* Description:
*                   These represent the different buttons available.
* 
* Note:
*                   When new buttons are created:
*                   1) update the Enum.
*                   2) update the Switch statement in Start()
*                   3) update the Switch statement in PointerDown()
*/
public enum buttons
{
    levelSelect,
    tutorial,
    levelOne
}

public class ButtonInteraction : MonoBehaviour
{

    public float gazeTime = 2f; //How many seconds the player must gaze at the button to select it
    public buttons button;

    private float timer = 0f; //How many seconds the player has been gazing at the button
    private bool gazedAt;
    private Transform progressBar;
    private TextMesh buttonText;
    private Vector3 originalprogressBarScale;
    private Vector3 originalprogressBarPosition;
    private float leftSideOfParentPosition = -0.5f;

    /**
    * Date:             May 13, 2017
    * Author:           Jacob Frank
    * Interface:        void Start ()
    * Description:
    *                   Initializes the button with correct text displayed.              
    */
    void Start ()
    {
        progressBar = gameObject.transform.GetChild(0);
        buttonText = gameObject.transform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        originalprogressBarScale = progressBar.localScale;
        originalprogressBarPosition = progressBar.localPosition;
        if (buttonText != null) //Set the button text if not null
        {
            switch (button)
            {
                case buttons.tutorial:
                    buttonText.text = "Tutorial";
                    break;
                case buttons.levelSelect:
                    buttonText.text = "Level Select";
                    break;
                case buttons.levelOne:
                    buttonText.text = "Level 1";
                    break;
                default:
                    Debug.Log("default");
                    break;
            }
        }
    }

    /**
    * Date:             May 13, 2017
    * Author:           Jacob Frank
    * Interface:        void Update ()
    * Description:
    *                   While player Gazes at a button, the progress bar fills
    *                   until the gazeTime is reached, upon which the button is
    *                   pressed.
    */
    void Update ()
    {
		if(gazedAt) //Checks if button is being gazed at
        {
            timer += Time.deltaTime; //Increases timer
            
            //Sets the scale and position of the progress bar to fill the button from left to right
            Vector3 newScale = new Vector3(timer / gazeTime, progressBar.localScale.y, progressBar.localScale.z);
            Vector3 newPosition = new Vector3(leftSideOfParentPosition + (timer / gazeTime / 2), progressBar.localPosition.y, progressBar.localPosition.z);

            progressBar.localScale = newScale;
            progressBar.localPosition = newPosition;

            if (timer >= gazeTime)
            {
                timer = 0f; //reset the timer
                PointerDown(); //click the button
            }
        }
	}

    /**
    * Date:             May 13, 2017
    * Author:           Jacob Frank
    * Interface:        void PointerEnter ()
    * Description:
    *                   When player's gaze crosair enters button,
    *                   Sets the buttons gazedAt bool to true.
    */
    public void PointerEnter ()
    {
        gazedAt = true;
    }

    /**
    * Date:             May 13, 2017
    * Author:           Jacob Frank
    * Interface:        void PointerExit ()
    * Description:
    *                   sets button's gazedAt bool to false when
    *                   player no longer looking at button.
    *                   Resets progress bar to initial position.
    */
    public void PointerExit ()
    {
        gazedAt = false;
        timer = 0f;

        //Reset pregress bar position and scale
        progressBar.localScale = originalprogressBarScale;
        progressBar.localPosition = originalprogressBarPosition;
    }

    /**
    * Date:             May 13, 2017
    * Author:           Jacob Frank
    * Interface:        void PointerDown ()
    * Description:
    *                   Performs a context-sensitive operation
    *                   dependant upon the button clicked.
    */
    public void PointerDown ()
    {
        switch(button)
        {
            case buttons.tutorial:
                break;
            case buttons.levelSelect:
                SceneManager.LoadScene("Scenes/Master/CaveFlier");
                break;
            case buttons.levelOne:
                break;
            default:
                break;
        }
    }
}
