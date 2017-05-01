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
    
	// Update is called before rendering a frame
	void Update () {
        //transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime);
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
        if (collision.gameObject.name == "wall")
        {
            Destroy(collision.gameObject);  //for testing, destroy the wall
        }
    }
    
}
