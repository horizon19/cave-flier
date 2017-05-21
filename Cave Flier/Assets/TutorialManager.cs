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

    private const string TUTORIAL_SCREEN_NAME = "Tutorial";

    private const string SLIDE_ONE_TITLE = "Movement";
    private const string SLIDE_TWO_TITLE = "Obstacles";
    private const string SLIDE_THREE_TITLE = "Collision";
    private const string SLIDE_FOUR_TITLE = "Consumables";
    private const string SLIDE_FIVE_TITLE = "Victory";

    private const string SLIDE_ONE_TEXT = "Slide One Text";
    private const string SLIDE_TWO_TEXT = "Slide Two Text";
    private const string SLIDE_THREE_TEXT = "Slide Three Text";
    private const string SLIDE_FOUR_TEXT = "Slide Four Text";
    private const string SLIDE_FIVE_TEXT = "Slide Five Text";

    // Use this for initialization
    void Start () {
        slideTitles = new string[] { SLIDE_ONE_TITLE, SLIDE_TWO_TITLE, SLIDE_THREE_TITLE, SLIDE_FOUR_TITLE, SLIDE_FIVE_TITLE };
        slideText = new string[] { SLIDE_ONE_TEXT, SLIDE_TWO_TEXT, SLIDE_THREE_TEXT, SLIDE_FOUR_TEXT, SLIDE_FIVE_TEXT };

        if (slideText.Length != slideTitles.Length)
        {
            throw new System.Exception("Slide Count Mismatch");
            //The number of slide Titles must equal the number of Slide Texts
        }
        maxSlide = slideText.Length - 1;
        tutorialScrn = GameObject.Find(TUTORIAL_SCREEN_NAME);

        titleTextMesh = tutorialScrn.transform.GetChild(0).transform.Find("Front Wall").transform.GetChild(0).transform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        textTextMesh = tutorialScrn.transform.GetChild(0).transform.Find("Front Wall").transform.GetChild(1).transform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void previousTutorial()
    {
        --currentSlide;
        if (currentSlide < 0)
        {
            currentSlide = 0;
        }
    }

    public void nextSlide()
    {
        ++currentSlide;
        if (currentSlide > maxSlide)
        {
            currentSlide = maxSlide;
        }
    }

    public void changeSlide()
    {
        titleTextMesh.text = slideTitles[currentSlide];
        textTextMesh.text = slideText[currentSlide];
    }
}
