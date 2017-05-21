/*-----------------------------------------------------------------------
--  SOURCE FILE:    ScreenManager.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                         void Start ()
--                  public void activateScreen(screens screen)
--                  public void deactivateScreen(screens screen)
--                  public void deactivateVisibleLayer(string layerToDeactivate)
--                  public void activateVisibleLayer(string layerToActivate)
--                  public void deactivateAllScreens()
--                  public void findAllScreens()
--                
--  DATE:           May 17, 2017
--
--  DESIGNER:       Jacob Frank
--
--  NOTES:
--		            This script handles the transitioning between different
--                  screens within a single screen (ie. moving between menus
--                  or moving from gameplay to a victory screen)
----------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Date:             May 15, 2017
* Author:           Jacob Frank
* Description:
*                   These represent all screens in the game.
* 
* Note:
*                   When new screens are created:
*                   1) Add new Enum to list.
*                   2) Add private variable to store screen as GameObject
*                   3) Add required button, layer, and tag const values before start()
*                   4) Update activateScreen()
*                   5) Update deactivateScreen()
*                   6) Update deactivateAllScreens()
*                   7) Update findAllScreens()
*/
public enum screens
{
    mainMenuScreen,
    levelSelectScreen,
    gameplayScreen,
    victoryScreen,
    deathScreen,
    tutorialScreen
}

public class ScreenManager : MonoBehaviour {

    private GameObject mainMenuScrn;
    private GameObject levelSelectScrn;
    private GameObject gameplayScrn;
    private GameObject victoryScrn;
    private GameObject deathScrn;
    private GameObject tutorialScrn;

    private GameObject cameraPosition; //Parent object of the camera to allow for camera translation
    private GameObject player, playerModel;
    private Camera camera;
    private Camera cameraLeft; //Google VR camera's left eye camera
    private Camera cameraRight; //Google VR camera's right eye camera
    private Camera HudCamera;
    private TextMesh scoreText;
    private playerMovement pmScript;
    private GvrViewer gvrView;

    private const string MAIN_CAMERA_TAG = "MainCamera";
    private const string MAIN_CAMERA_POSITION_TAG = "MainCameraPos";
    private const string LEFT_EYE_TAG = "LeftEye";
    private const string RIGHT_EYE_TAG = "RightEye";
    private const string HUD_CAMERA_TAG = "HudCamera";

    private const string TUTORIAL_BUTTON_NAME = "TutorialBtn";
    private const string LEVEL_SELECT_BUTTON_NAME = "LevelSelectBtn";
    private const string LEVEL_ONE_BUTTON_NAME = "LevelOneBtn";
    private const string LEVEL_TWO_BUTTON_NAME = "LevelTwoBtn";
    private const string LEVEL_THREE_BUTTON_NAME = "LevelThreeBtn";
    private const string LEVEL_FOUR_BUTTON_NAME = "LevelFourBtn";
    private const string REPLAY_LEVEL_BUTTON_NAME = "ReplayBtn";
    private const string MAIN_MENU_BUTTON_NAME = "MainMenuBtn";
    private const string NEXT_INSTRUCTION_BUTTON_NAME = "NextInstructionBtn";
    private const string PREVIOUS_INSTRUCTION_BUTTON_NAME = "PreviousInstructionBtn";
    private const string BACK_TO_MAIN_MENU_BUTTON_NAME = "BackToMainMenuBtn";

    private const string MAIN_MENU_LAYER = "Main Menu Layer";
    private const string LEVEL_SELECT_LAYER = "Level Select Layer";
    private const string GAMEPLAY_LAYER = "Gameplay Layer";
    private const string VICTORY_LAYER = "Victory Layer";
    private const string DEATH_LAYER = "Death Layer";
    private const string HUD_LAYER = "HUDLayer";
    private const string TUTORIAL_LAYER = "Tutorial Layer";

    private const string MAIN_MENU_SCREEN_NAME = "Main Menu";
    private const string LEVEL_SELECT_SCREEN_NAME = "Level Select";
    private const string GAMEPLAY_SCREEN_NAME = "Gameplay";
    private const string VICTORY_SCREEN_NAME = "Victory";
    private const string DEATH_SCREEN_NAME = "Death";
    private const string TUTORIAL_SCREEN_NAME = "Tutorial";
    private const string PLAYER_NAME = "Player";
    private const string PLAYER_MODEL_NAME = "PlayerModel";

    public screens initialScreen; //Screen to be activated upon start (changed in Unity editor)

