using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;
using System.Globalization;


public class playerMovement : MonoBehaviour {
    public float speed = 6;

	// Use this for initialization, called on first frame the object is active
	void Start () {
        Rigidbody rigidbody = GetComponent<Rigidbody>();    //get the physics of the object
        rigidbody.freezeRotation = true;    //stop the object from rotating
    }   

    private void FixedUpdate()
    {
        float transH = Input.GetAxis("Horizontal");
        float transV = Input.GetAxis("Vertical");

        Vector3 playerInput = new Vector3(transH, transV, Time.deltaTime*speed);
        
        transform.Translate(playerInput);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Walls")
        {
            //restart the game
        }

        //if collision == "obstacles"
            //restart the game
    }
    
}
