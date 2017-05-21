/*-----------------------------------------------------------------------
--  SOURCE FILE:    ScreenManager.cs   
--
--  PROGRAM:        Cave Flier
--
--  FUNCTIONS:
--                         void Start ()
--                  public void previousSlide()
--                  public void nextSlide()
--                  public void setCurrentSlide(int slideNumber)
--                  public void changeSlideInformation()
--                
--  DATE:           May 19, 2017
--
--  DESIGNER:       Jacob Frank
--
--  NOTES:
--		            This script handles the transitioning between different
--                  screens within the tutorial screen.  This script
--                  will display different information to the user
--                  depedant upon the tutorial slide they are viewing.
----------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    private string[] slideTitles;
    private string[] slideText;
    private int currentSlide = 0;
    private int maxSlide;
    private TextMesh titleTextMesh;
    private TextMesh textTextMesh;
    private GameObject tutorialScrn;
    private GameObject previousInstructionBtn;
    private GameObject nextInstructionBtn;

    private const string TUTORIAL_SCREEN_NAME = "Tutorial";

    private const string SLIDE_ONE_TITLE = "Movement";
    private const string SLIDE_TWO_TITLE = "Obstacles";
    private const string SLIDE_THREE_TITLE = "Collision";
    private const string SLIDE_FOUR_TITLE = "Consumables";
    private const string SLIDE_FIVE_TITLE = "Victory";

    private const string SLIDE_ONE_TEXT = "Player will fly forward automatically.\nTilt head left/right to turn left/right.\nLook up/down to move up/down.";
    private const string SLIDE_TWO_TEXT = "Use movement to avoid obstacles.\nFlying near obstacles awards points.";
    private const string SLIDE_THREE_TEXT = "Colliding with obstacles lowers health.\nHead-on collisions result in death";
    private const string SLIDE_FOUR_TEXT = "Use consumables to help you in the level.\nBoost increases your speed.\nBrake reduces your speed.";
    private const string SLIDE_FIVE_TEXT = "Reach the tunnel exit to beat the level.\nThe faster you beat the level,\n the more points you get.";

    private const string NEXT_INSTRUCTION_BUTTON_NAME = "NextInstructionBtn";
    private const string PREVIOUS_INSTRUCTION_BUTTON_NAME = "PreviousInstructionBtn";

    /**
    * Date:             May 19, 2017
    * Author:           Jacob Frank
    * Interface:        void Start ()
    * Description:
    *                   Initializes the slide titles and slide text arrays with the data to be
    *                   displayed in each tutorial slide.
    *                   Stores references to the text fields that require updating for each slide.
    */
    void Start () {
        slideTitles = new string[] { SLIDE_ONE_TITLE, SLIDE_TWO_TITLE, SLIDE_THREE_TITLE, SLIDE_FOUR_TITLE, SLIDE_FIVE_TITLE };
        slideText = new string[] { SLIDE_ONE_TEXT, SLIDE_TWO_TEXT, SLIDE_THREE_TEXT, SLIDE_FOUR_TEXT, SLIDE_FIVE_TEXT };

        if (slideText.Length != slideTitles.Length)
        {
            throw new System.Exception("Slide Count Mismatch"); //The number of slide Titles must equal the number of Slide Texts
        }
        maxSlide = slideText.Length - 1;
        tutorialScrn = GameObject.Find(TUTORIAL_SCREEN_NAME);

        titleTextMesh = tutorialScrn.transform.GetChild(0).transform.Find("Front Wall").transform.GetChild(0).transform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        textTextMesh = tutorialScrn.transform.GetChild(0).transform.Find("Front Wall").transform.GetChild(1).transform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        nextInstructionBtn = GameObject.Find(NEXT_INSTRUCTION_BUTTON_NAME);
        previousInstructionBtn = GameObject.Find(PREVIOUS_INSTRUCTION_BUTTON_NAME);
        changeSlideInformation();
    }

    /**
    * Date:             May 19, 2017
    * Author:           Jacob Frank
    * Interface:        void previousSlide()
    * Description:
    *                   Decreases the value of current slide
    *                   to move tot he previous slide's information,
    *                   then updates the slide info.
    */
    public void previousSlide()
    {
        --currentSlide;
        if (currentSlide < 0)
        {
            currentSlide = 0;
        }
        changeSlideInformation();
    }

   /**
   * Date:             May 19, 2017
   * Author:           Jacob Frank
   * Interface:        void nextSlide()
   * Description:
   *                   Increases the value of current slide
   *                   to move tot he next slide's information,
   *                   then updates the slide info.
   */
    public void nextSlide()
    {
        ++currentSlide;
        if (currentSlide > maxSlide)
        {
            currentSlide = maxSlide;
        }
        changeSlideInformation();
    }

   /**
   * Date:             May 19, 2017
   * Author:           Jacob Frank
   * Interface:        setCurrentSlide(int slideNumber)
   * Description:
   *                   Sets the value of current slide to
   *                   the value specified in slideNumber.
   *                   This enables the forced movement to a 
   *                   specific slide in the tutorial.
   */
    public void setCurrentSlide(int slideNumber)
    {
        if (slideNumber < 0 || slideNumber > maxSlide)
        {
            currentSlide = 0;
        }
        else
        {
            currentSlide = slideNumber;
        }
    }

    /**
   * Date:             May 19, 2017
   * Author:           Jacob Frank
   * Interface:        void changeSlideInformation()
   * Description:
   *                   Updates the tutorial slide with the correct
   *                   information needed to be displayed to the player.
   */
    public void changeSlideInformation()
    {
        titleTextMesh.text = slideTitles[currentSlide]; //Sets the slide title
        textTextMesh.text = slideText[currentSlide]; //Sets the slide text

        switch (currentSlide)
        {
            case 0:
                tutorialScrn.transform.GetChild(1).transform.Find(PREVIOUS_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                tutorialScrn.transform.GetChild(1).transform.Find(NEXT_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                break;
            case 1:
                tutorialScrn.transform.GetChild(1).transform.Find(PREVIOUS_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                tutorialScrn.transform.GetChild(1).transform.Find(NEXT_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                break;
            case 2:
                tutorialScrn.transform.GetChild(1).transform.Find(PREVIOUS_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                tutorialScrn.transform.GetChild(1).transform.Find(NEXT_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                break;
            case 3:
                tutorialScrn.transform.GetChild(1).transform.Find(PREVIOUS_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                tutorialScrn.transform.GetChild(1).transform.Find(NEXT_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                break;
            case 4:
                tutorialScrn.transform.GetChild(1).transform.Find(NEXT_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(false);
                tutorialScrn.transform.GetChild(1).transform.Find(PREVIOUS_INSTRUCTION_BUTTON_NAME).GetComponent<ButtonInteraction>().setActive(true);
                break;
            default:
                break;
        }
    }
}