    /**
    * Date:             May 15, 2017
    * Author:           Jacob Frank
    * Interface:        void Start ()
    * Description:
    *                   Finds and stores references to all cameras and screens within the scene.
    *                   Deactivates all screens and activates the initial defined by the 
    *                   public variable "initialscreen" in the Unity editor.
    * 
    * Revision:
    *                   Jacob Frank (May 17, 2017)
    *                   Seperated out repeating code to individual functions
    */
    void Start () {

        findAllScreens(); //Finds all Screens based on name

        camera = GameObject.FindWithTag(MAIN_CAMERA_TAG).GetComponent<Camera>();
        cameraPosition = GameObject.FindWithTag(MAIN_CAMERA_POSITION_TAG);
        cameraLeft = GameObject.FindWithTag(LEFT_EYE_TAG).GetComponent<Camera>();
        cameraRight = GameObject.FindWithTag(RIGHT_EYE_TAG).GetComponent<Camera>();
        player = GameObject.FindWithTag(PLAYER_NAME);
        playerModel = GameObject.Find(PLAYER_MODEL_NAME);
        gvrView = GameObject.FindWithTag("GVRViewer").GetComponent<GvrViewer>();

        deactivateAllScreens(); //Deactivates all game screens to ensure only one will be active after next call
        activateScreen(initialScreen); //Activates game screen defined by the  public variable "initialscreen" in the Unity editor.
    }

