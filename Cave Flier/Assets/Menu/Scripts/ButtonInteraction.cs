/*-----------------------------------------------------------------------
--  SOURCE FILE:    ButtonInteraction.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                  public void Start ()
--                  public void Update ()
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
*                   1) Add new Enum to list.
*                   2) update the Switch statement in Start()
*                   3) update the Switch statement in PointerDown()
*                   4) Select the Enum in the editor for the button in the scene
*                   5) Add the button name to the screen manager static variable list
*/
public enum buttons
{
    levelSelect,
    tutorial,
    levelOne,
    replayLevel,
    goToMainMenu,
    backToMainMenu,
    nextInstruction,
    previousInstruction,
    levelTwo,
    levelThree,
    levelFour,
    callibration
}

public class ButtonInteraction : MonoBehaviour
{

    public float gazeTime = 2f; //How many seconds the player must gaze at the button to select it
    public buttons button;
    public bool isActive = true;

    private float timer = 0f; //How many seconds the player has been gazing at the button
    private bool gazedAt;
    private Transform progressBar;
    private TextMesh buttonText;
    private Vector3 originalprogressBarScale;
    private Vector3 originalprogressBarPosition;
    private float leftSideOfParentPosition = -0.5f;
    private ScreenManager smScript;
    private playerMovement pmScript;
    private TutorialManager tmScript;

    private const string LEVEL_ONE_PATH = "Scenes/Master/LevelOne";
    private const string LEVEL_TWO_PATH = "Scenes/Master/LevelTwo";
    private const string LEVEL_THREE_PATH = "Scenes/Master/LevelThree";
    private const string LEVEL_FOUR_PATH = "Scenes/Master/LevelFour";
    private const string MAIN_MENU_PATH = "Scenes/Master/mainMenu";

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

        smScript = (ScreenManager)GameObject.Find("Screen Manager").GetComponent(typeof(ScreenManager));

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
                case buttons.levelTwo:
                    buttonText.text = "Level 2";
                    break;
                case buttons.levelThree:
                    buttonText.text = "Level 3";
                    break;
                case buttons.levelFour:
                    buttonText.text = "Level 4";
                    break;
                case buttons.replayLevel:
                    buttonText.text = "Replay";
                    break;
                case buttons.goToMainMenu:
                    buttonText.text = "Main Menu";
                    break;
                case buttons.callibration:
                    buttonText.text = "Callibrate\nCamera";
                    break;
                case buttons.backToMainMenu:
                    buttonText.text = "Main Menu";
                    break;
                case buttons.nextInstruction:
                    buttonText.text = "Next";
                    break;
                case buttons.previousInstruction:
                    buttonText.text = "Previous";
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
		if(gazedAt && isActive) //Checks if button is being gazed at
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
                smScript.activateScreen(screens.tutorialScreen);
                smScript.deactivateScreen(screens.mainMenuScreen);
                break;
            case buttons.levelSelect:
                smScript.activateScreen(screens.levelSelectScreen);
                smScript.deactivateScreen(screens.mainMenuScreen);
                break;
            case buttons.levelOne:
                SceneManager.LoadScene(LEVEL_ONE_PATH);
                break;
            case buttons.levelTwo:
                SceneManager.LoadScene(LEVEL_TWO_PATH);
                break;
            case buttons.levelThree:
                SceneManager.LoadScene(LEVEL_THREE_PATH);
                break;
            case buttons.levelFour:
                SceneManager.LoadScene(LEVEL_FOUR_PATH);
                break;
            case buttons.replayLevel:
                smScript.activateScreen(screens.gameplayScreen);
                smScript.deactivateScreen(screens.victoryScreen);
                smScript.deactivateScreen(screens.deathScreen);
                if (smScript.getGameplayScreen() != null)
                {
                    pmScript = (playerMovement)GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent(typeof(playerMovement));
                    pmScript.respawn();
                }                
                break;
            case buttons.goToMainMenu:
                SceneManager.LoadScene(MAIN_MENU_PATH);
                break;
            case buttons.callibration:
                smScript.callibratePlayer();
                break;
            case buttons.backToMainMenu:
                if (smScript.getTutorialScreen() != null)
                {
                    tmScript = (TutorialManager)GameObject.Find("Tutorial Manager").GetComponent(typeof(TutorialManager));
                }
                tmScript.setCurrentSlide(0);
                smScript.activateScreen(screens.mainMenuScreen);
                smScript.deactivateScreen(screens.tutorialScreen);
                break;
            case buttons.nextInstruction:
                if (smScript.getTutorialScreen() != null)
                {
                    tmScript = (TutorialManager)GameObject.Find("Tutorial Manager").GetComponent(typeof(TutorialManager));
                }
                tmScript.nextSlide();
                break;
            case buttons.previousInstruction:
                if (smScript.getTutorialScreen() != null)
                {
                    tmScript = (TutorialManager)GameObject.Find("Tutorial Manager").GetComponent(typeof(TutorialManager));
                }
                tmScript.previousSlide();
                break;
            default:
                break;
        }
    }

    public void setActive(bool active)
    {
        this.GetComponent<Collider>().enabled = active;
        this.GetComponent<Renderer>().enabled = active;
        this.transform.GetChild(0).GetComponent<Renderer>().enabled = active;
        this.transform.GetChild(1).GetComponent<Renderer>().enabled = active;
        isActive = active;
    }
}
