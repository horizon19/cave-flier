using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public int playerHealth;
    public int playerPoints;

    public Canvas canvas;
    public Text healthText;
    public Text pointText;

    private playerMovement pmScript;

	// Use this for initialization
	void Start ()
    {
        pmScript = (playerMovement)GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent(typeof(playerMovement));

        playerHealth = pmScript.getHealth();

        if(canvas == null)
        {
            canvas = this.GetComponent<Canvas>();
        }

        if(healthText == null)
        {
            canvas.transform.GetChild(0).GetComponent<Text>();
        }

        healthText.text = "Health: " + playerHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void updatePlayerHealth(int health)
    {
        healthText.text = "Health: " + health;
    }
}