    /**
    * Date:             May 15, 2017
    * Author:           Jacob Frank
    * Interface:        void activateScreen(screens screen)
    * Description:
    *                   Activates a specified Screen within the Unity Scene by:
    *                       1) Activating any buttons to enable gaze input
    *                       2) Changing the camera's position to view the screen
    *                       3) Changing the camera's visible layers
    *                   
    *                   Method needs to be updated when new screens are created:
    *                       1) Add screen to switch statement
    *                       2) Activate buttons in screen
    *                       3) Change the camera's position
    *                       4) Change the camera's visible layers - call activateVisibleLayer()
    * 
    * Revision:
    *                   Jacob Frank (May 16, 2017)
    *                       Added error checking for null screen objects
    *                   Jacob Frank (May 17, 2017)
    *                       Seperated out repeating code to individual functions
    */
    public void activateScreen(screens screen)
    {
        switch (screen)
        {
            case screens.mainMenuScreen:
                if (mainMenuScrn != null)
                {
                    mainMenuScrn.transform.GetChild(1).transform.Find(TUTORIAL_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    mainMenuScrn.transform.GetChild(1).transform.Find(LEVEL_SELECT_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    cameraPosition.transform.position = mainMenuScrn.transform.GetChild(2).transform.position;
                    activateVisibleLayer(MAIN_MENU_LAYER);
                }
                break;
            case screens.levelSelectScreen:
                if (levelSelectScrn != null)
                {
                    levelSelectScrn.transform.GetChild(1).transform.Find(LEVEL_ONE_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    levelSelectScrn.transform.GetChild(1).transform.Find(LEVEL_TWO_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    levelSelectScrn.transform.GetChild(1).transform.Find(LEVEL_THREE_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    levelSelectScrn.transform.GetChild(1).transform.Find(LEVEL_FOUR_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    cameraPosition.transform.position = levelSelectScrn.transform.GetChild(2).transform.position;
                    activateVisibleLayer(LEVEL_SELECT_LAYER);
                }
                break;
            case screens.tutorialScreen:
                if (tutorialScrn != null)
                {
                    tutorialScrn.transform.GetChild(1).transform.Find(NEXT_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    tutorialScrn.transform.GetChild(1).transform.Find(BACK_TO_MAIN_MENU_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    cameraPosition.transform.position = tutorialScrn.transform.GetChild(2).transform.position;
                    activateVisibleLayer(TUTORIAL_LAYER);
                }
                break;
            case screens.gameplayScreen:
                if (gameplayScrn != null)
                {
                    //cameraPosition.transform.position = gameplayScrn.transform.GetChild(2).transform.position;
                    playerModel.transform.position = gameplayScrn.transform.GetChild(2).transform.position;
                    //cameraPosition.transform.localPosition = new Vector3(0, 0, 0.8f);
                    camera.transform.Find("GvrReticlePointer").GetComponent<MeshRenderer>().enabled = false;
                    HudCamera = GameObject.FindWithTag(HUD_CAMERA_TAG).GetComponent<Camera>();
                    HudCamera.enabled = true;
                    activateVisibleLayer(GAMEPLAY_LAYER);
                    activateVisibleLayer(HUD_LAYER);
                }
                break;
            case screens.victoryScreen:
                if (victoryScrn != null)
                {
                    victoryScrn.transform.GetChild(1).transform.Find(REPLAY_LEVEL_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    victoryScrn.transform.GetChild(1).transform.Find(MAIN_MENU_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                    pmScript = (playerMovement)GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent(typeof(playerMovement));
                    scoreText = victoryScrn.transform.GetChild(0).transform.Find("Front Wall").transform.GetChild(0).transform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
                    scoreText.text = "Score: " + pmScript.getFinalScore();

                    //cameraPosition.transform.position = victoryScrn.transform.GetChild(2).transform.position;
                    playerModel.transform.position = victoryScrn.transform.GetChild(2).transform.position;
                    activateVisibleLayer(VICTORY_LAYER);
                }
                break;
            case screens.deathScreen:
                if (deathScrn != null)
                {
                    //cameraPosition.transform.position = deathScrn.transform.GetChild(2).transform.position;
                    playerModel.transform.position = deathScrn.transform.GetChild(2).transform.position;
                    activateVisibleLayer(DEATH_LAYER);
                }
                break;
            default:
                break;
        }
    }

    /**
    * Date:             May 15, 2017
    * Author:           Jacob Frank
    * Interface:        void deactivateScreen(screens screen)
    * Description:
    *                   Deactivates a specified Screen within the Unity Scene by:
    *                       1) Deactivating any buttons to enable gaze input
    *                       3) Disabling the camera's visible layer
    *                   
    *                   Method needs to be updated when new screens are created:
    *                       1) Add screen to switch statement
    *                       2) Deactivate buttons in screen
    *                       4) Change the camera's visible layers - call deactivateVisibleLayer()
    * 
    * Revision:
    *                   Jacob Frank (May 16, 2017)
    *                       Added error checking for null screen objects
    *                   Jacob Frank (May 17, 2017)
    *                       Seperated out repeating code to individual functions
    */
    public void deactivateScreen(screens screen)
    {
        switch (screen)
        {
            case screens.mainMenuScreen:
                if (mainMenuScrn != null)
                {
                    mainMenuScrn.transform.GetChild(1).transform.Find(TUTORIAL_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    mainMenuScrn.transform.GetChild(1).transform.Find(LEVEL_SELECT_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    deactivateVisibleLayer(MAIN_MENU_LAYER);
                }
                break;
            case screens.levelSelectScreen:
                if (levelSelectScrn != null)
                {
                    levelSelectScrn.transform.GetChild(1).transform.Find(LEVEL_ONE_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    levelSelectScrn.transform.GetChild(1).transform.Find(LEVEL_TWO_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    levelSelectScrn.transform.GetChild(1).transform.Find(LEVEL_THREE_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    levelSelectScrn.transform.GetChild(1).transform.Find(LEVEL_FOUR_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    deactivateVisibleLayer(LEVEL_SELECT_LAYER);
                }
                break;
            case screens.tutorialScreen:
                if (tutorialScrn != null)
                {
                    tutorialScrn.transform.GetChild(1).transform.Find(NEXT_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    tutorialScrn.transform.GetChild(1).transform.Find(PREVIOUS_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    tutorialScrn.transform.GetChild(1).transform.Find(BACK_TO_MAIN_MENU_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    deactivateVisibleLayer(TUTORIAL_LAYER);
                }
                break;
            case screens.gameplayScreen:
                if (gameplayScrn != null)
                {
                    camera.transform.Find("GvrReticlePointer").GetComponent<MeshRenderer>().enabled = true;
                    HudCamera = GameObject.FindWithTag(HUD_CAMERA_TAG).GetComponent<Camera>();
                    HudCamera.enabled = false;
                    deactivateVisibleLayer(GAMEPLAY_LAYER);
                    deactivateVisibleLayer(HUD_LAYER);
                    
                }
                break;
            case screens.victoryScreen:
                if (victoryScrn != null)
                {
                    victoryScrn.transform.GetChild(1).transform.Find(REPLAY_LEVEL_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    victoryScrn.transform.GetChild(1).transform.Find(MAIN_MENU_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                    deactivateVisibleLayer(VICTORY_LAYER);
                }
                break;
            case screens.deathScreen:
                if (deathScrn != null)
                {
                    deactivateVisibleLayer(DEATH_LAYER);
                }
                break;
            default:
                break;
        }
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jacob Frank
    * Interface:        void deactivateVisibleLayer(string layerToDeactivate)
    * Description:
    *                   Disables the specified layer for the main camera
    *                   and it's child VR cameras so that it is no longer visible in
    *                   the scene.
    */
    public void deactivateVisibleLayer(string layerToDeactivate)
    {
        camera.cullingMask &= ~(1 << LayerMask.NameToLayer(layerToDeactivate));
        cameraLeft.cullingMask &= ~(1 << LayerMask.NameToLayer(layerToDeactivate));
        cameraRight.cullingMask &= ~(1 << LayerMask.NameToLayer(layerToDeactivate));
        camera.transform.GetComponent<GvrPointerPhysicsRaycaster>().eventMask &= ~(1 << LayerMask.NameToLayer(layerToDeactivate));
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jacob Frank
    * Interface:        void activateVisibleLayer(string layerToActivate)
    * Description:
    *                   Enables the specified layer for the main camera
    *                   and it's child VR cameras so that it is visible in
    *                   the scene.
    */
    public void activateVisibleLayer(string layerToActivate)
    {
        camera.cullingMask |= (1 << LayerMask.NameToLayer(layerToActivate));
        cameraLeft.cullingMask |= (1 << LayerMask.NameToLayer(layerToActivate));
        cameraRight.cullingMask |= (1 << LayerMask.NameToLayer(layerToActivate));
        camera.transform.GetComponent<GvrPointerPhysicsRaycaster>().eventMask |= (1 << LayerMask.NameToLayer(layerToActivate));
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jacob Frank
    * Interface:        void deactivateAllScreens()
    * Description:
    *                   deactivates all screens in the Unity scene, so
    *                   the main camera and it's child VR cameras 
    *                   no longer see them in the scene.
    *                   
    *                   Method needs to be updated when new screens are created:
    *                       1) Add call to deactivateScreen() for new game screen enum created
    */
    public void deactivateAllScreens()
    {
        deactivateScreen(screens.mainMenuScreen);
        deactivateScreen(screens.levelSelectScreen);
        deactivateScreen(screens.gameplayScreen);
        deactivateScreen(screens.victoryScreen);
        deactivateScreen(screens.deathScreen);
        deactivateScreen(screens.tutorialScreen);
    }

    /**
    * Date:             May 17, 2017
    * Author:           Jacob Frank
    * Interface:        void findAllScreens()
    * Description:
    *                   Stores a reference to all screen game objects in the game
    *                   
    *                   Method needs to be updated when new screens are created:
    *                       1) add additional calls to GameObject.Find() for each
    *                       additional screen enum created.
    */
    public void findAllScreens()
    {
        mainMenuScrn = GameObject.Find(MAIN_MENU_SCREEN_NAME);
        levelSelectScrn = GameObject.Find(LEVEL_SELECT_SCREEN_NAME);
        gameplayScrn = GameObject.Find(GAMEPLAY_SCREEN_NAME);
        victoryScrn = GameObject.Find(VICTORY_SCREEN_NAME);
        deathScrn = GameObject.Find(DEATH_SCREEN_NAME);
        tutorialScrn = GameObject.Find(TUTORIAL_SCREEN_NAME);
    }

    /**
    * Date:             May 20, 2017
    * Author:           Jacob Frank
    * Interface:        GameObject getMainMenuScreen()
    * Description:
    *                   Returns the menu screen object
    */
    public GameObject getMainMenuScreen()
    {
        return mainMenuScrn;
    }

    /**
    * Date:             May 20, 2017
    * Author:           Jacob Frank
    * Interface:        GameObject getLevelSelectScreen()
    * Description:
    *                   Returns the level select screen object
    */
    public GameObject getLevelSelectScreen()
    {
        return levelSelectScrn;
    }

    /**
    * Date:             May 20, 2017
    * Author:           Jacob Frank
    * Interface:        GameObject getGameplayScreen()
    * Description:
    *                   Returns the gameplay screen object
    */
    public GameObject getGameplayScreen()
    {
        return gameplayScrn;
    }

    /**
    * Date:             May 20, 2017
    * Author:           Jacob Frank
    * Interface:        GameObject getVictoryScreen()
    * Description:
    *                   Returns the victory screen object
    */
    public GameObject getVictoryScreen()
    {
        return victoryScrn;
    }

    /**
    * Date:             May 20, 2017
    * Author:           Jacob Frank
    * Interface:        GameObject getDeathScreen()
    * Description:
    *                   Returns the death screen object
    */
    public GameObject getDeathScreen()
    {
        return deathScrn;
    }

    /**
    * Date:             May 20, 2017
    * Author:           Jacob Frank
    * Interface:        GameObject getTutorialScreen()
    * Description:
    *                   Returns the tutorial screen object
    */
    public GameObject getTutorialScreen()
    {
        return tutorialScrn;
    }


    /**
    * Date:             May 21, 2017
    * Author:           Jay Coughlan
    * Interface:        void callibratePlayer()
    * Description:
    *                   Allows the user to callibrate their views.
    */
    public void callibratePlayer()
    {
        gvrView.Recenter();
    }
}
